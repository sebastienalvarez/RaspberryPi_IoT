/****************************************************************************************************************************************
 * 
 * Classe BMP180ViewModel
 * Auteur : S. ALVAREZ
 * Date : 27-10-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe View Model pour un objet BMP180.
 * 
 ****************************************************************************************************************************************/

using IoTUtilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTUtilities.Sensors
{
    public class BMP180ViewModel : ViewModelBase
    {
        // PROPRIETES

        private BMP180 model; // Objet BMP180 représentant le capteur de pression et de température BMP180

        /// <summary>
        /// Drapeau indiquant si la connexion avec le capteur BMP180 est réalisée ou pas
        /// </summary>
        public bool IsConnected 
        {
            get
            {
                return model.IsConnected;
            } 
        }

        /// <summary>
        /// Température en °C
        /// </summary>
        public double Temperature { get; private set; }

        /// <summary>
        /// Pression en mbar
        /// </summary>
        public double Pressure { get; private set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_model">Objet BMP180 représentant le capteur de température et de pression BMP180</param>
        /// <param name="a_modelUsedOnUIthread">Flag indiquant sur quel type de thread est utilisé l'objet model : true si thread UI (défaut), false si thread parallèle</param>
        public BMP180ViewModel(BMP180 a_model, bool a_modelUsedOnUIthread = true)
        {
            model = a_model;
            Temperature = double.NaN;
            Pressure = double.NaN;

            model.OnConnected += Model_OnConnected;
            if (a_modelUsedOnUIthread)
            {
                model.OnNewMeasurement += Model_OnNewMeasurement;
            }
            else
            {
                model.OnNewMeasurementAsync += Model_OnNewMeasurement_NotOnUIthread;
            }
        }

        // METHODES
        /// <summary>
        /// Gère la notification de connexion avec le capteur BMP180 et notifie l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="e">Non utilisé</param>
        private void Model_OnConnected(object sender, EventArgs e)
        {
            OnPropertyChanged("IsConnected");
        }

        /// <summary>
        /// Gère la réception d'une nouvelle mesure du capteur BMP180 et notifie l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="measurement">Objet BMP180Measurement avec les valeurs de température et de pression</param>
        private void Model_OnNewMeasurement(object sender, BMP180Measurement measurement)
        {
            if(measurement != null)
            {
                Temperature = measurement.Temperature;
                Pressure = measurement.Pressure;
                OnPropertyChanged(nameof(Temperature));
                OnPropertyChanged(nameof(Pressure));
            }
        }

        /// <summary>
        /// Gère la réception d'une nouvelle mesure du capteur BMP180 et notifie l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="measurement">Objet BMP180Measurement avec les valeurs de température et de pression</param>
        /// <returns></returns>
        private async Task Model_OnNewMeasurement_NotOnUIthread(object sender, BMP180Measurement measurement)
        {
            if (measurement != null)
            {
                Temperature = measurement.Temperature;
                Pressure = measurement.Pressure;
                await coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    OnPropertyChanged(nameof(Temperature));
                    OnPropertyChanged(nameof(Pressure));
                });
            }
        }

    }
}
