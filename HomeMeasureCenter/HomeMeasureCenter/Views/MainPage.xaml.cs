using HomeMeasureCenter.Models;
using HomeMeasureCenter.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IoTUtilities.Sensors;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HomeMeasureCenter.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CustomTimer customTimer;
        private int dht11PinNumber = 21;
        private DHT11 dht11 = null;
        private bool isMeasurementInProgress = false;
        private object isMeasurementInProgressLock = new object();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            InitTimer();
            InitDHT11();
            if (dht11 != null)
            {
                MessageTextBlock.Text = "Démarrage des mesures du capteur DHT11...";
                App.log.LogEvent("Démarrage des mesures du capteur DHT11...", null, Windows.Foundation.Diagnostics.LoggingLevel.Verbose);
                ReadDHT11Data();
            }
        }

        private void InitTimer()
        {
            customTimer = new CustomTimer();
            TimeTextBlock.DataContext = new CustomTimerViewModel(customTimer);
            customTimer.OnMinute += CustomTimer_OnMinute;
        }

        private void CustomTimer_OnMinute(object sender, DateTime instant)
        {
            if (dht11 != null)
            {
                ReadDHT11Data();
            }
        }

        private void InitDHT11()
        {
            GpioController gpioController = GpioController.GetDefault();
            GpioPin dht11Pin = null;
            if (gpioController == null)
            {
                MessageTextBlock.Text = "Pas de GPIO sur cette plateforme...";
                App.log.LogEvent("Pas de GPIO sur cette plateforme...", null, Windows.Foundation.Diagnostics.LoggingLevel.Warning);
            }
            else
            {
                MessageTextBlock.Text = "GPIO trouvé sur la plateforme...";
                GpioOpenStatus status;
                if (!gpioController.TryOpenPin(dht11PinNumber, GpioSharingMode.Exclusive, out dht11Pin, out status))
                {
                    MessageTextBlock.Text = $"Pas d'accès à la broche {dht11PinNumber} : {status.ToString()}";
                    App.log.LogEvent($"Pas d'accès à la broche {dht11PinNumber} : {status.ToString()}", null, Windows.Foundation.Diagnostics.LoggingLevel.Warning);
                }
            }

            if (dht11Pin != null)
            {
                dht11 = new DHT11(dht11Pin);
                MessageTextBlock.Text = $"Broche {dht11PinNumber} initialisée...";
                App.log.LogEvent($"Broche {dht11PinNumber} initialisée...", null, Windows.Foundation.Diagnostics.LoggingLevel.Verbose);
                DHTStackPanel.DataContext = new DHT11ViewModel(dht11);
            }
        }

        private void ReadDHT11Data()
        {
            lock (isMeasurementInProgressLock)
            {
                if (isMeasurementInProgress)
                {
                    return;
                }
                isMeasurementInProgress = true;
            }
            try
            {
                DHT11Measurement measure = dht11.Read(60);
                if (measure != null)
                {
                    MessageTextBlock.Text = $"Dernière mesure reçue à {DateTime.Now.ToString("HH:mm:ss")}";
                    App.log.LogEvent($"Dernière mesure reçue à {DateTime.Now.ToString("HH:mm:ss")}", null, Windows.Foundation.Diagnostics.LoggingLevel.Verbose);
                }
                else
                {
                    App.log.LogEvent($"Echec de la mesure à {DateTime.Now.ToString("HH:mm:ss")}", null, Windows.Foundation.Diagnostics.LoggingLevel.Warning);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                lock (isMeasurementInProgressLock)
                {
                    isMeasurementInProgress = false;
                }
            }
        }

    }
}
