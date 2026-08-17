using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OriginalCircuit.Altium.Models.Pcb;
using OriginalCircuit.Altium.Serialization.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltEye.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial PcbDocument Document { get; set; }

        [RelayCommand]
        private async Task Load()
        {
            var reader = new PcbDocReader();
            //var filePath = "d:\\code\\cs\\Test\\Altium\\data\\simple-project\\Simple_Project.PcbDoc";
            var filePath = "d:\\code\\cs\\Test\\Altium\\data\\simple-project\\GrunMasterBox.PcbDoc";
            this.Document = await reader.ReadAsync(filePath);
        }
    }
}
