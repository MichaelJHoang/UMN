#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>

typedef struct KMData   // Data set to be clustered
{
    int ndata;          // count of data
    int dim;            // dimension of features for data
    float** features;   // pointers to individual features  
    int* assigns;       // cluster to which data is assigned
    int* labels;        // label for data if available
    int nlabels;        // max value of labels +1, number 0,1,...,nlabel0
} KMData;


typedef struct KMClust  // Cluster information
{
    int nclust;         // number of clusters, the "k" in kmeans
    int dim;            // dimension of features for data
    float** features;   // 2D indexing for individual cluster center features
    int* counts;        // number of data in each cluster
} KMClust;


/*

    # Load a data set from the named file. Data should be formatted as a
    # text file as:
    # 
    # 7 :  84 185 159 151  60  36   0   0   0   0   0   0
    # 2 :   0  77 251 210  25   0   0   0 122 248 253  65
    # 1 :   0   0   0   0   0   0   0   0   0  45 244 150
    # 0 :   0   0   0   0 110 190 251 251 251 253 169 109
    # 4 :   0   0   0   4 195 231   0   0   0   0   0   0
    # 1 :   0   0   0   0   0   0   0   0   0  81 254 254
    # 4 :   0  20 189 253 147   0   0   0   0   0   0   0
    # 9 :   0   0   0   0  91 224 253 253  19   0   0   0
    # 5 :   0   0   0   0   0   0  63 253 253 253 253 253
    # 9 :   0   0   0   0   0   0   0  36  56 137 201 199
    #
    # with the lead number being an optional correct label for the data
    # and remaining numbers being floating point values that are space
    # separated which are the feature vector for each data. The abve
    # example does not have any fractional values for features but it
    # could.

*/
KMData* kmdata_load(char* filename)
{
    // allocate and initialize the values within the struct
    KMData* data = calloc(1, sizeof(KMData));

    int maxLabel = -99999;                                    // keep track of the max value of a label

    // open the file to prepare to read in the inputs
    FILE* file = fopen(filename,"r");

    if (file == NULL)
    {
        printf("ERROR: Can't open file: [%c]\n", *filename);
        exit(-1);
    }
    else
    {
        char* buffer = NULL;
        size_t bufferSize = 0;

        // while loop to count num rows (ndata)
        while (getline(&buffer, &bufferSize, file) >= 0)
        {
            data->ndata += 1;
        }

        // read in a single row
        char* temp = strtok(buffer, ": ");  // temp array to store filtered buffer
                                            // the line initially points to the label, so...

        temp = strtok(NULL, ": ");          // move the strtok pointer to the nonlabel part

        // while loop to count num cols (dim)
        while (temp != NULL)
        {
            temp = strtok(NULL, ": ");
            data->dim++;
        }

        // allocate memory for the labels array based on ndata/line numbers
        data->labels = malloc(sizeof(int) * data->ndata);
        // and the assigns
        data->assigns = malloc(sizeof(int) * data->ndata);
        // now allocate memory for the 2d features array
        data->features = malloc(sizeof(float*) * data->ndata);      // rows

        for (int x = 0; x < data->ndata; x++)                       // cols
        {
            data->features[x] = malloc(sizeof(float) * data->dim);
        }

        /**
        
            Hey there Professor,

                I'm assuming you're wondering why in the world I had to scan through the file twice...
                and that's because my earlier attempts at converting this used dynamically allocated arrays
                with the realloc() function, which:

                    1) Caused segfaults that I didn't have time to investigate thoroughly
                    2) Did not return the correct size when I used [sizeof(arr) / sizeof(arr[0])]
                       since it was dynamically allocated

                because of that, I had to read the entire file once to get its dimensions and with that
                information, I then used malloc() with the known dimensions rather than realloc().

                Yeah, I know that's not what the Python code does, but I'm not gonna spend time trying 
                to fix this single issue and instead went about with an "underhanded" technique.
        
        */
        // reset file ptrs back to beginning
        free(buffer);
        buffer = NULL;
        bufferSize = 0;
        rewind(file);

        // small book-keeping vars
        int rowIndex = 0, colIndex = 0;

        // and store the values that're read in
        while (getline(&buffer, &bufferSize, file) >= 0)
        {
            // store the row/buffer in a temp string
            char* temp = strtok(buffer, ": ");

            // store the first number (the label) of the row as an int into the
            // labels array
            data->labels[rowIndex] = atoi(temp);
            maxLabel = data->labels[rowIndex] > maxLabel ? data->labels[rowIndex] : maxLabel;

            colIndex = 0;

            // read in and store the rows
            while ((temp = strtok(NULL, ": ")) != NULL)
            {
                data->features[rowIndex][colIndex] = atof(temp);
                colIndex++;
            }

            rowIndex++;
        }

        free(buffer);
    }

    // sanity checker
    /*
    for (int x = 0; x < data->dim; x++)
    {
        printf("%f\n", data->features[1974][x]);
    }
    */

    // malloc an array based on max label value + 1
    data->nlabels = maxLabel + 1; //malloc(sizeof(int) * (maxLabel + 1));

    fclose(file);

    return data;
}


