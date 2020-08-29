/****************************************************************************************************************************************
 * 
 * Classe DHT11
 * Auteur : S. ALVAREZ
 * Date : 29-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe permettant de représenter un capteur de température et d'humidité relative DHT11.
 * 
 ****************************************************************************************************************************************/

using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace IoTUtilities.Sensors
{
    public class DHT11
    {
        // PROPRIETES
        /// <summary>
        /// Broche de connexion pour la patte du bus de données du capteur DHT11
        /// </summary>
        public GpioPin Pin { get; private set; }

        // EVENEMENTS
        /// <summary>
        /// Délégué pour la gestion d'une nouvelle mesure d'un capteur DHT11
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="measurement">Objet DHT11Measurement avec les valeurs de température et d'humidité relative</param>
        public delegate void DHT11NewMeasurement(object sender, DHT11Measurement measurement);

        /// <summary>
        /// Délégué pour la gestion asynchrone d'une nouvelle mesure d'un capteur DHT11
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="measurement">Objet DHT11Measurement avec les valeurs de température et d'humidité relative</param>
        public delegate Task DHT11NewMeasurementAsync(object sender, DHT11Measurement measurement);

        /// <summary>
        /// Evenement pour la gestion d'une nouvelle mesure d'un capteur DHT11 
        /// </summary>
        public event DHT11NewMeasurement OnNewMeasurement;

        /// <summary>
        /// Evenement pour la gestion asynchrone d'une nouvelle mesure d'un capteur DHT11 
        /// </summary>
        public event DHT11NewMeasurementAsync OnNewMeasurementAsync;

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_pin">Broche de connexion pour la patte du bus de données du capteur DHT11</param>
        public DHT11(GpioPin a_pin)
        {
            Pin = a_pin;
        }

        // METHODES
        /// <summary>
        /// Effectue une demande de lecture du capteur DHT11
        /// </summary>
        /// <param name="a_maxTestNumber">Nombre maximal d'essai de lecture pour obtenir des résultats de mesure valides</param>
        /// <returns>Objet DHT11Measurement avec les mesures de température et d'humidité relative ou null si un résultat n'a pas pu être obtenu</returns>
        public DHT11Measurement Read(uint a_maxTestNumber)
        {
            DHT11Measurement measurement = null;
            if(Pin != null)
            {
                uint i = 0;
                while(i < a_maxTestNumber && measurement == null)
                {
                    DHT11BusIO rawData = DHT11BusIO.ReadData(Pin);
                    if(rawData.CurrentFallingEdgeIndex >= DHT11BusIO.FALLING_EDGE_MAX_NUMBER)
                    {
                        measurement = ConvertTimeLengthToBits(rawData);
                    }
                    i++;
                }
            }
            if(measurement != null)
            {
                OnNewMeasurement?.Invoke(this, measurement);
                OnNewMeasurementAsync?.Invoke(this, measurement);
            }
            return measurement;
        }

        /// <summary>
        /// Interprète les mesures de durées d'impulsion des données brutes en valeurs d'octets
        /// </summary>
        /// <param name="a_rawData">Données brutes incluant les mesures de durées d'impulsion</param>
        /// <returns>Objet DHT11Measurement avec les mesures de température et d'humidité relative ou null si un résultat n'a pas pu être obtenu</returns>
        private static DHT11Measurement ConvertTimeLengthToBits(DHT11BusIO a_rawData)
        {
            uint humidityMSB = ReadByte(a_rawData, 0);
            uint humidityLSB = ReadByte(a_rawData, 1);
            uint temperatureMSB = ReadByte(a_rawData, 2);
            uint temperatureLSB = ReadByte(a_rawData, 3);
            uint check = ReadByte(a_rawData, 4);

            if (((humidityMSB + humidityLSB + temperatureMSB + temperatureLSB) & 0xff) != check)
            {
                return null;
            }
            else
            {
                return new DHT11Measurement(humidityMSB, humidityLSB, temperatureMSB, temperatureLSB);
            }
        }

        /// <summary>
        /// Lit chacune des impulsions correspondant à un bit de données pour un des 5 octets des données brutes (40 bits = 5 octets)
        /// </summary>
        /// <param name="a_rawData">Données brutes incluant les mesures de durées d'impulsion</param>
        /// <param name="a_byteNumber">Numéro de l'octet spécifié</param>
        /// <returns>Valeur de l'octet spécifié</returns>
        private static byte ReadByte(DHT11BusIO a_rawData, byte a_byteNumber)
        {
            byte result = 0;
            for(int i = a_byteNumber * 8; i < (a_byteNumber + 1) * 8; i++)
            {
                result <<= 1;
                result += ReadBit(a_rawData, i);
            }
            return result;
        }

        /// <summary>
        /// Interprète la valeur d'un bit en fonction de la durée de l'impulsion correspondante
        /// </summary>
        /// <param name="a_rawData">Données brutes incluant les mesures de durées d'impulsion</param>
        /// <param name="a_bitNumber">Numéro du bit de l'octet (de 1 à 40)</param>
        /// <returns>Valeur du bit spécifié</returns>
        private static byte ReadBit(DHT11BusIO a_rawData, int a_bitNumber)
        {
            return a_rawData.FallingEdgeIntervalValues[a_bitNumber + 1] - a_rawData.FallingEdgeIntervalValues[a_bitNumber] > DHT11BusIO.BIT_THRESHOLD ? (byte)1 : (byte)0;
        }

    }
}
