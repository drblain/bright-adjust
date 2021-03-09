using System;
using System.Windows.Forms;

namespace BrightAdjust
{
    class SysTrayAppContext : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _contextMenu;

        protected SysTrayAppContext()
        {
            Application.ApplicationExit += this.ApplicationExitHandler;

            _contextMenu = new ContextMenuStrip();
            _notifyIcon = new NotifyIcon
            {
                ContextMenuStrip = _contextMenu,
                Text = Application.ProductName, 
                Visible = true 
            };

            this._notifyIcon.MouseDoubleClick += this.IconDoubleClickHandler;
            this._notifyIcon.MouseClick += this.IconClickHandler;
        }

        protected ContextMenuStrip ContextMenu
        {
            get { return _contextMenu;  }
        }

        protected NotifyIcon TrayIcon
        {
            get { return _notifyIcon; }
        }

        protected virtual void OnTrayIconClicked(MouseEventArgs e) {}

        protected virtual void OnTrayIconDoubleClicked(MouseEventArgs e) {}

        private void IconClickHandler(object sender, MouseEventArgs e)
        {
            this.OnTrayIconClicked(e);
        }

        private void IconDoubleClickHandler(object sender, MouseEventArgs e)
        {
            this.OnTrayIconDoubleClicked(e);
        }

        protected virtual void OnApplicationExit(EventArgs e)
        {
            // dispose of notification icon
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            // dispose of the context menu
            if (_contextMenu != null)
            {
                _contextMenu.Dispose();
            }
        }

        private void ApplicationExitHandler(object sender, EventArgs e)
        {
            this.OnApplicationExit(e);
        }
    }
}
