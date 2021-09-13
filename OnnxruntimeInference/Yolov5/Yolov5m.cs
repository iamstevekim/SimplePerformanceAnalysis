namespace ObjectDetection.OnnxruntimeInference.Yolov5
{
    public class Yolov5m : Yolov5
    {
        public const string Filename = "yolov5m.onnx";
        
        public Yolov5m(string directoryPath) : base(
            System.IO.Path.Combine(directoryPath, Filename), 
            System.IO.Path.Combine(directoryPath, LabelFilename)) { }
    }
}
