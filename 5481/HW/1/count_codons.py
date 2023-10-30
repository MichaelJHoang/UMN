import csv
import sys
import numpy as np
import Bio as bio
from Bio import SeqIO
import matplotlib.pyplot as plt

def main():

    # read cmd line args
    inputFile = sys.argv[1]
    outputFile = sys.argv[2]

    # use Bio to open and parse the .fna file
    fnaFile = SeqIO.parse(open(inputFile), 'fasta')

    # split the sequences into codones
    codonList = []
    # with the list of codones, build a hashmap with it of <codon, count>
    codonDict = {}

    for f in fnaFile:
        # split the sequences into groups of 3
        codonList = [f.seq[x:x+3] for x in range(0, len(f.seq), 3)]
        # count the codon frequencies in each sequence
        for codon in codonList:
            # in case there's a "codon" with 1 or 2, don't consider it
            if (len(codon) == 3):
                # if there's a codon that already exists, increment its count
                if (codon in codonDict):
                    codonDict[codon] += 1
                # otherwise, create a new one
                else:
                    codonDict[codon] = 1

    # sort the dictionary from most frequent to the least frequent
    codonDict = dict(sorted(codonDict.items(), key=lambda cd: cd[1], reverse=True))

    # sanity check to verify output
    #for k, v in codonDict.items():
        #print(k, v)
        #pass

    # output codonDict's contents to a csv file
    # csv file will contain two cols per row:
    # codon, frequency
    with open(outputFile, 'w') as csvOutputFile:
        writer = csv.writer(csvOutputFile)
        #writer.writerow(['Codon', 'Frequency']) # not sure as to whether or not to include this
        # codonDict.items() returns a tuple
        for row in codonDict.items():
            writer.writerow(row)

main()
