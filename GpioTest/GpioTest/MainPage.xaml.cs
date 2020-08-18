using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;


namespace GpioTest
{
    public sealed partial class MainPage : Page
    {
        // PROPRIETES
        private GpioController gpc;
        private int pinNumber = 21; // Broche GPIO n°21 utilisée
        private GpioPin pin;
        private bool isDeviceInitializedCorrectly = false;

        // CONSTRUCTEUR
        public MainPage()
        {
            this.InitializeComponent();
        }

        // METHODES
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialisation du GpioController
            gpc = GpioController.GetDefault();
            // Affichage message d'erreur si aucun controller GPIO a été trouvé
            if(gpc == null)
            {
                TextBlockMessage.Text = "Pas de controller GPIO sur cette plateforme Windows 10";
            }
            // Controller GPIO trouvé : OK
            else
            {
                TextBlockMessage.Text = "Controller GPIO initialisé";
                // Activation de l'électronique de la broche GPIO souhaitée
                pin = gpc.OpenPin(pinNumber);
                // Affichage message d'erreur si la pin souhaitée n'existe pas sur la plateforme
                if (pin == null)
                {
                    TextBlockMessage.Text = $"La broche GPIO n°{pinNumber} n'existe pas sur cette plateforme Windows 10";
                }
                // Broche GPIO activée : OK
                else
                {
                    TextBlockMessage.Text = $"Broche GPIO n°{pinNumber} activée";
                    pin.SetDriveMode(GpioPinDriveMode.Output);
                    isDeviceInitializedCorrectly = true;
                }
            }
            ButtonOn.IsEnabled = isDeviceInitializedCorrectly;
            ButtonOff.IsEnabled = false;
        }

        private void ButtonOn_Click(object sender, RoutedEventArgs e)
        {
            pin.Write(GpioPinValue.High);
            ButtonOn.IsEnabled = false;
            ButtonOff.IsEnabled = true;
        }

        private void ButtonOff_Click(object sender, RoutedEventArgs e)
        {
            pin.Write(GpioPinValue.Low);
            ButtonOn.IsEnabled = true;
            ButtonOff.IsEnabled = false;
        }
    }
}