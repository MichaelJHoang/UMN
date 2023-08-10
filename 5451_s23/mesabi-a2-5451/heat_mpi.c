#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>

/* HEAT TRANSFER SIMULATION
   
   Simple physical simulation of a rod connected at the left and right
   ends to constant temperature heat/cold sources.  All positions on
   the rod are set to an initial temperature.  Each time step, that
   temperature is altered by computing the difference between a cells
   temperature and its left and right neighbors.  A constant k
   (thermal conductivity) adjusts these differences before altering
   the heat at a cell.  Use the following model to compute the heat
   for a position on the rod according to the finite difference
   method.

      left_diff  = H[t][p] - H[t][p-1];
      right_diff = H[t][p] - H[t][p+1];
      delta = -k*( left_diff + right_diff )
      H[t+1][p] = H[t][p] + delta
   
   Substituting the above, one can get the following

     H[t+1][p] = H[t][p] + k*H[t][p-1] - 2*k*H[t][p] + k*H[t][p+1]

   The matrix H is computed for all time steps and all positions on
   the rod and displayed after running the simulation.  The simulation
   is run for a fixed number of time steps rather than until
   temperatures reach steady state.
*/

int main(int argc, char **argv){
  if(argc < 4){
    printf("usage: %s max_time width print\n", argv[0]);
    printf("  max_time: int\n");
    printf("  width: int\n");
    printf("  print: 1 print output, 0 no printing\n");
    return 0;
  }

  int max_time = atoi(argv[1]); // Number of time steps to simulate
  int width = atoi(argv[2]);    // Number of cells in the rod
  int print = atoi(argv[3]);
  double initial_temp = 50.0;   // Initial temp of internal cells 
  double L_bound_temp = 20.0;   // Constant temp at Left end of rod
  double R_bound_temp = 10.0;   // Constant temp at Right end of rod
  double k = 0.5;               // thermal conductivity constant
  double **H;                   // 2D array of temps at times/locations 


  int numProcs;
  int pid;
  int hostLen;
  char host[256];

  MPI_Init(&argc, &argv);                   // start MPI
  MPI_Comm_rank(MPI_COMM_WORLD, &pid);      // get current pid
  MPI_Comm_size(MPI_COMM_WORLD, &numProcs); // get num procs
  MPI_Get_processor_name(host, &hostLen);   // get the symbolic host name
  
  // Stops the program when there's an uneven distribution of cols for the procs
  // OR if there's an even distribution, but each proc has less than 3 cols
  if ((width % numProcs != 0) || (width / numProcs < 3))
  {
    MPI_Finalize();
    return -1;
  }

  // printf("Hello world from process %d of %d (host: %s)\n", pid, numProcs, host);

  // Allocate memory 
  H = malloc(sizeof(double*)*max_time);
  int t,p;
  
  for(t=0;t<max_time;t++){
    H[t] = malloc(sizeof(double)*width); // was sizeof(double*), now fixed
  }

  // Initialize constant left/right boundary temperatures
  for(t=0; t<max_time; t++){
    H[t][0] = L_bound_temp;
    H[t][width-1] = R_bound_temp;
  }

  // Initialize temperatures at time 0
  t = 0;
  for(p=1; p<width-1; p++){
    H[t][p] = initial_temp;
  }

  // Simulate the temperature changes for internal cells

  // optimal way:
  // middle stuff first
  // 0th and nth proc do special side cases
  // evens
  // odds

  int sharedAmount = width / numProcs;  // variable to record shared amount of work

  // assign L and R indices for each shared work
  int startIndex = pid * sharedAmount;
  int endIndex = startIndex + sharedAmount - 1; // -1 because indices start at 0
  endIndex = endIndex;

  // sanity check
  // printf("pid: %d startIndex: %d endIndex: %d width: %d\n", pid, startIndex, endIndex, sharedAmount);

  if (numProcs == 1)
  {
    for(t=0; t<max_time-1; t++)
    {
      for(p=1; p<width-1; p++)
      {
        double left_diff  = H[t][p] - H[t][p-1];
        double right_diff = H[t][p] - H[t][p+1];
        double delta = -k*( left_diff + right_diff );
        H[t+1][p] = H[t][p] + delta;
      }
    }
  }
  else // given numProcs > 1
  {
    /**

      Initialization of the processor's local memory
    
    */

    // local 2D array to contain heat values
    double **localH;

    // each proc allocates their "rows"
    localH = malloc(sizeof(double*)*max_time);

    // each proc allocates the "columns" based on its share amount
    for (int x = 0; x < max_time; x++)
    {
      localH[x] = malloc(sizeof(double)*sharedAmount);
    }

    // handle the leftmost processor initialization
    if (pid == 0)
    {
      for (t = 0; t < max_time; t++)
        localH[t][0] = L_bound_temp;
      
      for (int x = 1; x < sharedAmount; x++)
      {
        localH[0][x] = initial_temp;
      }
    }
    // handle the rightmost processor initialization
    else if (pid == numProcs - 1)
    {
      for (t = 0; t < max_time; t++)
        localH[t][sharedAmount-1] = R_bound_temp;

      for (int x = 0; x < sharedAmount - 1; x++)
      {
        localH[0][x] = initial_temp;
      }
    }
    // other processors that're in the middle
    else
    {
      for (int x = 0; x < sharedAmount; x++)
      {
        localH[0][x] = initial_temp;
      }
    }

    /**

      Begin heat exchange algorithm
    
    */

    for(t=0; t<max_time-1; t++)
    {
      /**
      
        Process the middle bits of each processor's work first

      */
      for (int middle = 1; middle < sharedAmount - 1; middle++)
      {
        double left_diff = localH[t][middle] - localH[t][middle - 1];
        double right_diff = localH[t][middle] - localH[t][middle + 1];

        double delta = -k*( left_diff + right_diff );

        localH[t + 1][middle] = localH[t][middle] + delta;
      }

      // barrier
      // MPI_Barrier(MPI_COMM_WORLD);

      /**

        exchange values between the first two pairs [A] and [B]:

        - [A]'s right side gets sent and receives [B]'s left side
        - [B]'s left side gets sent and receives [A]'s right side

        - Assumes that numProcs are divisible by 2
    
      */

      // establish variables for MPI_Sendrecv
      double send;
      double recv;
      int partner = (pid % 2 == 1) ? pid - 1 : pid + 1; // determine initial partners based on initial pairs

      if (pid % 2 == 0) // [A]
      {
        send = localH[t][sharedAmount-1];
      }
      else if (pid % 2 == 1)  // [B]
      {
        send = localH[t][0];
      }

      // exchange b/t pairs
      MPI_Sendrecv(&send, 1, MPI_DOUBLE, partner, 1,
                   &recv, 1, MPI_DOUBLE, partner, 1,
                   MPI_COMM_WORLD, MPI_STATUS_IGNORE);

      // now that the values are received, perform calculations
      if (pid % 2 == 0) // [A]
      {
        double left_diff = localH[t][sharedAmount - 1] - localH[t][sharedAmount - 2];
        double right_diff = localH[t][sharedAmount - 1] - recv;

        double delta = -k*( left_diff + right_diff );

        localH[t + 1][sharedAmount-1] = localH[t][sharedAmount - 1] + delta;
      }
      else if (pid % 2 == 1)  // [B]
      {
        double left_diff = localH[t][0] - recv;
        double right_diff = localH[t][0] - localH[t][1];

        double delta = -k*( left_diff + right_diff );

        localH[t + 1][0] = localH[t][0] + delta;
      }

      // now account for swaps that don't involve the 0th and numProc-1'th processor

      int flag = (pid == 0 || pid == numProcs -1) ? 1 : 0;  // flag to make the if() statement look more clean
      
      if (flag == 0)  // essentially if you're the 0th or numProc-1'th proc, you're not gonna touch this
      {
        partner = (pid % 2 == 1) ? pid + 1 : pid - 1; // reverse the logic: instead of 0+1, 2+3, etc., it's 1+2, 3+4, etc.

        // basically the previous part but reversed blocks
        if (pid % 2 == 0)
        {
          send = localH[t][0];
        }
        else if (pid % 2 == 1)
        {
          send = localH[t][sharedAmount-1];
        }
        
        // exchange b/t new pairs
        MPI_Sendrecv(&send, 1, MPI_DOUBLE, partner, 1,
                     &recv, 1, MPI_DOUBLE, partner, 1,
                     MPI_COMM_WORLD, MPI_STATUS_IGNORE);

        // now that the values are received, perform calculations
        // basically the previous part but reversed blocks
        if (pid % 2 == 0)
        {
          double left_diff = localH[t][0] - recv;
          double right_diff = localH[t][0] - localH[t][1];

          double delta = -k*( left_diff + right_diff );

          localH[t + 1][0] = localH[t][0] + delta;
        }
        else if (pid % 2 == 1)
        {
          double left_diff = localH[t][sharedAmount - 1] - localH[t][sharedAmount - 2];
          double right_diff = localH[t][sharedAmount - 1] - recv;

          double delta = -k*( left_diff + right_diff );

          localH[t + 1][sharedAmount-1] = localH[t][sharedAmount - 1] + delta;
        }
      }
      
      // barrier here just in case
      // MPI_Barrier(MPI_COMM_WORLD);

      // then gather all into H
      MPI_Gather(&(localH[t+1][0]), sharedAmount, MPI_DOUBLE, &(H[t+1][0]), sharedAmount, MPI_DOUBLE, 0, MPI_COMM_WORLD);
    }

    // clean up memory in procs
    for (t = 0; t < max_time; t++)
    {
      free(localH[t]);
    }

    free(localH);
  }
  
  if(print && pid == 0){

    // Print results
    // printf("Temperature results for 1D rod\n");
    // printf("Time step increases going down rows\n");
    // printf("Position on rod changes going accross columns\n");

    // Column headers
    printf("%3s| ","");
    for(p=0; p<width; p++){
      printf("%5d ",p);
    }
    printf("\n");
    printf("%3s+-","---");
    for(p=0; p<width; p++){
      printf("------");
    }
    printf("\n");
    // Row headers and data
    for(t=0; t<max_time; t++){
      printf("%3d| ",t);
      for(p=0; p<width; p++){
        printf("%5.1f ",H[t][p]);
      }
      printf("\n");
    }
  }

  for(t=0; t<max_time; t++)
  {
    free(H[t]);
  }

  free(H);

  MPI_Finalize();
  
  return 0;
}
      
/**

    I / We affirm that all parties listed below have contributed to each solution presented in this document. All parties are capable of describing how the solutions were derived, how they apply to the problem, and that they were created in accordance with the course's PRIME DIRECTIVE.

    Signed,

    Jon-Michael Hoang

*/