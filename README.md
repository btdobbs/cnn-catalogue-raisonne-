# Steps for Experiment Reproduction
## Data
To retrieve data for the experiment, download and run C# code in the wikiart folder.  Further instructions for the wikiart api are documented in the code.  The file names of all images used in experiments is located in images.txt.
## Experiment
Use MATLAB to run the code in matlab/resnet given the data downloaded in the previous step.  Note, the .mat files used for transfer learning are binary and are not added to this repository.  Both of these .mat files are reproducable by creating and saving a sample resnet 18 and 101 experiment in MATLAB.
## Measures
Using the confusion matrix output of the experiment, run the MATLAB code in matlab/measures to reproduces measures.
