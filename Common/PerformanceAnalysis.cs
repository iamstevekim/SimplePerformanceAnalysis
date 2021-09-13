using System.Linq;
using System.Drawing;

namespace ObjectDetection.Common
{
    public class PerformanceAnalysis
    {
        private IInferenceProvider OnnxruntimeProcessor;

        public delegate void MessageOutput(string msg);

        public event MessageOutput HandleOutput;

        public PerformanceAnalysis(IInferenceProvider inferenceProvider)
        {
            OnnxruntimeProcessor = inferenceProvider;
        }

        public void Run(System.IO.DirectoryInfo dir)
        {
            long sumProcessingTime = 0;
            long sumTensorTime = 0;
            long sumResizeTime = 0;

            System.Diagnostics.Stopwatch fileTime = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch totalTime = new System.Diagnostics.Stopwatch();
            totalTime.Start();
            foreach (System.IO.FileInfo imgFile in dir.EnumerateFiles())
            {
                fileTime.Reset();
                fileTime.Start();

                Image image = Image.FromFile(imgFile.FullName);

                Output[] result = OnnxruntimeProcessor.ProcessImage(image, out long resizeTime, out long tensorTime, out long processingTime);

                // Common.Output[] uniqueResults = Common.NonMaxSuppression.Execute(result);

                fileTime.Stop();

                sumProcessingTime += processingTime;
                sumTensorTime += tensorTime;
                sumResizeTime += resizeTime;

                //Console.WriteLine($"File: {imgFile.Name} resizeTime: {resizeTime} ms  tensorTime: {tensorTime} ms  processingTime: {processingTime} ms  totalTime: {fileTime.ElapsedMilliseconds} ms");
                RaiseMessageOutputEvent($"File: {imgFile.Name} resizeTime: {resizeTime} ms  tensorTime: {tensorTime} ms  processingTime: {processingTime} ms  totalTime: {fileTime.ElapsedMilliseconds} ms");
            }

            totalTime.Stop();

            //Console.WriteLine($"Total: image count: {dir.EnumerateFiles().Count()}  time: {totalTime.ElapsedMilliseconds} ms");
            //Console.WriteLine($"Average: resizeTime: {sumResizeTime / dir.EnumerateFiles().Count()} ms  tensorTime: {sumTensorTime / dir.EnumerateFiles().Count()} ms  processingTime: {sumProcessingTime / dir.EnumerateFiles().Count()} ms");
            RaiseMessageOutputEvent($"Total: image count: {dir.EnumerateFiles().Count()}  time: {totalTime.ElapsedMilliseconds} ms");
            RaiseMessageOutputEvent($"Average: resizeTime: {sumResizeTime / dir.EnumerateFiles().Count()} ms  tensorTime: {sumTensorTime / dir.EnumerateFiles().Count()} ms  processingTime: {sumProcessingTime / dir.EnumerateFiles().Count()} ms");
        }

        private void RaiseMessageOutputEvent(string msg)
        {
            try
            {
                HandleOutput(msg);
            }
            catch
            {
                // Do nothing
            }
        }
    }
}
