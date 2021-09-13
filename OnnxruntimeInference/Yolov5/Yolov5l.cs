namespace ObjectDetection.OnnxruntimeInference.Yolov5
{
    public class Yolov5l : Yolov5
    {
        public const string Filename = "yolov5l.onnx";

        public Yolov5l(string directoryPath) : base(
            System.IO.Path.Combine(directoryPath, Filename),
            System.IO.Path.Combine(directoryPath, LabelFilename)) { }
    }
}
