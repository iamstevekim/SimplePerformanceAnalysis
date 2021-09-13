using Microsoft.ML.OnnxRuntime;

namespace ObjectDetection.OnnxruntimeInference.Cpu
{
    public class OnnxruntimeInferenceProvider_Cpu : OnnxruntimeInferenceProvider
    {
        public static SessionOptions GetSessionOptions(int gpuDeviceId = 0)
        {
            return new SessionOptions();
        }
        public OnnxruntimeInferenceProvider_Cpu(IOnnxruntimeModel model) : base(model, GetSessionOptions()) { }
    }
}
