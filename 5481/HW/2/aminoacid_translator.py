import argparse
import numpy as np
import Bio as bio
from Bio import SeqIO
from Bio.Seq import Seq



# helper function to find the first occurrence of a stop codon
# and provided so, trim the list up to said codon
def find_stopCodon(L):

    stopCodons = ["TAG", "TAA", "TGA"]

    for element in L:
        if element in stopCodons:
            index = L.index(element)
            return L[:index + 1]
    
    print("No stop codon found")
    return L



def aminoacid_translator():
    # set up argument parser
    parser = argparse.ArgumentParser()
    parser.add_argument('-q', '--query', required=True, type=str)
    parser.add_argument('-r', '--reference', required=True, type=str)
    parser.add_argument('-qo', '--query_output', required=True, type=str)
    parser.add_argument('-ro', '--ref_output', required=True, type=str)


    # parsing arguments to get variables (can rename variables anything you choose,
    # this just illustrates how to get contents of the parser)
    args = parser.parse_args()

    qFile, rFile = args.query, args.reference
    qOutFile, rOutFile = args.query_output, args.ref_output

    # parse the files
    qParse = SeqIO.parse(open(qFile), 'fasta')
    rParse = SeqIO.parse(open(rFile), 'fasta')

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

    # find the start codon within the genome -- trim everything up to it
    # and traverse until we reach a stop codon, and trim off everything after
    qAA = [qSeq[x:x+3] for x in range(qSeq.find("ATG"), len(qSeq), 3)]
    rAA = [rSeq[x:x+3] for x in range(rSeq.find("ATG"), len(rSeq), 3)]

    qAA = find_stopCodon(qAA)
    rAA = find_stopCodon(rAA)

    #print(qAA)
    #print(rAA)

    # convert the entire list of Seq codons into a single sequence string
    qAA = ''.join(str(codon) for codon in qAA)
    rAA = ''.join(str(codon) for codon in rAA)
    
    # then translate them into amino acids
    qAA = Seq(qAA).translate()
    rAA = Seq(rAA).translate()

    # and write them to a file
    with open(qOutFile, 'w') as outputFile:
        outputFile.write(">" + str(qID) + "\n")
        outputFile.write(str(qAA) + "\n")

    with open(rOutFile, 'w') as outputFile:
        outputFile.write(">" + str(rID) + "\n")
        outputFile.write(str(rAA) + "\n")


aminoacid_translator()