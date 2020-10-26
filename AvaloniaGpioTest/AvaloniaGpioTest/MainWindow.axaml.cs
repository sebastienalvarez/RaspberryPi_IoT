using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Device.Gpio;

namespace AvaloniaGpioTest
{
    public class MainWindow : Window
    {
        // PROPRIETES
        private Button buttonOn;
        private Button buttonOff;
        private TextBlock textBlockMessage;
        private GpioController gpc;
        private int pinNumber = 21; // Broche GPIO n°21 utilisée
        private bool isDeviceInitializedCorrectly = false;

        // CONSTRUCTEUR
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            buttonOn = this.FindControl<Button>("ButtonOn");
            buttonOn.Click += ButtonOn_Click;
            buttonOff = this.FindControl<Button>("ButtonOff");
            buttonOff.Click += ButtonOff_Click;
            textBlockMessage = this.FindControl<TextBlock>("TextBlockMessage");
            this.Opened += MainWindow_Initialized;
        }

        private void MainWindow_Initialized(object sender, System.EventArgs e)
        {
            string message = "Pas de controller GPIO sur cette plateforme";
            try
            {
                // Initialisation du GpioController
                gpc = new GpioController();
                if(gpc != null)
                {
                    // Controller GPIO trouvé : OK
                    message = "Controller GPIO initialisé";
                    // Activation de l'électronique de la broche GPIO souhaitée
                    try
                    {
                        gpc.OpenPin(pinNumber, PinMode.Output);
                        gpc.Write(pinNumber, PinValue.Low);
                        // Broche GPIO activée : OK
                        message = $"Controller GPIO initialisé : broche GPIO n°{pinNumber} activée";
                        isDeviceInitializedCorrectly = true;
                    }
                    catch (System.Exception)
                    {
                        message = $"Controller GPIO initialisé : broche GPIO n°{pinNumber} non existante sur cette plateforme";
                    }
                }
            }
            catch (System.Exception)
            {
                // Pas d'action nécessaire
            }
            // Affichage message avec le résultat de l'initialisation
            textBlockMessage.Text = message;
            buttonOn.IsEnabled = isDeviceInitializedCorrectly;
            buttonOff.IsEnabled = false;
        }

        private void ButtonOn_Click(object sender, RoutedEventArgs e)
        {
            gpc.Write(pinNumber, PinValue.High);
            buttonOn.IsEnabled = false;
            buttonOff.IsEnabled = true;
        }

        private void ButtonOff_Click(object sender, RoutedEventArgs e)
        {
            gpc.Write(pinNumber, PinValue.Low);
            buttonOn.IsEnabled = true;
            buttonOff.IsEnabled = false;
        }

    }
}
