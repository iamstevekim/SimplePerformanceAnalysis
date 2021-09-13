using System;
using System.IO;
using ObjectDetection.Common;
using ObjectDetection.OnnxruntimeInference;
using ObjectDetection.OnnxruntimeInference.Cuda;

namespace Test.OnnxruntimeInference.Cuda
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ***** Onnxruntime Cuda Performance Analysis ***** ");

            Console.WriteLine(" Enter Model/Label directory:");
            string modelDirectoryPath = GetDirectoryInput("Enter Model/Label directory:");

            IOnnxruntimeModel model = new ObjectDetection.OnnxruntimeInference.Yolov5.Yolov5s(modelDirectoryPath);

            // Cuda
            IInferenceProvider inferenceProvider = new OnnxruntimeInferenceProvider_Cuda(model);

            PerformanceAnalysis test = new(inferenceProvider);
            test.HandleOutput += (message) => { Console.WriteLine(message); };

            Console.WriteLine(" Enter Image directory:");
            string imageDirectoryPath = GetDirectoryInput("Enter Image directory:");

            test.Run(new DirectoryInfo(imageDirectoryPath));

            Console.WriteLine(" Test complete. Hit any key to close.");
            Console.ReadLine();
        }

        private static string GetDirectoryInput(string errorMessage)
        {
            string directory;
            do
            {
                directory = Console.ReadLine();
                if (!Directory.Exists(directory))
                {
                    directory = string.Empty;
                    Console.WriteLine($" Error: Invalid directory. {errorMessage}");
                }
            } while (directory == string.Empty);
            return directory;
        }
    }
}