// Allocate space for clusters in an object
KMClust* kmclust_new(int nclust, int dim)
{
    KMClust* clust = calloc(1, sizeof(KMClust));
    clust->nclust= nclust;
    clust->dim = dim;

    // malloc centers based on nclust (rows) x dim
    clust->features = malloc(sizeof(float*) * nclust);
    // calloc the counts since we want them to all start at 0
    clust->counts = calloc(nclust, sizeof(int));

    for (int c = 0; c < nclust; c++)
    {
        // make memory for and initialize to 0
        clust->features[c] = calloc(dim, sizeof(float));
    }

    return clust;
}


/*

    # Save clust centers in the PGM (portable gray map) image format;
    # plain text and can be displayed in many image viewers. File names re
    # cent_0000.pgm and so on.

*/
void save_pgm_files(KMClust* clust, char* savedir)
{
    int dim_root = (int)sqrt(clust->dim);

    if (clust->dim % dim_root == 0)         // check if this looks like a square image
    {
        printf("Saving cluster centers to %s/cent_0000.pgm ...\n", savedir);
        
        float maxfeat = 0.0f;

        for (int c = 0; c < clust->nclust; c++)
        {
            for (int d = 0; d < clust->dim; d++)
            {
                maxfeat = clust->features[c][d] > maxfeat ? clust->features[c][d] : maxfeat;
            }
        }

        for (int c = 0; c < clust->nclust; c++)
        {
            char outfile[100];

            sprintf(outfile, "%s/cent_%04d.pgm", savedir, c);

            FILE* pgm = fopen(outfile, "w");                    // output the cluster centers as
                                                                // pgm files, a simple image format which
            if (pgm != NULL)                                    // can be viewed in most image
            {                                                   // viewers to show how the cluster center
                fprintf(pgm, "P2\n");                           // actually appears; nomacs is a good viwer
                fprintf(pgm, "%d %d\n", dim_root, dim_root);
                fprintf(pgm, "%.0f\n", maxfeat);
            }

            for (int d = 0; d < clust->dim; d++)
            {
                if (d > 0 && (d % dim_root == 0))
                {
                    fprintf(pgm, "\n");
                }

                fprintf(pgm, "%3.0f ", clust->features[c][d]);
            }

            fprintf(pgm, "\n");

            fclose(pgm);
        }
    }
}


