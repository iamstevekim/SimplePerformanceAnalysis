using System.Collections.Generic;
using System.Drawing;
using ObjectDetection.Common;
using Microsoft.ML.OnnxRuntime;

namespace ObjectDetection.OnnxruntimeInference
{
    public interface IOnnxruntimeModel
    {
        string Filepath { get; }

        int ProcessingImageSize { get; }

        Bitmap ResizeImage(Image image);

        List<NamedOnnxValue> BuildInput(Bitmap image);

        Output[] Convert(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results);
    }
}
