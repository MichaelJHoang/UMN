import csv
import sys
import numpy as np
import Bio as bio
from Bio import SeqIO
from Bio.Seq import Seq
import matplotlib.pyplot as plt

# py file to organize data for hw 1
def main():
    # read the output files from count_codons.py
    file = open("./output/SARS-CoV-2_separate_genome_output.csv", "r")
    sepList = list(csv.reader(file, delimiter=","))
    file.close()

    file = open ("./output/SARS-CoV-2_whole_genome_output.csv", "r")
    wholeList = list(csv.reader(file, delimiter=","))
    file.close()

    ########## For problem 5 ##########

    # Create a dictionary to map the first elements of sepList to their original positions
    sepList_mapping = {item[0]: x for x, item in enumerate(sepList)}

    # Sort wholeList based on the order of sepList
    wholeList_sorted = sorted(wholeList, key=lambda x: sepList_mapping[x[0]])

    # output it (the whole list) to a file to be used in excel (for problem 5)
    with open("./output/wholeList_sorted_based_on_sepList.csv", "w") as output:
        writer = csv.writer(output)
        for row in wholeList_sorted:
            writer.writerow(row)

    ########## END problem 5 ##########



    ########## For problem 6 ##########

    aminoAcids_sep = {}
    aminoAcids_whole = {}

    # convert codons to amino acids and build a hasmap with it
    for x in range(len(sepList)):       # sep
        AA = Seq(sepList[x][0]).translate()
        if (AA in aminoAcids_sep):
            aminoAcids_sep[AA] += int(sepList[x][1])    # cast to int bc somehow sepList is interpreted as a str
        else:
            aminoAcids_sep[AA] = int(sepList[x][1])

    for x in range(len(wholeList_sorted)):       # whole
        AA = Seq(wholeList_sorted[x][0]).translate()
        if (AA in aminoAcids_whole):
            aminoAcids_whole[AA] += int(wholeList_sorted[x][1])    # cast to int bc somehow sepList is interpreted as a str
        else:
            aminoAcids_whole[AA] = int(wholeList_sorted[x][1])

    
    # reorder the sep list from most frequent to least
    aminoAcids_sep = dict(sorted(aminoAcids_sep.items(), key=lambda aaS:aaS[1], reverse=True))
    
    # convert the dicts into lists
    aa_sepList = [(key, value) for key, value in aminoAcids_sep.items()]
    aa_wholeList = [(key, value) for key, value in aminoAcids_whole.items()]

    # reorder the whole list s.t. its order of AA is the same as the sep list's
    aa_sepMapping = {item[0]: x for x, item in enumerate(aa_sepList)}
    aa_wholeListSorted = sorted(aa_wholeList, key=lambda x:aa_sepMapping[x[0]])
    
    # print(aa_sepList, "\n\n", aa_wholeListSorted)

    # output the lists into csv files

    # for the sep list
    with open("./output/aa_sep.csv", "w") as output:
        writer = csv.writer(output)
        for row in aa_sepList:
            writer.writerow(row)

    # for the whole list
    with open("./output/aa_whole.csv", "w") as output:
        writer = csv.writer(output)
        for row in aa_wholeListSorted:
            writer.writerow(row)

    ########## END problem 6 ##########

main()