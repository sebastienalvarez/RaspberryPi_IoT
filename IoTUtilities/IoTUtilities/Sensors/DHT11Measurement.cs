/****************************************************************************************************************************************
 * 
 * Classe DHT11Measurement
 * Auteur : S. ALVAREZ
 * Date : 29-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe permettant de calculer les valeurs de température et d'humidité à partir des 4 octets de données fournis par un
 *         capteur DHT11.
 * 
 ****************************************************************************************************************************************/

namespace IoTUtilities.Sensors
{
    public class DHT11Measurement
    {
        // PROPRIETES
        /// <summary>
        /// Température mesurée
        /// </summary>
        public double Temperature { get; private set; }

        /// <summary>
        /// Humidité relative mesurée
        /// </summary>
        public double Humidity { get; private set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_humidityMSB">Octet de poids fort pour l'humidité relative (donne la partie entière)</param>
        /// <param name="a_humidityLSB">Octet de poids faible pour l'humidité relative (donne la partie décimale)</param>
        /// <param name="a_temperatureMSB">Octet de poids fort pour la température (donne la partie entière)</param>
        /// <param name="a_temperatureLSB">Octet de poids faible pour la température (donne la partie décimale)</param>
        public DHT11Measurement(uint a_humidityMSB, uint a_humidityLSB, uint a_temperatureMSB, uint a_temperatureLSB)
        {
            // DHT11
            Humidity = a_humidityMSB + a_humidityLSB / 100.0;
            Temperature = a_temperatureMSB + a_temperatureLSB / 100.0;
        }

        // METHODES
        // Pas de méthodes

    }
}