// #### MAIN FUNCTION ####
int main(int argc, char **argv)
{
    if (argc < 3)
    {
        printf("usage: kmeans.py <datafile> <nclust> [savedir] [maxiter]");
        exit (-1);
    }

    char* datafile = argv[1];
    int nclust = atoi(argv[2]);
    char* savedir = ".";
    int MAXITER = 100;          // bounds the iterations

    if (argc > 3)               // create save directory if specified
    {
        savedir = argv[3];
        char cmdlineBuffer[100];
        sprintf(cmdlineBuffer, "mkdir -p %s", savedir);
        system(cmdlineBuffer);
    }

    if (argc > 4)
    {
        MAXITER = atoi(argv[4]);
    }

    printf("datafile: %s\n", datafile);
    printf("nclust: %d\n", nclust);
    printf("savedir: %s\n", savedir);

    KMData* data = kmdata_load(datafile);
    KMClust* clust = kmclust_new(nclust, data->dim);

    printf("ndata: %d\n", data->ndata);
    printf("dim: %d\n\n", data->dim);

    // NEED TO PARALLELIZE THIS
    for (int i = 0; i < data->ndata; i++)                       // random, regular initial cluster assignment
    {
        data->assigns[i] = i % clust->nclust;
    }

    // NEED TO PARALLELIZE THIS?
    for (int c = 0; c < clust->nclust; c++)
    {
        int icount = floor(data->ndata / clust->nclust);        // integer division
        int extra = 0;

        if (c < (data->ndata % clust->nclust))
        {
            extra = 1;                                          // extras in earlier clusters
        }

        clust->counts[c] = icount + extra;
    }

    /*
    
        MAIN ALGORITHM
    
    */
    int curiter = 1;                // current iteration
    int nchanges = data->ndata;     // check for changes in cluster assignment; 0 is converged

    printf("==CLUSTERING: MAXITER %d==\n", MAXITER);
    printf("ITER NCHANGE CLUST_COUNTS\n");

    int c = 0, i = 0, d = 0;                        // loop iterators

    // CANNOT PARALLELIZE THIS OUTERLOOP
    // due to its strict dependence on previous iterations
    while (nchanges > 0 && curiter <= MAXITER)      // loop until convergence
    {
        // DETERMINE NEW CLUSTER CENTERS
        for (c = 0; c < clust->nclust; c++)         // reset cluster centers to 0.0
        {
            for (d = 0; d < clust->dim; d++)
            {
                clust->features[c][d] = 0.0;
            }
        }

        for (i = 0; i < data->ndata; i++)           // sum up data in each cluster
        {
            c = data->assigns[i];

            for (d = 0; d < clust->dim; d++)
            {
                clust->features[c][d] += data->features[i][d];
            }
        }

        // ALL_REDUCE HERE
        for (c = 0; c < clust->nclust; c++)       // divide by ndatas of data to get mean of cluster center
        {
            if(clust->counts[c] > 0)
            {
                for (d = 0; d < clust->dim; d++)
                {
                    clust->features[c][d] = clust->features[c][d] / clust->counts[c];
                }
            }
        }

        // DETERMINE NEW CLUSTER ASSIGNMENTS FOR EACH DATA
        for (c = 0; c < clust->nclust; c++)     // reset cluster counts to 0
        {
            clust->counts[c] = 0;               // re-init here to support first iteration
        }

        nchanges = 0;

        for (i = 0; i < data->ndata; i++)           // iterate over all data
        {
            int best_clust = 0;
            float best_distsq = (float)INFINITY;

            for (c = 0; c < clust->nclust; c++)     // compare data to each cluster and assign to closest
            {
                float distsq = 0.0f;

                for (d = 0; d < clust->dim; d++)    // calculate squared distance to each data dim
                {
                    float diff = data->features[i][d] - clust->features[c][d];
                    distsq += diff * diff;
                }

                if (distsq < best_distsq)           // if closer to this cluster than current best
                {
                    best_clust = c;
                    best_distsq = distsq;
                }
            }

            clust->counts[best_clust] += 1;

            if (best_clust != data->assigns[i])     // assigning data to a different cluster?
            {
                nchanges += 1;                      // indicate cluster assignment has changed
                data->assigns[i] = best_clust;      // assign to new cluster
            }
        }


        // print iteration information at the end of the iter
        printf("%3d: %5d |", curiter, nchanges);

        for (c = 0; c < clust->nclust; c++)
        {
            printf(" %4d", clust->counts[c]);
        }
        printf("\n");

        curiter += 1;
    }

    // Loop has converged
    if (curiter > MAXITER)
    {
        printf("WARNING: maximum iteration %d exceeded, may not have converged\n", MAXITER);
    }
    else
    {
        printf("CONVERGED: after %d iterations\n\n", curiter);
    }

    /**
    
        CLEANUP + OUTPUT
    
    */

    // CONFUSION MATRIX
    int** confusion;
    confusion = calloc(data->nlabels, sizeof(int*));    // confusion matrix: labels * clusters big

    for (i = 0; i < data->nlabels; i++)
    {
        confusion[i] = calloc(nclust, sizeof(int));
    }

    for (i = 0;  i < data->ndata; i++)                  // count which labels in which clusters
    {
        confusion[data->labels[i]][data->assigns[i]] += 1;
    }

    printf("==CONFUSION MATRIX + COUNTS==\n");
    printf("LABEL \\ CLUST\n");

    printf("%2s ", "");                                 // confusion matrix header
    
    for (int j = 0; j < clust->nclust; j++)
    {
        printf(" %4d", j);
    }
    printf(" %4s\n", "TOT");

    for (i = 0; i < data->nlabels; i++)                 // each row of confusion matrix
    {
        printf("%2d:", i);

        int tot = 0;

        for (int j = 0; j < clust->nclust; j++)
        {
            printf(" %4d", confusion[i][j]);
            tot += confusion[i][j];
        }

        printf(" %4d\n", tot);
    }

    printf("TOT");                                      // final total row of confusion matrix
    
    int tot = 0;

    for (c = 0; c < clust->nclust; c++)
    {
        printf(" %4d", clust->counts[c]);
        tot += clust->counts[c];
    }
    printf(" %4d\n\n", tot);

    // LABEL FILE OUTPUT
    char outfile[100];
    sprintf(outfile, "%s/labels.txt", savedir);

    printf("Saving cluster labels to file %s\n", outfile);

    FILE* fout = fopen(outfile, "w");

    if (fout != NULL)
    {
        for (int i = 0; i < data->ndata; i++)
        {
            fprintf(fout, "%2d %2d\n", data->labels[i], data->assigns[i]);
        }
    }

    // SAVE PGM FILES CONDITIONALLY
    save_pgm_files(clust, savedir);

    // close the file
    fclose(fout);

    // CLEAN UP DATA
    for (i = 0; i < data->nlabels; i++)
    {
        free(confusion[i]);
    }
    free(confusion);


    // KMData cleanup
    for (int x = 0; x < data->ndata; x++)
    {
        free(data->features[x]);
    }

    free(data->features);
    free(data->assigns);
    free(data->labels);
    //free(data->nlabels);
    free(data);

    // KMClust cleanup
    for (int c = 0; c < nclust; c++)
    {
        free(clust->features[c]);
    }
    free(clust->features);
    free(clust->counts);
    free(clust);

    return 0;
}

/**

    I / We affirm that all parties listed below have contributed to each solution presented in this document. All parties are capable of describing how the solutions were derived, how they apply to the problem, and that they were created in accordance with the course's PRIME DIRECTIVE.

    Signed,

    Jon-Michael Hoang

*/