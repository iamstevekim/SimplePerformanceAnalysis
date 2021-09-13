namespace ObjectDetection.OnnxruntimeInference.Yolov5
{
    public class Yolov5s : Yolov5
    {
        public const string Filename = "yolov5s.onnx";

        public Yolov5s(string directoryPath) : base(
            System.IO.Path.Combine(directoryPath, Filename),
            System.IO.Path.Combine(directoryPath, LabelFilename)) { }
    }
}
