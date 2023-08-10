from cProfile import label
import numpy as np

class GaussianDiscriminant:
    def __init__(self,k=2,d=8,priors=None,shared_cov=False):
        self.mean = np.zeros((k,d)) # mean
        self.shared_cov = shared_cov # using class-independent covariance or not
        if self.shared_cov:
            self.S = np.zeros((d,d)) # class-independent covariance (S1=S2)
        else:
            self.S = np.zeros((k,d,d)) # class-dependent covariance (S1!=S2)
        if priors is not None:
            self.p = priors
        else:
            self.p = [1.0/k for i in range(k)] # assume equal priors if not given
        self.k = k
        self.d = d

    def fit(self, Xtrain, ytrain):

        for label in range((len(np.unique(ytrain)))):
            
            indices = [index for index, element in enumerate(ytrain) if element == label + 1]

            #print(indices)
            
            # axis=0 b/c data is multi-dim
            self.mean[label] = np.mean(Xtrain[indices, :], axis=0)

            #numpy determinant

            if self.shared_cov:
                # compute the class-independent covariance
                # xtrain
                self.S = np.cov(Xtrain, rowvar=0, ddof=0)
            else:
                # compute the class-dependent covariance
                self.S[label] = np.cov(Xtrain[indices, :], rowvar=0, ddof=0)

    def predict(self, Xtest):
        # predict function to get predictions on test set
        predicted_class = np.ones(Xtest.shape[0]) # placeholder
        
        for i in np.arange(Xtest.shape[0]): # for each test set example
            tempList = []

            # calculate the value of discriminant function for each class
            for c in np.arange(self.k):
                if self.shared_cov:
                    #print("SHARED:\n", self.S)
                    tempList.append(-(1/2) * np.log( np.linalg.det(self.S) ) - (1/2) * np.dot(Xtest[i] - self.mean[c], np.linalg.inv(self.S)).dot(Xtest[i] - self.mean[c]) + np.log(self.p[c]))
                    #pass
                else:
                    #print("NOT SHARED:\n",self.S[c])
                    tempList.append(-(1/2) * np.log( np.linalg.det(self.S[c]) ) - (1/2) * np.dot(Xtest[i] - self.mean[c], np.linalg.inv(self.S[c])).dot(Xtest[i] - self.mean[c]) + np.log(self.p[c]))
                    #pass

            # determine the predicted class based on the values of discriminant function
            predicted_class[i] = 1 if tempList[0] >= tempList[1] else 2


        return predicted_class

    def params(self):
        if self.shared_cov:
            return self.mean[0], self.mean[1], self.S
        else:
            return self.mean[0],self.mean[1],self.S[0,:,:],self.S[1,:,:]


#PAGE 17 BAYES2 s <== std dev, not S covariance
class GaussianDiscriminant_Diagonal:
    def __init__(self,k=2,d=8,priors=None):
        self.mean = np.zeros((k,d)) # mean
        self.S = np.zeros((d,)) # variance
        if priors is not None:
            self.p = priors
        else:
            self.p = [1.0/k for i in range(k)] # assume equal priors if not given
        self.k = k
        self.d = d
        
    def fit(self, Xtrain, ytrain):

        # compute the mean for each class
        for label in range((len(np.unique(ytrain)))):
            indices = [index for index, element in enumerate(ytrain) if element == label + 1]
            self.mean[label] = np.mean(Xtrain[indices, :], axis=0)
        
        # compute the variance of different features
        for x in range(len(self.S)):
            self.S[x] = np.var(Xtrain.T[x], ddof=0)

        pass # placeholder

    def predict(self, Xtest):
        # predict function to get prediction for test set
        predicted_class = np.ones(Xtest.shape[0]) # placeholder

        #print(self.S)
        for i in np.arange(Xtest.shape[0]): # for each test set example

            tempList = []
            #print(Xtest.shape)
            
            # calculate the value of discriminant function for each class
            for c in np.arange(self.k):
                #print("Xtest:\n", Xtest[i], Xtest[i].shape)
                #print("Mean:\n", self.mean[c], self.mean.shape)
                #print("std:\n", np.std(Xtest))
                
                tempTerm = np.sum( np.power( (Xtest[i] - self.mean[c]) / np.sqrt(self.S) ,2 ) )
                tempList.append(-(1/2) * (tempTerm) + np.log(self.p[c]))

            # determine the predicted class based on the values of discriminant function
            predicted_class[i] = 1 if tempList[0] >= tempList[1] else 2

        return predicted_class

    def params(self):
        return self.mean[0], self.mean[1], self.S
