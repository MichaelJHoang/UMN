import numpy as np

class Normalization:
    def __init__(self,):
        self.mean = np.zeros([1,64]) # means of training features
        self.std = np.zeros([1,64]) # standard deviation of training features

    def fit(self,x):
        # compute the statistics of training samples (i.e., means and std)
        self.mean = x.mean(axis=0)
        self.std = x.std(axis=0)

        pass # placeholder

    def normalize(self,x):
        # normalize the given samples to have zero mean and unit variance (add 1e-15 to std to avoid numeric issue)
        x = (x - self.mean) / (self.std + 1e-15)

        return x

def process_label(label):
    # convert the labels into one-hot vector for training
    # 1000 x 10
    one_hot = np.zeros([len(label),10])
    
    for index in range(len(one_hot)):
        one_hot[index][label[index]] = 1

    return one_hot

def tanh(x):
    # implement the hyperbolic tangent activation function for hidden layer
    x = np.clip(x,a_min=-100,a_max=100) # for stablility, do not remove this line

    f_x = np.tanh(x) # placeholder
    
    return f_x

def softmax(x):
    # implement the softmax activation function for output layer
    f_x = np.exp(x) / np.sum(np.exp(x), axis=1).reshape(-1, 1) # placeholder

    return f_x

class MLP:
    def __init__(self,num_hid):
        # initialize the weights
        self.weight_1 = np.random.random([64,num_hid]) # wh in p1
        self.bias_1 = np.random.random([1,num_hid]) # wh0 in p1
        self.weight_2 = np.random.random([num_hid,10])  # vh in p1
        self.bias_2 = np.random.random([1,10])  # v0

    def fit(self,train_x,train_y, valid_x, valid_y):
        # y train is the 1 hot vector
        # learning rate
        lr = 5e-3
        # counter for recording the number of epochs without improvement
        count = 0
        best_valid_acc = 0

        """
        Stop the training if there is no improvment over the best validation accuracy for more than 50 iterations
        """
        while count<=50:
            # training with all samples (full-batch gradient descents)
            # implement the forward pass (from inputs to predictions)
            z = self.get_hidden(train_x)
            y = softmax(np.matmul(z, self.weight_2) + self.bias_2)

            # implement the backward pass (backpropagation)
            # compute the gradients w.r.t. different parameters
            # compute the gradients by hand and then translate into python code
            # calculate the partial derivative (e.g., delta_w = dE/dW, W = self.weight_1)
            # shape is 64 x 3
            dEdWh = np.dot( train_x.T, np.dot(y - train_y, self.weight_2.T) * (1-z**2) )
            dEdW0 = np.sum(np.dot(y - train_y, self.weight_2.T) * (1-z**2), axis = 0)
            dEdVh = np.dot(z.T, (y - train_y))
            dEdV0 = np.sum(y - train_y, axis=0)

            # (1000, 10)
            # print(dEdW0.shape)
            
            # (64, 3) (3, 10) (1, 3) (1, 10)
            # print(self.weight_1.shape, self.weight_2.shape, self.bias_1.shape, self.bias_2.shape)

            #update the parameters based on sum of gradients for all training samples
            # gradient descent here

            # update rule for W, gradient descent, self.weight1 = selfweight1 - lr*delta_w
            self.weight_1 = self.weight_1 - (lr * dEdWh)
            self.weight_2 = self.weight_2 - (lr * dEdVh)
            self.bias_1 = self.bias_1 - (lr * dEdW0)
            self.bias_2 = self.bias_2 - (lr * dEdV0)

            # evaluate on validation data
            predictions = self.predict(valid_x)
            valid_acc = np.count_nonzero(predictions.reshape(-1)==valid_y.reshape(-1))/len(valid_x)

            # compare the current validation accuracy with the best one
            if valid_acc>best_valid_acc:
                best_valid_acc = valid_acc
                count = 0
            else:
                count += 1

        return best_valid_acc

    def predict(self,x):
        # similar to forward pass
        # generate the predicted probability of different classes

        # convert class probability to predicted labels

        # make use of the trained parameters to get y from x
        # look at softmax output and convert probabilities to labels

        inputLayer = self.get_hidden(x)
        outputLayer = softmax(np.matmul(inputLayer, self.weight_2) + self.bias_2)

        y = np.zeros([len(x),]).astype('int') # placeholder

        for index in range(len(y)):
            # y[index] = outputLayer[index].tolist().index(max(outputLayer[index]))
            y[index] = np.argmax(outputLayer[index])

        return y

    def get_hidden(self,x):
        # extract the intermediate features computed at the hidden layers (after applying activation function)
        # print("X:\n", x.shape) 1867 64
        # print("Self Bias 1:\n", self.bias_1.shape) 1 3
        # print("Self weight 1:\n", self.weight_1.shape) 64 3
        
        z = tanh(np.matmul(x, self.weight_1) + self.bias_1)

        return z

    def params(self):
        return self.weight_1, self.bias_1, self.weight_2, self.bias_2
