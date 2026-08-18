using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace AltEye.Services
{
    internal class FilePickerService
    {
        public FilePickerService(Func<IntPtr> getWindowHandle)
        {
            this._getWindowCallback = getWindowHandle;
        }

        private readonly Func<IntPtr> _getWindowCallback;

        public async Task<StorageFile> PickOpenFileAsync(PickerLocationId? pickerLocationId = null, string fileType = ".pcbdoc")
        {
            var openPicker = new FileOpenPicker();
            InitializeWithWindow.Initialize(openPicker, this._getWindowCallback());

            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;            
            openPicker.FileTypeFilter.Add(fileType);

            return await openPicker.PickSingleFileAsync();
        }
    }
}
