using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightAdjust
{
    class BrightnessOperator
    {
        public static void ChangeBrightness()
        {
            // Capture an image from the primary camera
            VideoCapture cam = new VideoCapture(0);
            Mat img = new Mat();
            cam.Read(img);
            cam.Release();

            for (int i = 0; i < numThreads; i++)
            {
                // Start a thread set to work on 
                Console.WriteLine(i);
            }
        }
    }
}
