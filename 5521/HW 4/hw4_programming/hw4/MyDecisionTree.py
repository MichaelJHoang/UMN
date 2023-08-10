from inspect import istraceback
import numpy as np

class Tree_node:
    """
    Data structure for nodes in the decision-tree
    """
    def __init__(self,):
        self.feature = None # index of the selected feature (for non-leaf node)
        self.label = -1 # class label (for leaf node), -1 means the node is not a leaf node
        self.left_child = None # left child node
        self.right_child = None # right child node

class Decision_tree:
    """
    Decision tree with binary features
    """
    def __init__(self,min_entropy):
        self.min_entropy = min_entropy
        self.root = None

    def fit(self,train_x,train_y):
        # construct the decision-tree with recursion
        self.root = self.generate_tree(train_x,train_y)

    def predict(self,test_x):
        # iterate through all samples
        prediction = np.zeros([len(test_x),]).astype('int') # shape 3000x64 and 562x64, l x w

        # given test_x, what do you predict each row of test_x to be classified as?
        # use the decision tree
        for i in range(len(test_x)):
            # traverse the decision-tree based on the features of the current sample till reaching a leaf node
            tempNode = self.root
            
            while True:
                if tempNode.label == -1:
                    if (test_x[i][tempNode.feature] == 0):
                        tempNode = tempNode.left_child
                    elif (test_x[i][tempNode.feature] == 1):
                        tempNode = tempNode.right_child
                else:
                    prediction[i] = tempNode.label
                    break

        return prediction

    # training step
    def generate_tree(self,data,label):
        # initialize the current tree node
        cur_node = Tree_node()  # current node is unknown

        # compute the node entropy
        node_entropy = self.compute_node_entropy(label)

        # determine if the current node is a leaf node based on minimum node entropy (if yes, find the corresponding class label with majority voting and exit the current recursion)
        if (node_entropy < self.min_entropy):
            cur_node.label = np.bincount(label[cur_node.feature][0]).argmax()
            return cur_node

        # select the feature that will best split the current non-leaf node
        selected_feature = self.select_feature(data, label)
        cur_node.feature = selected_feature

        # split the data based on the selected feature and start the next level of recursion
        leftLabel = label[data.T[selected_feature] == 0]  # get all labels where data.T == 0 / false   // left child
        leftData = data[data.T[selected_feature] == 0]

        rightLabel = label[data.T[selected_feature] == 1]   # get all labels where data.T == 1 / true    // right child
        rightData = data[data.T[selected_feature] == 1]
        
        cur_node.left_child = self.generate_tree(leftData, leftLabel)
        cur_node.right_child = self.generate_tree(rightData, rightLabel)

        return cur_node

    def select_feature(self,data,label):
        # iterate through all features and compute their corresponding entropy
        best_feat = 0
        minEntropy = np.Infinity

        for i in range(len(data[0])):   # 64 iterations for len(data[0]), 3000 for len(label) ==> data is 3000 x 64

            # map features in data.T[i] to labels ===> mapping 1:1 b/c each are len 3000
            # data contains 2 classes: true and false

            classTrue = label[data.T[i] == 1]   # get all labels where data.T == 1 / true
            classFalse = label[data.T[i] == 0]  # get all labels where data.T == 0 / false

            # compute the entropy of splitting based on the selected features
            splitEntropy = self.compute_split_entropy(classTrue, classFalse)

            # select the feature with minimum entropy
            if (splitEntropy < minEntropy):
                minEntropy = splitEntropy
                best_feat = i
            

        return best_feat

    def compute_split_entropy(self,left_y,right_y):
        # compute the entropy of a potential split (with compute_node_entropy function), left_y and right_y are labels for the two branches
        uniqueLeft = np.unique(left_y)
        uniqueRight = np.unique(right_y)

        probabilitiesLeft = np.zeros(len(uniqueLeft))
        probabilitiesRight = np.zeros(len(uniqueRight))

        for x in range (len(uniqueLeft)):
            probabilitiesLeft[x] = np.count_nonzero(left_y == uniqueLeft[x]) / (len(left_y) + len(right_y))

        for x in range (len(uniqueRight)):
            probabilitiesRight[x] = np.count_nonzero(right_y == uniqueRight[x]) / (len(right_y) + len(left_y))

        leftEntropy = np.sum(probabilitiesLeft * self.compute_node_entropy(left_y))
        rightEntropy = np.sum(probabilitiesRight * self.compute_node_entropy(right_y))

        split_entropy = leftEntropy + rightEntropy

        return split_entropy

    def compute_node_entropy(self,label):
        # compute the entropy of a tree node (add 1e-15 inside the log2 when computing the entropy to prevent numerical issue)
        uniqueLabels = np.unique(label)

        probabilities = np.zeros(len(uniqueLabels))
        
        for x in range (len(uniqueLabels)):
            probabilities[x] = np.count_nonzero(label == uniqueLabels[x]) / len(label)
        
        node_entropy = np.sum(-probabilities * np.log2(probabilities + 1e-15))

        return node_entropy
