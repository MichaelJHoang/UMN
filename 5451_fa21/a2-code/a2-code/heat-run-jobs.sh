#!/bin/bash
# 
# Runs MPI heat with various numbers of processors and widths to
# generate timings.
# 
# After completing all jobs, perform a grep like below to extract the
# runtimes for the jobs based on their output files.

source ./mpiopts.sh                       # load MPIOPTS variable

TIME=500                                  # number of time steps
ALLWIDTHS="6400 25600 102400"             # various widths for rods
ALLNP="1 2 4 8 10 16 32 64 128"           # number of processors to use via mpirun -np
# ALLNP="1 2 4 8 10 16"                   # number of processors to use via mpirun -np
ALLWIDTHS="6400 12800 25600 51200 102400" # various widths for rods

thedate="$(date)"
outfile=$(printf 'heat-timings.%s.log' "${thedate// /_}")

msg=$(printf "Output stored in the file '%s'" "$outfile")

printf "%s\n" "$msg"

printf 'Heat Timings on host %s for %s\n' "$HOSTNAME" "$thedate" > $outfile

HOSTFILE="-hostfile hostfile-veggie-ip.txt"

for WIDTH in $ALLWIDTHS ; do 
    for NP in $ALLNP; do
        # outfile=$(printf "ht.%06d.%03d.out" $WIDTH $NP)
        cmd="mpirun $MPIOPTS $HOSTFILE -np $NP ./heat_mpi $TIME $WIDTH 0"
        printf '%s\n' "$cmd"
        {
            echo $cmd
            /usr/bin/time -f "runtime: procs $NP width $WIDTH realtime %e" $cmd
            echo
        } &>> $outfile
    done
done

printf "%s\n" "$msg"
