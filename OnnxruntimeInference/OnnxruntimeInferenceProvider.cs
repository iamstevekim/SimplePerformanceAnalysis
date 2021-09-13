using ObjectDetection.Common;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.ML.OnnxRuntime;

namespace ObjectDetection.OnnxruntimeInference
{
    public abstract class OnnxruntimeInferenceProvider : IInferenceProvider
    {
        private readonly InferenceSession Session;
        private IOnnxruntimeModel Model;

        /// <summary>
        /// Instantiates the Onnxruntime Inference Provider.
        /// </summary>
        /// <param name="model">Model to be used by the Onnxruntime Interference Provider.</param>
        /// <param name="options">Session Options for Onnxruntime. If not provided, a default session will be created which uses CPU for execution.</param>
        public OnnxruntimeInferenceProvider(IOnnxruntimeModel model, SessionOptions options = null)
        {
            Model = model;

            if (options == null)
                options = new SessionOptions();

            Session = new InferenceSession(System.IO.File.ReadAllBytes(Model.Filepath), options);
        }

        public Output[] ProcessImage(Image image, out long resizeTime, out long tensorTime, out long processingTime)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap resizedImage = Model.ResizeImage(image);

            sw.Stop();
            resizeTime = sw.ElapsedMilliseconds;

            sw.Reset();
            sw.Start();

            List<NamedOnnxValue> inputs = Model.BuildInput(resizedImage);

            sw.Stop();
            tensorTime = sw.ElapsedMilliseconds;

            sw.Reset();
            sw.Start();

            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = Session.Run(inputs);

            sw.Stop();
            processingTime = sw.ElapsedMilliseconds;

            return Model.Convert(results);
        }
    }
}
