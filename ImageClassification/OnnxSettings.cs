namespace BlazorMLWebApp.ImageClassification
{
    public static class SqueezeNetSettings
    {
        public static readonly string modelPath = ".\\Onnx\\SqueezeNet.onnx";
        public static readonly string inputColumnName = "data_0";
        public static readonly string outputColumnName = "softmaxout_1";
    }
}