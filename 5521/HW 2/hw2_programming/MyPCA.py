import numpy as np

class PCA():
    def __init__(self,num_dim=None):
        self.num_dim = num_dim
        self.mean = np.zeros([1,784]) # means of training data
        self.W = None # projection matrix

    def fit(self,X):
        # normalize the data to make it centered at zero (also store the means as class attribute)
        # normalize by subtracting average features (1 x d)
        # mean should be 1 x 784
        # data 2400 x 784
        
        for meanIndex in range(len(self.mean[0])):
            self.mean[0][meanIndex] = np.mean(X.T[meanIndex, :], axis = 0)
        
        m = np.subtract(X, self.mean)

        sigma = np.cov(m, rowvar=0, bias=False, ddof=0)

        # finding the projection matrix that maximize the variance (Hint: for eigen computation, use numpy.eigh instead of numpy.eig)
        # np.eigh -> eigval, eigvector -> in ascending order (last one is largest) -> flip them so that it's in descending order -> indexing -> eigval[::-1], eigvector[:,::-1]
        eigenVals, eigenVectors = np.linalg.eigh(sigma)

        eigenVals = eigenVals[::-1]
        eigenVectors = eigenVectors[:,::-1]

        eigenSum = 0.0
        eigenTotalSum = np.sum(eigenVals)

        # counter -> count the sum of added eigenvalue
        if self.num_dim is None:
            # select the reduced dimension that keep >90% of the variance
            # have a loop -> each iteration add the one eigenvalue to the counter -> check if the condition is satisfied -> if True, break, o.w., num_dim++
            # keep adding eigenvalue until counter/sum of all eigenval > 0.9 -> break
            self.num_dim = 0

            for counter in range(len(eigenVals)):
                if ( (eigenSum / eigenTotalSum) > 0.90):
                    break
                else:
                    eigenSum += eigenVals[self.num_dim]
                    self.num_dim += 1

            # store the projected dimension
            # self.num_dim = 784 # placeholder

        # determine the projection matrix and store it as class attribute
        # the projection matrix should also be in descending order
        self.W = eigenVectors[:,:self.num_dim] # placeholder

        # project the high-dimensional data to low-dimensional one (use an affine transform (or dot product) to convert D dimensional data into num_dum dimensional data)
        X_pca = m.dot(self.W) # placeholder

        return X_pca, self.num_dim

    def predict(self,X):
        # normalize the test data based on training statistics
        # use same w and self.mean
        X -= self.mean

        # project the test data
        X_pca = X.dot(self.W) # placeholder

        return X_pca

    def params(self):
        return self.W, self.mean, self.num_dim
