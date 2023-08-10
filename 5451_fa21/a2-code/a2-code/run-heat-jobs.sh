#!/bin/bash

. ./mpiopts.sh                  # load MPIOPTS variable

TIME=500                                  # number of time steps
ALLWIDTHS="6400 12800 25600 51200 102400" # various widths for rods
ALLNP="1 2 4 8 10 16 32 64 128"           # number of processors to use via mpirun -np
# ALLNP="1 2 4 8 10 16"                     # number of processors to use via mpirun -np

mkdir -p output
printf "Output is in the output/ directory\n"

HOSTFILE=hostfile-veggie-ip.txt

for WIDTH in $ALLWIDTHS ; do 
    for NP in $ALLNP; do
        outfile="ht.$(printf "%06d" $WIDTH).$(printf "%03d" $NP).out"
        cmd="mpirun $MPIOPTS -hostfile $HOSTFILE -np $NP ./mpi_heat $TIME $WIDTH 0"
        printf '%s > %s\n' "$cmd" "$outfile"
        {
            date
            time -p $cmd
            date
        } &> output/$outfile
    done
done

# After completing all jobs, perform a grep like below to extract the
# runtimes for the jobs based on their output files.
#
# > grep 'real' ht.*.out
# ht.006400.01.out:real 1.47
# ht.006400.02.out:real 1.64
# ht.006400.04.out:real 2.12
# ht.006400.08.out:real 3.59
# ht.006400.10.out:real 3.09
# ht.006400.16.out:real 3.01
# ht.012800.01.out:real 1.66
# ht.012800.02.out:real 1.77
# ht.012800.04.out:real 2.13
# ht.012800.08.out:real 3.69
# ht.012800.10.out:real 3.46
# ht.012800.16.out:real 4.32
# ht.025600.01.out:real 1.95
# ht.025600.02.out:real 1.98
# ht.025600.04.out:real 2.20
# ht.025600.08.out:real 3.93
# ht.025600.10.out:real 4.13
# ht.025600.16.out:real 5.97
# ht.051200.01.out:real 2.62
# ht.051200.02.out:real 2.44
# ht.051200.04.out:real 2.90
# ht.051200.08.out:real 5.19
# ht.051200.10.out:real 5.12
# ht.051200.16.out:real 6.20
# ht.102400.01.out:real 4.11
# ht.102400.02.out:real 3.71
# ht.102400.04.out:real 4.13
# ht.102400.08.out:real 6.92
# ht.102400.10.out:real 6.68
# ht.102400.16.out:real 7.89
