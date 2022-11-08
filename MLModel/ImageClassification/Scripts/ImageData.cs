using Microsoft.ML.Data;

namespace BlazorMLWebApp.ImageClassification
{
    public class ImageDataInput
    {
        public string imagePath = "";
        public static IEnumerable<ImageDataInput> ReadFromFile(string imageFolder)
        {
            return Directory
                .GetFiles(imageFolder)
                .Where(x => Path.GetExtension(x) == ".jpg" || Path.GetExtension(x) == ".png" || Path.GetExtension(x) == ".jfif")
                .Select(x => new ImageDataInput { imagePath = x });
        }
    }

    public class ImageDataOutput
    {
        [ColumnName("softmaxout_1")]
        public float[] scores = new float[1000];
        public float probability = 0;
        public string label = "";
    }
}