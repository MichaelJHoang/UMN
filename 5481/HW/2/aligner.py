import argparse
import numpy as np
import Bio as bio
from Bio import SeqIO



def main():
    # set up argument parser
    parser = argparse.ArgumentParser()
    parser.add_argument('-q', '--query', required=True, type=str)
    parser.add_argument('-r', '--reference', required=True, type=str)
    parser.add_argument('-o', '--output', required=True, type=str)
    parser.add_argument('-g', '--gap_penalty', required=True, type=int)
    parser.add_argument('-p', '--mismatch_penalty', required=True, type=int)
    parser.add_argument('-m', '--match_score', required=True, type=int)
    parser.add_argument('--ignore_outer_gaps', action='store_true')
    parser.add_argument('-s', '--affine_gap_penalty', required=False, default=0,
    type=int)
    # parsing arguments to get variables (can rename variables anything you choose,
    # this just illustrates how to get contents of the parser)
    args = parser.parse_args()
    q_file, ref_file, out_file = args.query, args.reference, args.output
    d, mismatch, match = args.gap_penalty, args.mismatch_penalty, args.match_score
    startend_tf = args.ignore_outer_gaps
    a = args.affine_gap_penalty

    # parse the files
    qParse = SeqIO.parse(open(q_file), 'fasta')
    rParse = SeqIO.parse(open(ref_file), 'fasta')

    # and store them in their respective variables for processing
    qID = ''
    rID = ''

    qSeq = ''
    rSeq = ''

    for q in qParse:
        qID = q.id
        qSeq = q.seq

    for r in rParse:
        rID = r.id
        rSeq = r.seq
        
    # initialize the matrix
    # NOTE: reference is on the x-axis
    #       query is on the y-axis
    nw_matrix = np.zeros((len(rSeq) + 1, len(qSeq) + 1), dtype=int)
    dir_matrix = np.zeros(nw_matrix.shape, dtype=int)

    # provided that we aren't accounting for initial and terminal gaps
    # calculate the first row and column's penalties
    # fun TODO: parallelize this
    if startend_tf is not True:
        for x in range(0, len(rSeq) + 1):
            nw_matrix[x][0] =  x * d
        for y in range(0, len(qSeq) + 1):
            nw_matrix[0][y] = y * d

    # begin the Needleman-Wunsch algorithm
    # fun TODO: parallelize this because this is too god-damn slow
    xDim, yDim = nw_matrix.shape

    # NOTE: you iterate through the X first, and then Y as they correspond to horizontal and vertical respectively.
    for y in range(1, yDim):
        for x in range(1, xDim):
            # temp variable to determine whether or not the bases we're comparing are the same or not
            # if so, assign the matchScore the match value
            # if not, assign the mismatch value
            # -1s here because the for loop is based on nw_matrix's range, which has 1 more column and row than the two
            matchScore = match if rSeq[x - 1] == qSeq[y - 1] else mismatch
            
            # then determine the nw_matrix at pos(x, y)'s value based on
            # the maximum value between:
            #   - the diagonal (x-1, y-1) added by the matchScore
            #   - the position to the left (x-1, y) added by the gap penalty
            #   - the position from the top (x, y-1) added by the gap penalty
            nw_matrix[x][y] = max(nw_matrix[x-1][y-1] + matchScore, nw_matrix[x-1][y] + d, nw_matrix[x][y-1] + d)

            # whatever the max value was, get its index and store it in a direction matrix so that when we backtrack, 
            # we can use this as reference
            dir_matrix[x][y] = np.argmax([nw_matrix[x-1][y-1] + matchScore, nw_matrix[x-1][y] + d, nw_matrix[x][y-1] + d])

            # provided that the startend_tf is True,
            # don't incorporate start and end gap penalties
            if startend_tf is True:
                if x == xDim - 1:
                    nw_matrix[x][y] = max(nw_matrix[x-1][y-1] + matchScore, nw_matrix[x-1][y], nw_matrix[x][y-1])
                    # same with the dir matrix
                    dir_matrix[x][y] = np.argmax([nw_matrix[x-1][y-1] + matchScore, nw_matrix[x-1][y], nw_matrix[x][y-1]])
                if y == yDim - 1:
                    nw_matrix[x][y] = max(nw_matrix[x-1][y-1] + matchScore, nw_matrix[x-1][y], nw_matrix[x][y-1])
                    # same with the dir matrix
                    dir_matrix[x][y] = np.argmax([nw_matrix[x-1][y-1] + matchScore, nw_matrix[x-1][y], nw_matrix[x][y-1]])
                
                # not sure why, but running this line instead of having it copied under the two ifs above causes
                # the autograder to time out
                #dir_matrix[x][y] = np.argmax([nw_matrix[x-1][y-1] + matchScore, nw_matrix[x-1][y], nw_matrix[x][y-1]])
                    
    # time to backtrack, which involves:
    #   - calculating the alignment score
    #   - and producing an alignment visualization (3 arrays) between the 2 sequences
    #       - reference on top
    #       - alignment vis in the middle
    #       - query on bottom
    rVis = []
    qVis = []
    vis  = []

    xIndex, yIndex = nw_matrix.shape
    # -1 bc shape is [1, size]
    xIndex -= 1
    yIndex -= 1

    # the total score's the bottom-right-most value in the matrix
    alignmentScore = nw_matrix[xIndex][yIndex]

    # for question 6
    mismatchCount = 0

    # from the bottom-right-most of the matrix,
    # while the index variables are not negative,
    while xIndex > 0 or yIndex > 0:
        # use the direction matrix we built in the forwards step to now backstep
        # if dir_matrix[xIndex][yIndex] is...
        #   - 0: go diagonal
        #   - 1: go left
        #   - 2: go up
        maxIndex = dir_matrix[xIndex][yIndex]
        
        # if we've reached the very left column, we just want to go up
        if xIndex == 0:
            # assign visualization
            rVis.append("_")
            qVis.append(qSeq[yIndex-1])
            vis.append(" ")

            mismatchCount += 1

            # decrement since we're going up
            yIndex -= 1

        # ditto but for the very top, we'd want to go to the left from there instead
        elif yIndex == 0:
            # assign visualization
            rVis.append(rSeq[xIndex-1])
            qVis.append("_")
            vis.append(" ")

            mismatchCount += 1

            # decrement since we're going left
            xIndex -= 1

        # if the maximum's to the top left
        elif maxIndex == 0:
            # assign visualization
            rVis.append(rSeq[xIndex-1])
            qVis.append(qSeq[yIndex-1])

            # determine whether or not the bases are the same and append a
            # " | " if they match or
            # " x " if they don't match
            if rVis[-1] == qVis[-1]:
                vis.append("|")
            else:
                vis.append("x")

                mismatchCount += 1

            # decrement both values since we're going top left
            xIndex -= 1
            yIndex -= 1

        # or to the left
        elif maxIndex == 1:
            # assign visualization
            rVis.append(rSeq[xIndex-1])
            qVis.append("_")
            vis.append(" ")

            mismatchCount += 1

            # decrement since we're going left
            xIndex -= 1

        # or to the top
        elif maxIndex == 2:
            # assign visualization
            rVis.append("_")
            qVis.append(qSeq[yIndex-1])
            vis.append(" ")

            mismatchCount += 1

            # decrement since we're going up
            yIndex -= 1

        # default case -- error handling and checking
        else:
            print("ERROR", xIndex, " ", yIndex)
            break

    print("Mismatches: ", mismatchCount)

    # because we trace backed, we should reverse these lists
    rVis = rVis[::-1]
    qVis = qVis[::-1]
    vis  = vis[::-1]

    # output the results to the text file
    with open(out_file, 'w') as outputFile:
        outputFile.write(str(int(alignmentScore)) + "\n")
        outputFile.write('>' + qID +"\n")
        outputFile.write(''.join(qVis) + "\n")
        outputFile.write(''.join(vis) + "\n")
        outputFile.write(''.join(rVis) + "\n")
        outputFile.write('>' + rID + "\n")
        
main()