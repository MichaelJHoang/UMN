// sendrecv_ring.c : Demonstrates MPI_Sendrecv() in a (virtual) ring;
// the source/destination for sends is not symmetric instead sending
// to one processor and receiving from another to "shift" data around
// the ring of procs. Also shows MPI_Sendrecv_replace() which allows
// use of a single buffer for both send and receive reducing data
// requirements.

#include <stdio.h>
#include <mpi.h>

#define NAME_LEN 255

int main (int argc, char *argv[]){
  MPI_Init (&argc, &argv);	// starts MPI

  // Basic info
  int npes, myrank, name_len;   
  char processor_name[NAME_LEN];
  MPI_Comm_size(MPI_COMM_WORLD, &npes); 
  MPI_Comm_rank(MPI_COMM_WORLD, &myrank); 
  MPI_Get_processor_name(processor_name, &name_len);

  MPI_Barrier(MPI_COMM_WORLD);
  if(myrank == 0){
    printf("==================================================\n");
    printf("Sending / Receiving in a ring\n");
  }
  MPI_Barrier(MPI_COMM_WORLD);

  // Fill a with powers of proc rank
  int mine = 10*(myrank);
  int yours = -1;

  // establish send/receive partners in a ring
  int left_part  = (myrank - 1 + npes) % npes;
  int right_part = (myrank + 1 + npes) % npes;

  // Send/Receive partners are NOT the same but MPI_Sendrecv() takes
  // does not mind. Will send 'mine' to right partner and receive
  // 'yours' from left partner.
  MPI_Sendrecv(&mine,  1, MPI_INT, right_part, 1,
               &yours, 1, MPI_INT, left_part,  1,
               MPI_COMM_WORLD, MPI_STATUS_IGNORE);

  printf("Proc %2d (%s): mine %3d yours %3d\n",
         myrank,processor_name,mine,yours);

  MPI_Barrier(MPI_COMM_WORLD);
  if(myrank == 0){
    printf("==================================================\n");
    printf("Sending / Receiving in same buffer\n");
  }
  MPI_Barrier(MPI_COMM_WORLD);

  // establish send/receive partners in a ring
  left_part  = (myrank - 3 + npes) % npes;
  right_part = (myrank + 3 + npes) % npes;

  int mydata = 10*myrank;

  // MPI_Sendrecv_replace() allows a single buffer to be used for
  // send/recieve: old contents are overwritten with what is received
  // but this does not interfere with the original data being
  // correctly sent to the destination
  MPI_Sendrecv_replace(&mydata, 1, MPI_INT,
                       right_part, 1, left_part,  1,
                       MPI_COMM_WORLD, MPI_STATUS_IGNORE);

  printf("Proc %2d (%s): mydata %3d\n",
         myrank,processor_name,mydata);

  MPI_Finalize();
  return 0;
}
