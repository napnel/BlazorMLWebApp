namespace BlazorMLWebApp.ImageClassification.Settings
{
    public static class ModelSettings
    {
        public static readonly string modelPath = @"MLModel\\ImageClassification\\Assets\\SqueezeNet.onnx";
        public static readonly string inputColumnName = "data_0";
        public static readonly string outputColumnName = "softmaxout_1";
    }
}