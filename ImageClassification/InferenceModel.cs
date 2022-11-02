using Microsoft.ML;

namespace ImageClassification
{
    public class InferenceModel
    {
        MLContext mlContext;
        ITransformer model;
        public InferenceModel(string modelPath, string inputColumnName, string outputColumnName)
        {
            this.mlContext = new MLContext();
            this.model = LoadModel(modelPath, inputColumnName, outputColumnName);
        }

        public List<string> PredictImage(IEnumerable<ImageData> images)
        {
            // Transform IDataView
            IDataView imageDataView = mlContext.Data.LoadFromEnumerable(images);

            // Predict the image
            IDataView outputs = model.Transform(imageDataView);
            var predictions = mlContext.Data.CreateEnumerable<ImageData>(outputs, reuseRowObject: false).ToList();
            List<string> res = new List<string>();
            foreach (var prediction in predictions)
            {
                Console.WriteLine($"Image: {prediction.ImagePath}");
                float[] probs = prediction.Scores;
                int maxIndex = probs.ToList().IndexOf(probs.Max());
                Console.WriteLine($"Predicted Label: {ImageNetSettings.labels[maxIndex]} with probability {probs[maxIndex]}");
                res.Add(ImageNetSettings.labels[maxIndex]);
            }
            return res;
        }

        private ITransformer LoadModel(string modelPath, string inputColumnName, string outputColumnName)
        {
            var data = mlContext.Data.LoadFromEnumerable(new List<ImageData>());
            var pipeline = mlContext.Transforms.LoadImages(outputColumnName: inputColumnName, imageFolder: "", inputColumnName: nameof(ImageData.ImagePath))
                .Append(mlContext.Transforms.ResizeImages(outputColumnName: inputColumnName, imageWidth: ImageNetSettings.imageWidth, imageHeight: ImageNetSettings.imageHeight, inputColumnName: inputColumnName))
                .Append(mlContext.Transforms.ExtractPixels(outputColumnName: inputColumnName))
                .Append(mlContext.Transforms.ApplyOnnxModel(modelFile: modelPath, outputColumnName: outputColumnName, inputColumnName: inputColumnName));
            return model = pipeline.Fit(data);
        }
    }
}