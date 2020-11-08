using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using BMP180AvaloniaTest.Sensors;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BMP180AvaloniaTest
{
    public class MainWindow : Window
    {
        // PROPRIETES
        private BMP180 sensor;
        private TextBlock messageTextBlock;
        private Button connectionButton;
        private Button measurementButton;
        ICommand connectionCommand;
        ICommand measurementCommand;

        // CONSTRUCTEUR
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        // METHODE
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            messageTextBlock = this.FindControl<TextBlock>("MessageTextBlock");
            connectionButton = this.FindControl<Button>("ConnectionButton");
            measurementButton = this.FindControl<Button>("MeasurementButton");
            DisplayMessage("Bienvenue sur l'application de test du portage .NET Core de la bibliothèque BMP180");
            this.Opened += MainWindow_Opened;

            connectionCommand = new ActionCommand((parameter) =>
            {
                if (!sensor.IsConnected)
                {
                    sensor.Connect();
                    if (sensor.IsConnected)
                    {
                        sensor.OnNewMeasurement += Sensor_OnNewMeasurement;
                        DisplayMessage("Connexion réussie au capteur BMP180 !");
                        sensor.ReadMeasurementAsync();
                    }
                    else
                    {
                        DisplayMessage("Echec de la connexion, pas de capteur BMP180 détecté...");
                    }
                }
                else
                {
                    DisplayMessage("Capteur BMP180 déjà connecté...");
                }
            });

            measurementCommand = new ActionCommand((parameter) =>
            {
                if (sensor != null && sensor.IsConnected)
                {
                    sensor.ReadMeasurementAsync();
                    DisplayMessage("Nouvelle mesure demandée");
                }
            });

            connectionButton.Command = connectionCommand;
            measurementButton.Command = measurementCommand;
        }

        private void MainWindow_Opened(object sender, System.EventArgs e)
        {
            // Initialisation du capteur BMP180
            sensor = new BMP180();
            this.DataContext = new BMP180ViewModel(sensor);
        }

        private void Sensor_OnNewMeasurement(object sender, BMP180Measurement measurement)
        {
            DisplayMessage("Mesure reçue !");
        }

        private void DisplayMessage(string a_message)
        {
            if(messageTextBlock != null)
            {
                messageTextBlock.Text = a_message;
            }
        }

        // Binding avec la méthode ne fonctionne pas, le bouton est toujours inactif ??
        private void FalseConnectionCommand(string a_parameter)
        {
            if (!string.IsNullOrEmpty(a_parameter))
            {
                if(a_parameter == "Valid")
                {
                    DisplayMessage("FAUSSE COMMANDE : CONNEXION OK");
                }
            }
            DisplayMessage("FAUSSE COMMANDE : CONNEXION KO");
        }

    }
}
