using Microsoft.ML.OnnxRuntime;

namespace ObjectDetection.OnnxruntimeInference.DirectML
{
    public class OnnxruntimeInferenceProvider_DirectML : OnnxruntimeInferenceProvider
    {
        public static SessionOptions GetSessionOptions(int gpuDeviceId = 0)
        {
            Microsoft.ML.OnnxRuntime.
            SessionOptions options = new SessionOptions();
            options.AppendExecutionProvider_DML(gpuDeviceId);
            return options;
        }
        public OnnxruntimeInferenceProvider_DirectML(IOnnxruntimeModel model) : base(model, GetSessionOptions()) { }
    }
}
