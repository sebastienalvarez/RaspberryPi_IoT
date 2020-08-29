/****************************************************************************************************************************************
 * 
 * Classe DHT11ViewModel
 * Auteur : S. ALVAREZ
 * Date : 29-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe View Model pour un objet DHT11.
 * 
 ****************************************************************************************************************************************/

using IoTUtilities.ViewModel;
using System;
using System.Threading.Tasks;

namespace IoTUtilities.Sensors
{
    public class DHT11ViewModel : ViewModelBase
    {
        // PROPRIETES
        private DHT11 model; // Objet DHT11 représentant le capteur de température et d'humidité relative DHT11

        /// <summary>
        /// Température
        /// </summary>
        public double Temperature { get; private set; }

        /// <summary>
        /// Humidité relative
        /// </summary>
        public double Humidity { get; private set; }

        /// <summary>
        /// Flag indiquant la validité de la dernière mesure reçue
        /// </summary>
        public bool IsLastMeasurementSuccessfull { get; set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_model">Objet DHT11 représentant le capteur de température et d'humidité relative DHT11</param>
        /// <param name="a_modelUsedOnUIthread">Flag indiquant sur quel type de thread est utilisé l'objet model : true si thread UI (défaut), false si thread parallèle</param>
        public DHT11ViewModel(DHT11 a_model, bool a_modelUsedOnUIthread = true)
        {
            model = a_model;
            Temperature = double.NaN;
            Humidity = double.NaN;
            IsLastMeasurementSuccessfull = false;
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
        /// Gère la réception d'une nouvelle mesure du capteur DHT11 et notifie l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="measurement">Objet DHT11Measurement avec les valeurs de température et d'humidité relative</param>
        private void Model_OnNewMeasurement(object sender, DHT11Measurement measurement)
        {
            IsLastMeasurementSuccessfull = measurement != null;
            OnPropertyChanged(nameof(IsLastMeasurementSuccessfull));

            if(measurement != null)
            {
                Temperature = measurement.Temperature;
                Humidity = measurement.Humidity;
                OnPropertyChanged(nameof(Temperature));
                OnPropertyChanged(nameof(Humidity));
            }
        }

        /// <summary>
        /// Gère la réception d'une nouvelle mesure du capteur DHT11 et notifie l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="measurement">Objet DHT11Measurement avec les valeurs de température et d'humidité relative</param>
        /// <returns></returns>
        private async Task Model_OnNewMeasurement_NotOnUIthread(object sender, DHT11Measurement measurement)
        {
            IsLastMeasurementSuccessfull = measurement != null;
            await coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                OnPropertyChanged(nameof(IsLastMeasurementSuccessfull));
            });


            if (measurement != null)
            {
                Temperature = measurement.Temperature;
                Humidity = measurement.Humidity;
                await coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    OnPropertyChanged(nameof(Temperature));
                    OnPropertyChanged(nameof(Humidity));
                });
            }
        }

    }
}
