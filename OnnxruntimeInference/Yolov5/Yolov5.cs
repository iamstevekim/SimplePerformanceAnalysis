using Microsoft.ML.OnnxRuntime;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Microsoft.ML.OnnxRuntime.Tensors;
using ObjectDetection.Common;

namespace ObjectDetection.OnnxruntimeInference.Yolov5
{
    public class Yolov5 : IOnnxruntimeModel
    {
        public const string LabelFilename = "coco80.onnx";

        private const string InputParameter = "images";
        private const string OutputParameter = "output";

        private string InternalFilepath;
        private string InternalLabelFilepath;
        private const int ImageSize = 640;

        // 0,1,2,3 ->box, 4->confidence，5-85 -> coco classes confidence 
        private const int PredictionXCoordinateIndex = 0;
        private const int PredictionYCoordinateIndex = 1;
        private const int PredictionWidthIndex = 2;
        private const int PredictionHeightIndex = 3;
        private const int PredictionConfidenceIndex = 4;
        private const int PredictionLabelStartingIndex = 5;
        private const int PredictionLabelCount = 80;

        public string Filepath => InternalFilepath;

        public int ProcessingImageSize => ImageSize;

        public Yolov5(string modelFilepath, string labelFilepath) 
        {
            InternalFilepath = modelFilepath;
            InternalLabelFilepath = labelFilepath;
        }

        public Bitmap ResizeImage(Image image)
        {
            return ImageManipulation.ResizeImage(image, ImageSize, ImageSize);
        }

        public List<NamedOnnxValue> BuildInput(Bitmap image)
        {
            return new List<NamedOnnxValue>() {
                NamedOnnxValue.CreateFromTensor(InputParameter, BuildTensorInput(image))
            };
        }

        private unsafe Tensor<float> BuildTensorInput(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte* scan0 = (byte*)data.Scan0.ToPointer();

            int dimension = ImageSize * ImageSize;
            int size = 1 * 3 * dimension;

            float[] matrix = new float[size];

            var mean = new[] { 102.9801f, 115.9465f, 122.7717f };
            for (int height = 0; height < data.Height; height++)
            {
                for (int width = 0; width < data.Width; width++)
                {
                    byte* pixelData = scan0 + height * data.Stride + width * 3;

                    int relativeIndex = (height * ImageSize) + width;
                    matrix[(dimension * 0) + relativeIndex] = ((float)pixelData[0] - mean[0]) / 255;
                    matrix[(dimension * 1) + relativeIndex] = ((float)pixelData[1] - mean[1]) / 255;
                    matrix[(dimension * 2) + relativeIndex] = ((float)pixelData[2] - mean[2]) / 255;
                }
            }

            image.UnlockBits(data);

            int[] inputDimensions = new[] { 1, 3, ImageSize, ImageSize };
            return new DenseTensor<float>(matrix, inputDimensions);
        }

        public Output[] Convert(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results)
        {
            //DisposableNamedOnnxValue result = results.ToArray()[0];
            DisposableNamedOnnxValue result = results.Where((r) => r.Name == OutputParameter).FirstOrDefault();
            if (result == null)
                return new Output[] { };

            float[] output = result.AsEnumerable<float>().ToArray();
            int size = output.Count();
            int dimensions = PredictionLabelStartingIndex + PredictionLabelCount;
            int rowCount = size / dimensions;

            List<Output> predictions = new List<Output>();
            for (int row = 0; row < rowCount; ++row)
            {
                int index = row * dimensions;

                // predictionConfidence is not currently used nor returned
                float predictionConfidence = output[index + PredictionConfidenceIndex];

                for (int labelIndex = PredictionLabelStartingIndex; labelIndex < dimensions; ++labelIndex)
                {
                    float labelConfidence = output[index + labelIndex];
                    int label = labelIndex - PredictionLabelStartingIndex;
                    string labelName = GetLabelName(label);

                    if (labelConfidence > 0.4f)
                    {
                        Output prediction = new Output(label.ToString(), labelConfidence);
                        prediction.X1 = output[index + PredictionXCoordinateIndex] - output[index + PredictionWidthIndex] / 2;
                        prediction.Y1 = output[index + PredictionYCoordinateIndex] - output[index + PredictionHeightIndex] / 2;
                        prediction.X2 = output[index + PredictionXCoordinateIndex] + output[index + PredictionWidthIndex] / 2;
                        prediction.Y2 = output[index + PredictionYCoordinateIndex] + output[index + PredictionHeightIndex] / 2;
                        predictions.Add(prediction);
                    }
                }

            }
            return predictions.ToArray();
        }

        private string GetLabelName(int label)
        {
            // Incomplete
            return label.ToString();
        }
    }
}
