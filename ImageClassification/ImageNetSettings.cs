namespace ImageClassification
{
    public static class ImageNetSettings
    {
        public static readonly int imageHeight = 224;
        public static readonly int imageWidth = 224;
        public static readonly string[] labels = GetLabels();
        static string[] GetLabels()
        {
            List<string> labels = new List<string>();
            string[] lines = File.ReadAllLines("Data/imagenet_labels.txt");
            foreach (var line in lines)
            {
                int spaceIndex = line.IndexOf(" ");
                string label = line.Substring(spaceIndex + 1);
                labels.Add(label);
            }
            return labels.ToArray();
        }
    }
}