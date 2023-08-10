#include <stdlib.h>
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <math.h>
#include <errno.h>
#include "densemat.h"

// Free a densemat
void densemat_free(densemat_t *mat){
  free(mat->all);
  free(mat->data);
  free(mat);
}

// Allocate a dense matrix of the given size. Initialize its elements
// to 0.0
densemat_t *densemat_new(int nrows, int ncols){
  densemat_t * mat = malloc(sizeof(densemat_t));
  mat->nrows = nrows;
  mat->ncols = ncols;
  mat->nnz = 0;
  mat->all = malloc(nrows*ncols * sizeof(double *));
  for(int i=0; i<nrows*ncols; i++){
    mat->all[i] = 0.0;
  }

  mat->data = malloc(nrows * sizeof(double**));
  for(int i=0; i<nrows; i++){
    mat->data[i] = &mat->all[i*ncols];
  }
  return mat;
}    

// Read a dense matrix from the given file. Assumes a square matrix in
// the file.  Contents of the file are only row and column pairs whose
// value is assumed 1.0.  The format of the file starts with the
// number of rows and nonzeros (number of lines in file) on the first
// line. Each subsequent line has a row/col entry whose value is
// assumed to be 1.0
densemat_t *densemat_load(char *fname){
  FILE *f = fopen(fname,"r");
  if(f==NULL){
    perror(fname);
    exit(1);
  }

  int nrows, nnz;

  fscanf(f, "%d %d", &nrows, &nnz);
  densemat_t *mat = densemat_new(nrows,nrows);

  int i;
  for(i=0; i<nnz; i++){
    int row,col;
    fscanf(f,"%d %d",&row,&col);
    if(row >= nrows || col >= nrows){
      fprintf(stderr,"ERROR: line %d has row/col %d %d for matrix with rows/cols %d %d\n",
              i+1,row,col,nrows,nrows);
      exit(1);
    }
    mat->data[row][col] = 1.0;
    mat->nnz++;
  }
  assert(i==nnz);
  fclose(f);
  return mat;
}

