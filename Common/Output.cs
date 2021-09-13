namespace ObjectDetection.Common
{
    public class Output
    {
        public readonly float OverallConfidence;

        public readonly string Label;
        public readonly float Confidence;
        
        public float X1;
        public float Y1;
        public float X2;
        public float Y2;

        public Output(string label, float confidence)
        {
            Label = label;
            Confidence = confidence;
        }
    }
}
