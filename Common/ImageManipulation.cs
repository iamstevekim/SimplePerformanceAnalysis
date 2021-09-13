using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ObjectDetection.Common
{
    public class ImageManipulation
    {
        /// <summary>
        /// Resize image to the specified width and height.
        /// </summary>
        /// <param name="image">Original image to resize.</param>
        /// <param name="width">Target image width.</param>
        /// <param name="height">Target image height.</param>
        /// <returns>Resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.Default;
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                // Originally used HighQuality for the following properties which impact the resizing duration
                // HighSpeed has higher performance but overall inference performance has not been assessed. 
                //graphics.CompositingQuality = CompositingQuality.HighQuality;
                //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //graphics.SmoothingMode = SmoothingMode.HighQuality;
                //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Resize image to the specified width and height.
        /// </summary>
        /// <param name="imageStream">Original image to resize.</param>
        /// <param name="width">Target image width.</param>
        /// <param name="height">Target image height.</param>
        /// <returns>Resized image.</returns>
        public static Bitmap ResizeImage(System.IO.Stream imageStream, int width, int height)
        {
            using (Image image = Image.FromStream(imageStream))
            {
                return ResizeImage(image, width, height);
            }
        }
    }
}
