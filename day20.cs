namespace adventCode21
{
    public class day20
    {
        private static bool real = true;
        
        private string file = real ? "day20_input.txt" : "day20_inputTest.txt";


        public void execute()
        {
            //do1();
            do2();
        }

        private void do2()
        {
            var rawInput = InputConverter.getInput(file).ToList();

            var algo = rawInput[0];

            var rawImage = rawInput.GetRange(2, rawInput.Count-2);

            var image = ImageEnhancement(rawImage, algo, 50);

            var count = image.Select(l => l.Count(c => c == '#')).Sum();

            Console.WriteLine("Lit Pixel: {0}", count);
        }

        private void do1()
        {
            var rawInput = InputConverter.getInput(file).ToList();

            var algo = rawInput[0];

            var rawImage = rawInput.GetRange(2, rawInput.Count-2);

            var image = ImageEnhancement(rawImage, algo, 2);

            var count = image.Select(l => l.Count(c => c == '#')).Sum();

            Console.WriteLine("Lit Pixel: {0}", count);
        }

        private List<string> ImageEnhancement(List<string> inputImage, string algo, int cycle)
        {
            var image = enlargeImage(inputImage, cycle);

            for (int i = 0; i < cycle; i++)
            {
                image = enhanceImage(image, algo);
            }

            return image;
        }

        private List<string> enlargeImage(List<string> inputImage, int cycle)
        {
            var enlargedImage = inputImage.Select(pixel => new String(pixel)).ToList();
            for (int i = 0; i < enlargedImage.Count; i++)
            {
                enlargedImage[i] = new String('.', cycle*2) + enlargedImage[i] + new String('.', cycle*2);
            }

            for (int i = 0; i < cycle*2; i++)
            {
                enlargedImage.Add(new string('.', enlargedImage[0].Length));
                enlargedImage.Insert(0, new String('.', enlargedImage[0].Length));
            }

            return enlargedImage;
        }

        private List<string> enhanceImage(List<string> inputImage, string algorithm)
        {
            var outputImage = new List<string>();
            var workingImage = inputImage.Select(pixel => new String(pixel)).ToList();

            var numberImage = workingImage.Select(l => l.Replace('.', '0')).Select(s => s.Replace('#', '1')).ToArray();
            var image = InputConverter.get2dArray(numberImage);

            for (int y = 1; y < image.GetUpperBound(0); y++)
            {
                var line = String.Empty;
                for (int x = 1; x < image.GetUpperBound(1); x++)
                {
                    var binary = String.Join(String.Empty,   image[y-1,x-1], image[y-1,x], image[y-1,x+1], 
                                                image[y,x-1], image[y,x], image[y,x+1], 
                                                image[y+1,x-1], image[y+1,x], image[y+1,x+1]);
                    var index = Convert.ToInt32(binary, 2);
                    line += algorithm[index];

                }
                outputImage.Add(line);
            }

            return outputImage;
        }
    }
}