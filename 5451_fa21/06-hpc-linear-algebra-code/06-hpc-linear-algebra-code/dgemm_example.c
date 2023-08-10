// dgemm_example.c : shows example of BLAS dgemm() call to multiply
// two matrices. Uses the CBLAS interface which is more ergonomic than
// regular BLAS.
//
// Compile: gcc dgemm_example.c -lcblas
//                              ^^^^^^^ links cblas library
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <cblas.h>              // for cblas_dgemm() - general matrix multiply

int main(int argc, char *argv[]) {
  // A : arows * midim matrix
  // B : midim * bcols matrix
  // B : arows * bcols matrix
  int arows = 50;
  int bcols = 100;
  int midim = 75;
  double *A = malloc(arows*midim*sizeof(double));
  double *B = malloc(midim*bcols*sizeof(double));
  double *C = malloc(arows*bcols*sizeof(double));

  // Following code will set up multi-dimensional access for the
  // matrix A if this is needed; creates an array of row pointers into
  // the 1D block A; Arows can then use 2D indexing
  // 
  // double **Arows = malloc(arows*sizoef(double*));
  // for(int i=0; i<arows; i++){
  //   Arows[i] = A + i*midim;   // via pointer arithmetic
  //   Arows[i] = &A[i*midim];   // via addressing
  // }
  // Arows[i][j] = 1.0;

  printf("Intializing matrix data\n\n");
  for(int i=0; i < arows*midim; i++) {
    A[i] = i+1.0;
  }
  for(int i=0; i < midim*bcols; i++) {
    B[i] = i-1.0;
  }
  for(int i=0; i < arows*bcols; i++) {
    C[i] = 0.0;
  }

  double alpha=1.0, beta=1.0;

  printf("Multiplying Matrices\n");

  cblas_dgemm(CblasRowMajor, CblasNoTrans, CblasNoTrans,
              arows, bcols, midim,
              alpha,
              A, midim, B, bcols,
              beta, C, bcols);

  printf("Dellocating memory\n");
  free(A);
  free(B);
  free(C);

  return 0;
}
