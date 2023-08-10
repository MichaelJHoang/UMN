// cartesian_comm.c: gives a basic demo of how Cartesian coordinate
// system for processors can be established using the
// MPI_Cart_create() function and how 2D coordinates for procs can be
// obtained via MPI_Cart_coords(). Also demonstrates use of the
// MPI_Cart_shift() function to calculate the linear rank of a
// processor for a 2D grid shift along rows or columns. This can be
// adjusted on the command line via
//
// mpirun -np 16 ./a.out 2 1   # shift rows down 2, cols right by 1
// mpirun -np 16 ./a.out -3 3  # shift rows up by 3, cols right by 3

#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>
#include <math.h>

#define NAME_LEN 255

int main (int argc, char *argv[]){
  MPI_Init (&argc, &argv);	// starts MPI

  // Basic info
  int npes, myrank, name_len;   
  char processor_name[NAME_LEN];
  MPI_Comm_size(MPI_COMM_WORLD, &npes); 
  MPI_Comm_rank(MPI_COMM_WORLD, &myrank); 
  MPI_Get_processor_name(processor_name, &name_len);

  // MPI_Barrier(MPI_COMM_WORLD);
  // if(myrank == 0){
  //   printf("==================================================\n");
  //   printf("Sending / Receiving in a ring\n");
  // }
  // MPI_Barrier(MPI_COMM_WORLD);

  // Set up the Cartesian topology 
  int dim_len = 2;
  int dims[2] = {sqrt(npes), sqrt(npes)}; // # rows/cols
  int periods[2] = {1, 1};      // wrap-around rows/cols

  // Create the Cartesian topology, with rank reordering 
  MPI_Comm comm_2d;
  MPI_Cart_create(MPI_COMM_WORLD,         // original comm
                  dim_len, dims, periods, // cartesian comm props
                  1,           // re-order linear rank if beneficial
                  &comm_2d);   // new communicator with 2D coords

  // Get the rank and coordinates with respect to the new topology 
  int my2drank = -1;            // may be differ from world rank
  MPI_Comm_rank(comm_2d, &my2drank);

  int mycoords[2] = {-1, -1};   // (i,j) coords
  MPI_Cart_coords(comm_2d, my2drank, 2, mycoords);

  printf("Proc %2d (%s): my2drank %3d mycoords (%3d, %3d)\n",
         myrank,processor_name,
         my2drank,mycoords[0],mycoords[1]);

  int rowshift = 1;
  int colshift = 0;
  if(argc >= 2){
    rowshift = atoi(argv[1]);
  }
  if(argc >= 3){
    colshift = atoi(argv[2]);
  }

  MPI_Barrier(MPI_COMM_WORLD);
  if(myrank == 0){
    printf("\n");
    printf("==================================================\n");
    printf("Performing row shift by %d and col shift by %d\n",
           rowshift, colshift);
  }
  MPI_Barrier(MPI_COMM_WORLD);

  int mydata = (100*mycoords[0])+mycoords[1];
  int rowsend=-1, rowrecv=-1;                               // calc/perform row shift
  MPI_Cart_shift(comm_2d, 0, rowshift, &rowrecv, &rowsend); // 0 is the row dimension
  MPI_Sendrecv_replace(&mydata, 1, MPI_INT,
                       rowsend, 1, rowrecv,  1,
                       MPI_COMM_WORLD, MPI_STATUS_IGNORE);

  int colsend=-1, colrecv=-1;                               // calc/perform col shift
  MPI_Cart_shift(comm_2d, 1, colshift, &colrecv, &colsend); // 1 is the column dimension
  MPI_Sendrecv_replace(&mydata, 1, MPI_INT,
                       colsend, 1, colrecv,  1,
                       MPI_COMM_WORLD, MPI_STATUS_IGNORE);

  printf("Proc %2d (%s): (%3d, %3d) mydata %03d\n",
         myrank,processor_name,
         mycoords[0], mycoords[1], mydata);

  MPI_Comm_free(&comm_2d);
  MPI_Finalize();
  return 0;
}