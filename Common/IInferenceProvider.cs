using System.Drawing;

namespace ObjectDetection.Common
{
    public interface IInferenceProvider
    {
        /// <summary>
        /// Execute inference on the provided image. 
        /// </summary>
        /// <param name="image">Image that Object Detection will execute on.</param>
        /// <param name="resizeTime">The duration for the image to be resized.</param>
        /// <param name="tensorTime">The duration for the input tensor to be built.</param>
        /// <param name="processingTime">the duration for inference to execute.</param>
        /// <returns>Inference results.</returns>
        Output[] ProcessImage(Image image, out long resizeTime, out long tensorTime, out long processingTime);
    }
}
