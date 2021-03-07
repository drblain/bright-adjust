using BrightAdjust.Properties;
using OpenCvSharp;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BrightAdjust
{
    class BrightnessTrayContext : SysTrayAppContext
    {
        public BrightnessTrayContext()
        {
            this.TrayIcon.Icon = Resources.SmallIcon;

            this.ContextMenu.Items.Add("Adjust &Brightness", null, this.AdjustBrightnessContextMenuHandler).Font = new Font(ContextMenu.Font, FontStyle.Bold);
            this.ContextMenu.Items.Add("-");
            this.ContextMenu.Items.Add("E&xit", null, this.ExitContextMenuClickHandler);
        }

        protected override void OnTrayIconDoubleClicked(MouseEventArgs e)
        {
            this.HandleChangeBrightness();

            base.OnTrayIconDoubleClicked(e);
        }

        private void AdjustBrightnessContextMenuHandler(object sender, EventArgs e)
        {
            this.HandleChangeBrightness();
        }

        private void ExitContextMenuClickHandler(object sender, EventArgs e)
        {
            this.ExitThread();
        }

        private void HandleChangeBrightness()
        {
            // The argument to change brightness is the number of threads to use during execution
            this.ChangeBrightness(4);
        }

        private void ChangeBrightness(int numThreads)
        {
            // Capture an image from the primary camera
            VideoCapture cam = new VideoCapture(0);
            Mat img = new Mat();
            cam.Read(img);
            cam.Release();
            Console.WriteLine(img);
        }

    }
}
