#pragma once

// Simple wrapper around a dense matrix. Can access elements either
// linearly through mat->all[i] or via 2d index through
// mat->data[i][j]
typedef struct {
  double *all;                  // Array of all elements, allows for easy sharing of entire matrix
  double **data;                // Pointers to individual rows to allow mat->data[r][c] access
  int nrows, ncols;             // Number of rows and columns
  int nnz;                      // Number of nonzeros in the matrix initially 
} densemat_t;

void densemat_free(densemat_t *mat);
densemat_t *densemat_new(int nrows, int ncols);
densemat_t *densemat_load(char *fname);
