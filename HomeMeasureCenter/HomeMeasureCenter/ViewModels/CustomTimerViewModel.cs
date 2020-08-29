using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMeasureCenter.Models;
using IoTUtilities.ViewModel;

namespace HomeMeasureCenter.ViewModels
{
    public class CustomTimerViewModel : ViewModelBase
    {
        // PROPRIETES
        private CustomTimer model;
        public DateTime Date { get; private set; }

        // EVENEMENTS

        // CONSTRUCTEUR
        public CustomTimerViewModel(CustomTimer a_model)
        {
            model = a_model;
            model.OnSecond += Model_OnSecond;
        }

        // METHODES
        private void Model_OnSecond(object sender, DateTime instant)
        {
            Date = instant;
            OnPropertyChanged(nameof(Date));
        }

    }
}
