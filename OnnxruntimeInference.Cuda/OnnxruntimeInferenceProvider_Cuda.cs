using Microsoft.ML.OnnxRuntime;

namespace ObjectDetection.OnnxruntimeInference.Cuda
{
    public class OnnxruntimeInferenceProvider_Cuda : OnnxruntimeInferenceProvider
    {
        public static SessionOptions GetSessionOptions(int gpuDeviceId = 0)
        {
            return SessionOptions.MakeSessionOptionWithCudaProvider(gpuDeviceId);
        }
        public OnnxruntimeInferenceProvider_Cuda(IOnnxruntimeModel model) : base(model, GetSessionOptions()) { }
    }
}
