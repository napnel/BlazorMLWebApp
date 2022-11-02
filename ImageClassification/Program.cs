using ImageClassification;

const string imageFolder = ".\\Data\\Test";

InferenceModel inference = new InferenceModel(SqueezeNetSettings.modelPath, SqueezeNetSettings.inputColumnName, SqueezeNetSettings.outputColumnName);
IEnumerable<ImageData> images = ImageData.ReadFromFile(imageFolder);
inference.PredictImage(images);