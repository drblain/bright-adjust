using OpenCvSharp;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Management;

namespace BrightAdjust
{
    class BrightnessOperator
    {
        // Array containing the scalars for the BGR components of each pixel
        private static readonly double[] bgr_scalars = { 0.0722, 0.7152, 0.2126 };
        private static readonly int num_threads = 3;
        private static readonly int video_device = 0;

        // Store brightness management instances here to avoid initialization every time this is called
        private static ManagementObject brightness_instance = null;
        private static ManagementBaseObject brightness_class = null;

        public static void WorkerMain(ref Mat img, int thread_id, out double sum)
        {
            sum = 0.0;
            for (int i = 0; i < img.Rows; i++)
            {
                for (int j = 0; j < img.Cols; j++)
                {
                    Vec3b bgrpixel = img.At<Vec3b>(i, j);
                    // Add the appropriate value to the output
                    switch (thread_id)
                    {
                        case 0:
                            sum += bgrpixel.Item0;
                            break;
                        case 1:
                            sum += bgrpixel.Item1;
                            break;
                        case 2:
                            sum += bgrpixel.Item2;
                            break;
                    }
                }
            }

            // Scale by the appropriate factor
            sum *= bgr_scalars[thread_id];
        }

        public static int CalcBrightness()
        {
            // Capture an image from the primary camera
            VideoCapture cam = new VideoCapture(video_device);
            Mat img = new Mat();
            cam.Read(img);
            cam.Release();

            double[] bgr_sums = { 0.0, 0.0, 0.0 };

            // HARD CODED NUMBER OF THREADS TO USE
            int working_thread_count = num_threads;

            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {
                Parallel.For(0, num_threads, i =>
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        WorkerMain(ref img, i, out bgr_sums[i]);
                        if (Interlocked.Decrement(ref working_thread_count) == 0)
                        {
                            resetEvent.Set();
                        }
                    });
                });

                resetEvent.WaitOne();
            }

            // Compute the final luma value and set the brightness proportionately
            // Do this by averaging the luma and normalizing the value to a 100-based scale
            int scaled_avg_luma = (int)((bgr_sums.Sum() * 100) / (img.Rows * img.Cols * 255));
            img.Dispose();
            return scaled_avg_luma;
        }

        public static void SetBrightness(int brightness)
        {
            if (brightness < 0)
            {
                brightness = 0;
            }

            if (brightness > 100)
            {
                brightness = 100;
            }

            var inParams = brightness_instance.GetMethodParameters("WmiSetBrightness");
            inParams["Brightness"] = brightness;
            inParams["Timeout"] = 0;
            brightness_instance.InvokeMethod("WmiSetBrightness", inParams, null);
        }

        public static void ChangeBrightness()
        {
            int brightness = CalcBrightness();

            if (brightness_instance == null || brightness_class == null)
            {
                // initialize

                var searcher = new ManagementObjectSearcher(
                    "root\\WMI",
                    "SELECT * FROM WmiMonitorBrightness");

                var results = searcher.Get();
                var resultEnum = results.GetEnumerator();
                resultEnum.MoveNext();
                brightness_class = resultEnum.Current;

                var instanceName = (string)brightness_class["InstanceName"];
                brightness_instance = new ManagementObject(
                    "root\\WMI",
                    "WmiMonitorBrightnessMethods.InstanceName='" + instanceName + "'",
                    null);
            }

            SetBrightness(brightness);
        }
    }
}
