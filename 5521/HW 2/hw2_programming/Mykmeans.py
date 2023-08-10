#import libraries
import numpy as np

class Kmeans:
    def __init__(self,k=8): # k is number of clusters
        self.num_cluster = k
        self.center = None # centers for different clusters
        self.cluster_label = np.zeros([k,]) # class labels for different clusters
        self.error_history = [] # don't touch

    # y is cluster label
    def fit(self, X, y):
        # X is matrix Nxd

        # initialize the centers of clusters as a set of pre-selected samples
        # initial indices
        init_idx = [1, 200, 500, 1000, 1001, 1500, 2000, 2005]

        # TA Notes:
        # use the indices provided in init_idx to find the features of corresponding samples -> self.center kxd matrix
        # for self.center[0] = 1xd feature of the 2nd sample (i.e., samples indexed at 1)
        self.center = X[init_idx] 

        num_iter = 0 # number of iterations for convergence

        # initialize the cluster assignment
        prev_cluster_assignment = np.zeros([len(X),]).astype('int')

        # each cluster, or "row" is gonna be assigned a digit
        cluster_assignment = np.zeros([len(X),]).astype('int')
        
        is_converged = False

        # Notes:
        # the basic undelying idea: self.center are the centers for X and both of them have (2400, d) dimensions. y is the label for each index X and cluster label is mapped to self.center
        # each row of X is actually a data point (d dimensional --> 784 dimensions, 73, 1, etc)
        # print ("Cluster Label: ", self.cluster_label, "\ny: ", y.shape, "\nself.center: ", self.center.shape, "\nX: ",X.shape, "\nCluster Assignment: ", cluster_assignment.shape, "\n")

        # iteratively update the centers of clusters till convergence
        while not is_converged:

            # iterate through the samples and compute their cluster assignment
            # E step
            for i in range(len(X)):

                # TA Notes:
                # use euclidean distance to measure the distance between sample and cluster centers (8 clusters -> 8 distances)
                # each sample is 1xd matrix, centers are kxd, the sum of square differences -> (1xd-kxd)**2.sum(-1) -> k-d vector

                # Store distances of each point at index i from the centers
                distances = [] 
                
                for centerIndex in range(len(self.center)):
                    distances.append( np.linalg.norm(X[i] - self.center[centerIndex]) )

                # Determine the cluster assignment based on the index of the lowest distance in distances[]
                cluster_assignment[i] = distances.index(min(distances))

            # M step
            # update self.center based on cluster assignment
            # iterate through different clusters -> find the samples assigned to the cluster -> use the average features of the selected samples as the new center
            centerMeans = np.zeros(self.center.shape)

            for centerIndex in range(len(X)):
                centerMeans[cluster_assignment[centerIndex]] += (X[centerIndex] / np.count_nonzero(cluster_assignment == cluster_assignment[centerIndex]))

            self.center = centerMeans

            # compute the reconstruction error for the current iteration
            cur_error = self.compute_error(X, cluster_assignment)
            self.error_history.append(cur_error)

            # reach convergence if the assignment does not change anymore
            is_converged = True if (cluster_assignment==prev_cluster_assignment).sum() == len(X) else False
            prev_cluster_assignment = np.copy(cluster_assignment)
            num_iter += 1

        # TA Notes:
        # compute the class label (k dimensions) of each cluster based on majority voting (remember to update the corresponding class attribute)
        # if the most common class label in cluster 0 is digit 0 (0, 8, 9), then self.cluster_label[0] = 0
        # iterate through different clusters -> for each cluster, find the labels of corresponding samples (labels are stored in y) -> find the most common labels
        
        # create a list of lists that contains the labels of y that map to each label
        tempList = np.zeros((self.num_cluster, (max(y) + 1).astype('int')))

        # iterate through each cluster, and for each cluster, find the labels corresponding to the index (labels stored in y)
        for x in range (len(cluster_assignment)):
            tempList[cluster_assignment[x]][y[x].astype('int')] += 1

        # convert tempList from numpy's ndarray to python list
        tempList = tempList.tolist()

        # most common label in the list of lists is assigned to the index
        for x in range(len(self.cluster_label)):
            self.cluster_label[x] = tempList[x].index(max(tempList[x]))

        return num_iter, self.error_history

    def predict(self,X):
        # predicting the labels of test samples based on their clustering results
        prediction = np.ones([len(X),]) # placeholder

        cluster_assignment = np.zeros([len(X),]).astype('int')

        # iterate through the test samples
        for i in range(len(X)):
            # find the cluster of each sample
            # Store distances of each point at index i from the centers
            distances = [] 
            
            for centerIndex in range(len(self.center)):
                distances.append( np.linalg.norm(X[i] - self.center[centerIndex]) )

            # Determine the cluster assignment based on the index of the lowest distance in distances[]
            cluster_assignment[i] = distances.index(min(distances))

            # use the class label of the selected cluster as the predicted class
            prediction[i] = self.cluster_label[cluster_assignment[i]]

        # print(cluster_assignment)
        return prediction

    def compute_error(self,X,cluster_assignment):
        # compute the reconstruction error for given cluster assignment and centers
        error = 0 # placeholder
        # if the current sample is assigned to cluster 0 -> reconstruction error for the sample will be (X[i]-self.center[0])**2.sum()
        for x in range(len(X)):
            error += ((X[x] - self.center[cluster_assignment[x]])**2).sum()

        return error

    def params(self):
        return self.center, self.cluster_label
