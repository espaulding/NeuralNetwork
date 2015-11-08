#Author: Eric Spaulding
#CSCI 557 - Machine Learning
#Project6 - Neural Network
#Spring 2014

#source("barplot.R")

#setup the data
errors = c(89,87,87,87,87,
           87,87,87,87,41,
           41,28,18,13,8,
           6,5,3,3,3,
           3,2,2,2,2,
           3,3,3,3,3,
           3,3,3,3,2,2)
epochs = seq(0,length(errors) - 1) * 20
epochs[1] = 1


#make the plot
barplot(errors,names.arg=epochs,main="Error Plot",
        xlab="Number of Epochs",ylab="Number classified incorrectly")
