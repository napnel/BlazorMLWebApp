using Microsoft.ML;
using BlazorMLWebApp.ImageClassification.Settings;

namespace BlazorMLWebApp.ImageClassification
{
    public class InferenceModel
    {
        MLContext mlContext = new MLContext();
        ITransformer model;
        public InferenceModel()
        {
            model = LoadModel(ModelSettings.modelPath, ModelSettings.inputColumnName, ModelSettings.outputColumnName);
        }

        public InferenceModel(Stream zipFileUrl)
        {
            model = LoadModelFromZip(zipFileUrl);
        }

        public ImageDataOutput PredictImage(ImageDataInput image)
        {
            PredictionEngine<ImageDataInput, ImageDataOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ImageDataInput, ImageDataOutput>(model);
            ImageDataOutput prediction = predictionEngine.Predict(image);
            prediction.probability = prediction.scores.Max();
            prediction.label = ImageNetSettings.labels[prediction.scores.ToList().IndexOf(prediction.probability)];
            Console.WriteLine($"Label: {prediction.label}");
            Console.WriteLine($"Probability: {prediction.probability}");
            return prediction;
        }

        public List<ImageDataOutput> PredictImages(IEnumerable<ImageDataInput> images)
        {
            // Transform IDataView
            IDataView imageDataView = mlContext.Data.LoadFromEnumerable(images);

            // Predict the image
            IDataView outputs = model.Transform(imageDataView);
            var predictions = mlContext.Data.CreateEnumerable<ImageDataOutput>(outputs, reuseRowObject: false).ToList();
            foreach (var prediction in predictions)
            {
                float[] probs = prediction.scores;
                int maxIndex = probs.ToList().IndexOf(probs.Max());
                // prediction.probability = probs[maxIndex];
                // prediction.label = ImageNetSettings.labels[maxIndex];
                // Console.WriteLine($"Predicted Label: {ImageNetSettings.labels[maxIndex]} with probability {probs[maxIndex]}");
            }
            return predictions;
        }

        public string ExportZipFile(string savePath)
        {
            this.mlContext.Model.Save(model, null, savePath);
            return savePath;
        }

        private ITransformer LoadModel(string modelPath, string inputColumnName, string outputColumnName)
        {
            var data = mlContext.Data.LoadFromEnumerable(new List<ImageDataInput>());
            var pipeline = mlContext.Transforms.LoadImages(outputColumnName: inputColumnName, imageFolder: "", inputColumnName: nameof(ImageDataInput.imagePath))
                .Append(mlContext.Transforms.ResizeImages(outputColumnName: inputColumnName, imageWidth: ImageNetSettings.imageWidth, imageHeight: ImageNetSettings.imageHeight, inputColumnName: inputColumnName))
                .Append(mlContext.Transforms.ExtractPixels(outputColumnName: inputColumnName))
                .Append(mlContext.Transforms.ApplyOnnxModel(modelFile: modelPath, outputColumnName: outputColumnName, inputColumnName: inputColumnName));
            var model = pipeline.Fit(data);
            return model;
        }

        public ITransformer LoadModelFromZip(Stream zipFileUrl)
        {
            return mlContext.Model.Load(zipFileUrl, out var modelInputSchema);
        }
    }
}