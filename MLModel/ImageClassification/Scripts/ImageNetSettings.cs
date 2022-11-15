namespace BlazorMLWebApp.ImageClassification.Settings
{
    public static class ImageNetSettings
    {
        public static readonly int imageHeight = 224;
        public static readonly int imageWidth = 224;
        public static readonly string[] labels = LoadLabels();
        static string[] LoadLabels()
        {
            List<string> labels = new List<string>();
            Console.WriteLine("Loading labels...");
            string[] lines = File.ReadAllLines(@"MLModel\\ImageClassification\\Assets\\imagenet_labels.txt");
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