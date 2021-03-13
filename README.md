# bright-adjust

## 📝 description

This is a simple, lightweight application for automatically choosing the screen brightness on Windows laptops without adaptive light sensors. The app operates entirely from the system tray and is meant to run in the background to be used as needed.

## 💽 installation

Within `install/` is a setup wizard that can be used to install a current working version of the project. After installing, there will be a shortcut to the application on the Desktop.

You can also open the repository as a visual studio project and create your own setup project from there after modifying the source.

## 🕹️ usage

Once the application is running, its functionality can be accessed through either double-clicking the icon that appears in the system tray or by selecting the **Adjust Brightness** option in the icon's context menu. This will cause the application to read a single image from the device's default video capture device. From this image, the average ambient luma is calculated and subsequently used to set the system's brightness using Windows Management.

## 💳 license

This project uses the GNU GPL v3 license.
