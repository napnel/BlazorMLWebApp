using BlazorMLWebApp.ImageClassification;
using BlazorMLWebApp.ImageClassification.Settings;
using Microsoft.ML;

InferenceModel inference = new InferenceModel();
string exportPath = System.IO.Path.GetFullPath(@"Client\\wwwroot\\model.zip");
Console.WriteLine($"Exporting model to {exportPath}");
inference.ExportZipFile(exportPath);

// InferenceModel inferenceTest = new InferenceModel(filePath: exportPath);
// ImageDataOutput pred = inferenceTest.PredictImage(new ImageDataInput { imagePath = @"MLModel\\ImageClassification\\Assets\\images\\dog.png" });

MLContext mlContext = new MLContext();
ITransformer model = mlContext.Model.Load(exportPath, out var modelInputSchema);
PredictionEngine<ImageDataInput, ImageDataOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ImageDataInput, ImageDataOutput>(model);
ImageDataOutput pred = predictionEngine.Predict(new ImageDataInput { imagePath = @"MLModel\\ImageClassification\\Assets\\images\\dog.png" });
pred.probability = pred.scores.Max();
pred.label = ImageNetSettings.labels[pred.scores.ToList().IndexOf(pred.probability)];
Console.WriteLine($"Label: {pred.label}");
Console.WriteLine($"Probability: {pred.probability}");