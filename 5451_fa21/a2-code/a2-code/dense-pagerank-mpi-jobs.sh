#!/bin/bash
# 
# Runs mpi pageranks with various numbers of processors and graphs to
# generate timings.
# 
# After completing all jobs, perform a grep like below to extract the
# runtimes for the jobs based on their output files.
#
# >> ./dense-pagerank-mpi-jobs.sh                                                                                                                                
# Output stored in the file 'dense-pagerank-mpi-timings.Fri_19_Nov_2021_04:16:25_PM_CST.log'                                                                                                    
# mpirun  --mca opal_warn_on_missing_libcuda 0 -hostfile hostfile-veggie-ip.txt -np 1 ./dense_pagerank_mpi graphs/notredame-501.txt 0.85                                                        
# mpirun  --mca opal_warn_on_missing_libcuda 0 -hostfile hostfile-veggie-ip.txt -np 4 ./dense_pagerank_mpi graphs/notredame-501.txt 0.85                                                        
# mpirun  --mca opal_warn_on_missing_libcuda 0 -hostfile hostfile-veggie-ip.txt -np 10 ./dense_pagerank_mpi graphs/notredame-501.txt 0.85
# ...
# Output stored in the file 'dense-pagerank-mpi-timings.Fri_19_Nov_2021_04:16:25_PM_CST.log'

# >> grep runtime dense-pagerank-mpi-timings.Fri_19_Nov_2021_04:16:25_PM_CST.log
# runtime: procs 1 graph notredame-501.txt realtime 1.234
# runtime: procs 4 graph notredame-501.txt realtime 1.234
# runtime: procs 10 graph notredame-501.txt realtime 1.234
# ...

source ./mpiopts.sh                       # load MPIOPTS variable

# # small tests for laptops
# MPIOPTS+=" --oversubscribe"
# ALLGRAPHS="notredame-100.txt notredame-501.txt"
# ALLNP="1 2 4 8"                   # number of processors to use via mpirun -np

# full tests for veggie cluster
HOSTFILE="-hostfile hostfile-veggie-ip.txt"
ALLGRAPHS="notredame-501.txt notredame-8000.txt notredame-16000.txt" 
ALLNP="1 2 4 8 10 16 32 64 128"          # number of processors to use via mpirun -np

thedate="$(date)"
outfile=$(printf 'dense-pagerank-mpi-timings.%s.log' "${thedate// /_}")

msg=$(printf "Output stored in the file '%s'" "$outfile")

printf "%s\n" "$msg"

printf 'Dense Pagerank MPI Timings on host %s for %s\n' "$HOSTNAME" "$thedate" > $outfile

for GRAPH in $ALLGRAPHS ; do 
    for NP in $ALLNP; do
        cmd="mpirun $MPIOPTS $HOSTFILE -np $NP ./dense_pagerank_mpi graphs/$GRAPH 0.85" 
        printf '%s\n' "$cmd"
        {
            echo $cmd
            /usr/bin/time -f "runtime: procs $NP graph $GRAPH realtime %e" $cmd # > /dev/null
            echo
        } &>> $outfile
    done
done

printf "%s\n" "$msg"
