using AltEye.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using OriginalCircuit.Altium.Models.Pcb;
using OriginalCircuit.Altium.Serialization.Readers;
using System;
using System.Threading.Tasks;

namespace AltEye.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial PcbDocument Document { get; set; }

        [ObservableProperty]
        public partial bool IsProgressVisible { get; set; }

        [RelayCommand]
        private async Task Load()
        {
            var reader = new PcbDocReader();
            var filePicker = Ioc.Default.GetService<FilePickerService>();
            var fileStorage = await filePicker.PickOpenFileAsync();

            if (fileStorage?.IsAvailable == true)
            {
                this.IsProgressVisible = true;
                this.Document = await reader.ReadAsync(fileStorage.Path);                
                this.IsProgressVisible = false;
            }
        }
    }
}
