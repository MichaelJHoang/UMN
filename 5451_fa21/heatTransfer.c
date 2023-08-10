#include <stdio.h>
#include <stdlib.h>
//  HEAT TRANSFER SIMULATION
//  
//  Simple physical simulation of a rod connected at the left and right
//  ends to constant temperature heat/cold sources.  All positions on
//  the rod are set to an initial temperature.  Each time step, that
//  temperature is altered by computing the difference between a cells
//  temperature and its left and right neighbors.  A constant k
//  (thermal conductivity) adjusts these differences before altering
//  the heat at a cell.  Use the following model to compute the heat
//  for a position on the rod according to the finite difference
//  method.
// 
//     left_diff  = H[t][p] - H[t][p-1];
//     right_diff = H[t][p] - H[t][p+1];
//     delta = -k*( left_diff + right_diff )
//     H[t+1][p] = H[t][p] + delta
//  
//  Substituting the above, one can get the following
// 
//    H[t+1][p] = H[t][p] + k*H[t][p-1] - 2*k*H[t][p] + k*H[t][p+1]
// 
//  The matrix H is computed for all time steps and all positions on
//  the rod and displayed after running the simulation.  The simulation
//  is run for a fixed number of time steps rather than until
//  temperatures reach steady state.

int main(int argc, char **argv){
  int max_time = 50;            // Number of time steps to simulate 
  int width = 20;               // Number of cells in the rod 
  double initial_temp = 50.0;   // Initial temp of internal cells  
  double L_bound_temp = 20.0;   // Constant temp at Left end of rod 
  double R_bound_temp = 10.0;   // Constant temp at Right end of rod 
  double k = 0.5;               // thermal conductivity constant 
  double **H;                   // 2D array of temps at times/locations  

  // Allocate memory  
  H = malloc(sizeof(double*)*max_time); 
  int t,p;
  for(t=0;t<max_time;t++){
    H[t] = malloc(sizeof(double*)*width);
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
  for(t=0; t<max_time-1; t++){
    for(p=1; p<width-1; p++){
      double left_diff  = H[t][p] - H[t][p-1];
      double right_diff = H[t][p] - H[t][p+1];
      double delta = -k*( left_diff + right_diff );
      H[t+1][p] = H[t][p] + delta;
    }
  }

  // Print results 
  printf("Temperature results for 1D rod\n");
  printf("Time step increases going down rows\n");
  printf("Position on rod changes going accross columns\n");

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

  return 0;
}