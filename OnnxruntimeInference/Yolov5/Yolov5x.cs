namespace ObjectDetection.OnnxruntimeInference.Yolov5
{
    public class Yolov5x : Yolov5
    {
        public const string Filename = "yolov5x.onnx";

        public Yolov5x(string directoryPath) : base(
            System.IO.Path.Combine(directoryPath, Filename),
            System.IO.Path.Combine(directoryPath, LabelFilename)) { }
    }
}
