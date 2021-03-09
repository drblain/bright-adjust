using BrightAdjust.Properties;
using System;
using System.Windows.Forms;

namespace BrightAdjust
{
    class BrightnessTrayContext : SysTrayAppContext
    {
        public BrightnessTrayContext()
        {
            this.TrayIcon.Icon = Resources.SmallIcon;
            this.TrayIcon.Text = "Double click to adjust screen brightness";

            this.ContextMenu.Items.Add("Adjust &Brightness", null, this.AdjustBrightnessContextMenuHandler);
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
            this.ChangeBrightness();
        }

        private void ChangeBrightness()
        {
            BrightnessOperator.ChangeBrightness();
        }
    }
}
