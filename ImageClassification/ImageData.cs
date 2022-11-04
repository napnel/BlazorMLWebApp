using Microsoft.ML.Data;

namespace BlazorMLWebApp.ImageClassification
{
    public class ImageData
    {
        public string ImagePath = "";
        public string Label = "";
        [ColumnName("softmaxout_1")]
        public float[] Scores = new float[1000];

        public static IEnumerable<ImageData> ReadFromFile(string imageFolder)
        {
            return Directory
                .GetFiles(imageFolder)
                .Where(x => Path.GetExtension(x) == ".jpg" || Path.GetExtension(x) == ".png" || Path.GetExtension(x) == ".jfif")
                .Select(x => new ImageData { ImagePath = x, Label = Path.GetFileName(x) });
        }
    }
}