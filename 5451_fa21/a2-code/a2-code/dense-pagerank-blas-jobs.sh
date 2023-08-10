#!/bin/bash
# 
# Runs serial and blas pageranks on graphs to generate timings.
# 

# After completing all jobs, perform a grep like below to extract the
# runtimes for the jobs based on their output files.
# >> ./dense-pagerank-omp-jobs.sh
# Output stored in the file 'dense-pagerank-omp-timings.Fri_19_Nov_2021_08:44:39_PM_CST.log'
# OMP_NUM_THREADS=1 ./dense_pagerank_omp graphs/notredame-501.txt 0.85
# OMP_NUM_THREADS=4 ./dense_pagerank_omp graphs/notredame-501.txt 0.85
# OMP_NUM_THREADS=10 ./dense_pagerank_omp graphs/notredame-501.txt 0.85
# ...
# Output stored in the file 'dense-pagerank-omp-timings.Fri_19_Nov_2021_08:44:39_PM_CST.log'
# 
# >> grep runtime dense-pagerank-omp-timings.Fri_19_Nov_2021_08:44:39_PM_CST.log
# runtime: procs 1 graph notredame-501.txt realtime 1.23
# runtime: procs 4 graph notredame-501.txt realtime 1.23
# runtime: procs 10 graph notredame-501.txt realtime 1.23
# ...

# # small tests for laptops
# ALLGRAPHS="notredame-100.txt notredame-501.txt"
# ALLNP="1 2 4 8"                 # number of threads to use via OMP_NUM_THREADS

# full tests for veggie cluster
ALLGRAPHS="notredame-501.txt notredame-8000.txt notredame-16000.txt" 

thedate="$(date)"
outfile=$(printf 'dense-pagerank-blas-timings.%s.log' "${thedate// /_}")

msg=$(printf "Output stored in the file '%s'" "$outfile")

printf "%s\n" "$msg"
printf 'Dense Pagerank Serial/BLAS Timings on host %s for %s\n' "$HOSTNAME" "$thedate" > $outfile

for GRAPH in $ALLGRAPHS ; do 
    for prog in ./dense_pagerank_serial ./dense_pagerank_blas; do
        cmd="$prog graphs/$GRAPH 0.85" 
        printf '%s\n' "$cmd"
        {
            printf '%s\n' "$cmd"
            OMP_NUM_THREADS=$NP /usr/bin/time -f "runtime: $prog graph $GRAPH realtime %e" $cmd # > /dev/null
            echo
        } &>> $outfile
    done
done


printf "%s\n" "$msg"
