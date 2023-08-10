#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <math.h>
#include <errno.h>
#include <omp.h>
#include "densemat.h"

int main(int argc, char **argv){
  if(argc < 3){
    printf("usage: %s row_col.txt damping\n  0.0 < damping <= 1.0\n",argv[0]);
    return -1;
  }

  double damping_factor = atof(argv[2]);
  densemat_t *mat = densemat_load(argv[1]);
  
  printf("Loaded %s: %d rows, %d nonzeros\n",argv[1],mat->nrows,mat->nnz);

  // Normalize columns to sum to 1 by computing an array of column
  // sums then dividing each column by the corresponding sum
  double *colsums = malloc(mat->ncols * sizeof(double));
  for(int c=0; c<mat->ncols; c++){
    colsums[c] = 0.0;
  }
  for(int r=0; r<mat->nrows; r++){
    for(int c=0; c<mat->ncols; c++){
      colsums[c] += mat->data[r][c];
    }
  }
  for(int r=0; r<mat->nrows; r++){
    for(int c=0; c<mat->ncols; c++){
      mat->data[r][c] /= colsums[c];
    }
  }
  free(colsums);
  
  // Apply the damping factor
  double zelem = (1.0-damping_factor) / mat->nrows;
  for(int r=0; r<mat->nrows; r++){
    for(int c=0; c<mat->ncols; c++){
      if(mat->data[r][c] != 0.0){ // Scale down nonzero entries
        mat->data[r][c] *= damping_factor;
      }
      mat->data[r][c] += zelem; // Every entry increases a little
    }
  }

  // Allocate space for the page ranks and a second array to track
  // page ranks from the last iterations
  double *cur_ranks = malloc(mat->nrows * sizeof(double));
  double *old_ranks = malloc(mat->nrows * sizeof(double));
  for(int c=0; c<(mat->nrows); c++){
    cur_ranks[c] = 1.0 / mat->nrows;
    old_ranks[c] = cur_ranks[c];
  }

  double TOL = 1e-3;
  int MAX_ITER = 10000;
  int iter;
  double change = TOL*10;
  double cur_norm;

  printf("Beginning Computation\n\n%4s %8s %8s\n","ITER","DIFF","NORM");
  for(iter=1; change > TOL && iter<=MAX_ITER; iter++){
    // old_ranks assigned to cur_ranks
    for(int c=0; c<mat->ncols; c++){
      old_ranks[c] = cur_ranks[c];
    }

    change = 0.0;
    cur_norm = 0.0;

    // Compute matrix-vector product: cur_ranks = Matrix * old_ranks 
    for(int r=0; r<mat->nrows; r++){
      // Dot product of matrix row with old_ranks column
      cur_ranks[r] = 0.0;
      for(int c=0; c<mat->ncols; c++){
        cur_ranks[r] += mat->data[r][c] * old_ranks[c];
      }
      double diff = cur_ranks[r] - old_ranks[r];
      change += diff>0 ? diff : -diff;
      cur_norm += cur_ranks[r]; // Tracked to detect any errors
    }
    printf("%3d: %8.2e %8.2e\n",iter,change,cur_norm);
  }

  if(change < TOL){
    printf("CONVERGED\n");
  }
  else{
    printf("MAX ITERATION REACHED\n");
  }

  printf("\nPAGE RANKS\n");
  for(int r=0; r<mat->nrows; r++){
    printf("%.8f\n",cur_ranks[r]);
  }

  free(cur_ranks);
  free(old_ranks);
  densemat_free(mat);
  return 0;
}
