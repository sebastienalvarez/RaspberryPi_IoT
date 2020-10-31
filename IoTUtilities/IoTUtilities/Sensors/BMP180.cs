/****************************************************************************************************************************************
 * 
 * Classe BMP180
 * Auteur : S. ALVAREZ
 * Date : 31-10-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe permettant de représenter un capteur BMP180.
 *         La méthodologie d'acquisition des données et de calcul des mesures est conforme à la datasheet BOSH BMP180 BST-BMP180-DS000-09.
 * 
 ****************************************************************************************************************************************/

using IoTUtilities.I2C;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace IoTUtilities.Sensors
{
    public class BMP180 : IDisposable
    {
        // PROPRIETES
        // Objet I2cDevice représentant le capteur BMP180
        private I2cDevice sensor;

        // Registres d'adresse et de valeurs pour communiquer avec le capteur BMP180 par liaison I2C
        // Seuls les registres utilisés effectivement par la classe sont définis (on en trouve pas par exemple le registre pour le reset)
        private readonly Dictionary<string, byte> registers = new Dictionary<string, byte>(19)
        {
            { "device", 0x77 }, // adresse du capteur 

            { "AC1", 0xAA }, // adresse pour la valeur de calibration AC1
            { "AC2", 0xAC }, // adresse pour la valeur de calibration AC2
            { "AC3", 0xAE }, // adresse pour la valeur de calibration AC3
            { "AC4", 0xB0 }, // adresse pour la valeur de calibration AC4
            { "AC5", 0xB2 }, // adresse pour la valeur de calibration AC5
            { "AC6", 0xB4 }, // adresse pour la valeur de calibration AC6
            { "B1", 0xB6 }, // adresse pour la valeur de calibration B1
            { "B2", 0xB8 }, // adresse pour la valeur de calibration B2
            { "MB", 0xBA }, // adresse pour la valeur de calibration MB
            { "MC", 0xBC }, // adresse pour la valeur de calibration MC
            { "MD", 0xBE }, // adresse pour la valeur de calibration MD

            { "TemperatureCommand", 0x2E }, // valeur de la commande de mesure pour la température
            { "LowPowerPressureCommand", 0x34 }, // valeur de la commande de mesure pour la pression avec une résolution low power
            { "StandardPressureCommand", 0x74 }, // valeur de la commande de mesure pour la pression avec une résolution standard
            { "HighResolutionPressureCommand", 0xB4 }, // valeur de la commande de mesure pour la pression avec une résolution high resolution
            { "UltraHighResolutionPressureCommand", 0xF4 }, // valeur de la commande de mesure pour la pression avec une résolution ultra high resolution
            { "MeasurementControl", 0xF4 }, // adresse du contrôle du type de mesure dans laquelle écrire la valeur de la commande de mesure (température ou pression)

            { "MeasurementResult", 0xF6 }, // adresse pour le résultat d'une mesure
        };

        // Paramètre de calibration du capteur BMP180
        private BMP180CalibrationData calibrationData;

        /// <summary>
        /// Drapeau indiquant si la connexion avec le capteur BMP180 est réalisée ou pas
        /// </summary>
        public bool IsConnected { get; private set; }

        // EVENEMENTS
        /// <summary>
        /// Evenement pour la notification du succès de la connexion au capteur BMP180
        /// </summary>
        public event EventHandler OnConnected;

        /// <summary>
        /// Délégué pour la gestion d'une nouvelle mesure d'un capteur BMP180
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="measurement">Objet BMP180Measurement avec les valeurs de température et de pression</param>
        public delegate void BMP180NewMeasurement(object sender, BMP180Measurement measurement);

        /// <summary>
        /// Evenement pour la gestion d'une nouvelle mesure d'un capteur BMP180
        /// </summary>
        public event BMP180NewMeasurement OnNewMeasurement;

        /// <summary>
        /// Délégué pour la gestion asynchrone d'une nouvelle mesure d'un capteur BMP180
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="measurement">Objet BMP180Measurement avec les valeurs de température et de pression</param>
        public delegate Task BMP180NewMeasurementAsync(object sender, BMP180Measurement measurement);

        /// <summary>
        /// Evenement pour la gestion asynchrone d'une nouvelle mesure d'un capteur BMP180
        /// </summary>
        public event BMP180NewMeasurementAsync OnNewMeasurementAsync;

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        public BMP180()
        {
        }

        // METHODES
        /// <summary>
        /// Initialise la connexion avec le capteur BMP180, si un capteur BMP180 a été trouvé, un évenement OnConnected est envoyé
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            IsConnected = false;
            try
            {
                sensor = await I2cDeviceHelper.GetI2cDeviceBySlaveAddressAsync(registers["device"]);

                if (sensor != null)
                {
                    IsConnected = GetCalibrateData();
                    if (IsConnected)
                    {
                        OnConnected?.Invoke(this, null);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Récupères les données de calibration de l'EEPROM du capteur BMP180
        /// </summary>
        /// <returns>Drapeau indiquant le succès de la récupération des données de calibration</returns>
        private bool GetCalibrateData()
        {
            short ac1 = I2cDeviceHelper.ReadShort(sensor, registers["AC1"]);
            short ac2 = I2cDeviceHelper.ReadShort(sensor, registers["AC2"]);
            short ac3 = I2cDeviceHelper.ReadShort(sensor, registers["AC3"]);
            ushort ac4 = (ushort)I2cDeviceHelper.ReadShort(sensor, registers["AC4"]);
            ushort ac5 = (ushort)I2cDeviceHelper.ReadShort(sensor, registers["AC5"]);
            ushort ac6 = (ushort)I2cDeviceHelper.ReadShort(sensor, registers["AC6"]);
            short b1 = I2cDeviceHelper.ReadShort(sensor, registers["B1"]);
            short b2 = I2cDeviceHelper.ReadShort(sensor, registers["B2"]);
            short mb = I2cDeviceHelper.ReadShort(sensor, registers["MB"]);
            short mc = I2cDeviceHelper.ReadShort(sensor, registers["MC"]);
            short md = I2cDeviceHelper.ReadShort(sensor, registers["MD"]);

            if (ac1 != 0 && ac2 != 0 && ac3 != 0 && ac4 != 0 && ac5 != 0 && ac6 != 0 && b1 != 0 && b2 != 0 && mb != 0 && mc != 0 && md != 0)
            {
                calibrationData = new BMP180CalibrationData(ac1, ac2, ac3, ac4, ac5, ac6, b1, b2, mb, mc, md);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Effectue une mesure de température
        /// </summary>
        /// <returns>Mesure non calibrée de la température (nécessite un traitement)</returns>
        private async Task<int> ReadUncompensatedTemperatureAsync()
        {
            // Ecriture de la valeur de la commande pour la température à l'adresse du contrôle du type de mesure
            sensor.Write(new byte[] { registers["MeasurementControl"], registers["TemperatureCommand"] });

            // Attente de 4,5 ms min
            await Task.Delay(5);

            // Lecture du registre de résultat d'une mesure
            byte[] rawMeasurement = new byte[2];
            sensor.WriteRead(new byte[] { registers["MeasurementResult"] }, rawMeasurement);
            int measurement = (rawMeasurement[0] << 8) + rawMeasurement[1];

            return measurement;
        }

        /// <summary>
        /// Effectue une mesure de pression avec la résolution spécifiée (par défaut high resolution)
        /// </summary>
        /// <param name="a_pressureResolution">Résolution de la mesure</param>
        /// <returns>Mesure non calibrée de la pression (nécessite un traitement)</returns>
        private async Task<int> ReadUncompensatedTPressureAsync(BMP180PressureResolution a_pressureResolution = BMP180PressureResolution.HIGH_RESOLUTION)
        {
            byte pressureCommand = registers["HighResolutionPressureCommand"];
            int delay = 14;

            switch (a_pressureResolution)
            {
                case BMP180PressureResolution.LOW_POWER:
                    pressureCommand = registers["LowPowerPressureCommand"];
                    delay = 5;
                    break;
                case BMP180PressureResolution.STANDARD:
                    pressureCommand = registers["StandardPressureCommand"];
                    delay = 8; 
                    break;
                case BMP180PressureResolution.HIGH_RESOLUTION:
                    pressureCommand = registers["HighResolutionPressureCommand"];
                    delay = 14; 
                    break;
                case BMP180PressureResolution.ULTRA_HIGH_RESOLUTION:
                    pressureCommand = registers["UltraHighResolutionPressureCommand"];
                    delay = 26; 
                    break;
            }

            // Ecriture de la valeur de la commande pour la pression à l'adresse du contrôle du type de mesure
            sensor.Write(new byte[] { registers["MeasurementControl"], pressureCommand });

            // Attente de de la durée min (en fonction d ela résolution souhaitée)
            await Task.Delay(delay);

            // Lecture du registre de résultat d'une mesure
            byte[] rawMeasurement = new byte[3];
            sensor.WriteRead(new byte[] { registers["MeasurementResult"] }, rawMeasurement);
            int measurement = ((rawMeasurement[0] << 16) + (rawMeasurement[1] << 8) + rawMeasurement[0]) >> (8 - (int)a_pressureResolution);

            return measurement;
        }

        /// <summary>
        /// Calcule une variable intermédiaire commune aux calculs de la température et de la pression calibrées
        /// </summary>
        /// <param name="a_ut">température mesurée non calibrée</param>
        /// <returns>Variable intermédiaire B5</returns>
        private double ComputeB5(int a_ut)
        {
            double x1 = (a_ut - calibrationData.AC6) * calibrationData.AC5 / Math.Pow(2, 15);
            double x2 = calibrationData.MC * Math.Pow(2, 11) / (x1 + calibrationData.MD);
            return x1 + x2;
        }

        /// <summary>
        /// Calcule la température calibrée en °C
        /// </summary>
        /// <param name="a_b5">Variable intermédiaire B5</param>
        /// <returns>Température calibrée en °C</returns>
        private double ComputeRealTemperature(double a_b5)
        {
            return (a_b5 + 8) / Math.Pow(2, 4) / 10;
        }

        /// <summary>
        /// Calcule la presion calibrée en mbar
        /// </summary>
        /// <param name="a_b5">Variable intermédiaire B5</param>
        /// <param name="a_up">Pression mesurée non calibrée</param>
        /// <param name="a_pressureResolution">Résolution de la mesure</param>
        /// <returns>Pression calibrée en °C</returns>
        private double ComputeRealPressure(double a_b5, int a_up, BMP180PressureResolution a_pressureResolution)
        {
            // Les calculs effectués sont strictement ceux définis dans la datasheet BOSH BMP180 BST-BMP180-DS000-09
            double x1, x2, x3, b3, b4, b6, b7, p;
            b6 = a_b5 - 4000;
            x1 = (calibrationData.B2 * (b6 * b6 / Math.Pow(2, 12))) / Math.Pow(2, 11);
            x2 = calibrationData.AC2 * b6 / Math.Pow(2, 11);
            x3 = x1 + x2;
            b3 = (((int)(calibrationData.AC1 * 4 + x3) << (int)a_pressureResolution) + 2) / 4;
            x1 = calibrationData.AC3 * b6 / Math.Pow(2, 13);
            x2 = (calibrationData.B1 * (b6 * b6 / Math.Pow(2, 12))) / Math.Pow(2, 16);
            x3 = ((x1 + x2) + 2) / 4;
            b4 = calibrationData.AC4 * (ulong)(x3 + 32768) / Math.Pow(2, 15);
            b7 = ((ulong)a_up - b3) * (50000 >> (int)a_pressureResolution);
            if(b7 < 0x80000000)
            {
                p = (b7 * 2) / b4;
            }
            else
            {
                p = (b7 / b4) * 2;
            }
            x1 = (p / Math.Pow(2, 8)) * (p / Math.Pow(2, 8));
            x1 = (x1 * 3038) / Math.Pow(2, 16);
            x2 = (-7357 * p) / Math.Pow(2, 16);
            p = p + (x1 + x2 + 3791) / 4; // La pression calculée est en Pa
            p = p / 100; // La pression est en mbar 
            return p;
        }

        /// <summary>
        /// Effectue une mesure de la température et de la pression avec la résolution spécifiée (par défaut high resolution)
        /// </summary>
        /// <param name="a_pressureResolution">Résolution de la mesure</param>
        /// <returns>Objet BMP180Measurement avec les valeurs de la température en °C et de la pression en mbar ou null si non connecté ou erreur de mesure</returns>
        public async Task<BMP180Measurement> ReadMeasurementAsync(BMP180PressureResolution a_pressureResolution = BMP180PressureResolution.HIGH_RESOLUTION)
        {
            BMP180Measurement measurement = null;
            if (IsConnected)
            {
                int ut = await ReadUncompensatedTemperatureAsync();
                int up = await ReadUncompensatedTPressureAsync(a_pressureResolution);

                double b5 = ComputeB5(ut);

                double temperature = ComputeRealTemperature(b5);
                double pressure = ComputeRealPressure(b5, up, a_pressureResolution);

                measurement = new BMP180Measurement(temperature, pressure);
                if (measurement != null)
                {
                    OnNewMeasurement?.Invoke(this, measurement);
                    OnNewMeasurementAsync?.Invoke(this, measurement);
                } 
            }
            return measurement;
        }

        /// <summary>
        /// Libère les ressources
        /// </summary>
        public void Dispose()
        {
            calibrationData = null;
            IsConnected = false;
            sensor.Dispose();
        }

    }
}
