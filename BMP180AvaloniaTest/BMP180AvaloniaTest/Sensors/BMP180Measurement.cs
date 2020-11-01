/****************************************************************************************************************************************
 * 
 * Classe BMP180Measurement
 * Auteur : S. ALVAREZ
 * Date : 31-10-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe permettant de représenter les mesures d'un capteur BMP180.
 * 
 ****************************************************************************************************************************************/

namespace BMP180AvaloniaTest.Sensors
{
    public class BMP180Measurement
    {
        // PROPRIETES
        /// <summary>
        /// Température mesurée
        /// </summary>
        public double Temperature { get; private set; }

        /// <summary>
        /// Pression mesurée
        /// </summary>
        public double Pressure { get; private set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_temperature">Température mesurée</param>
        /// <param name="a_pressure">Pressure</param>
        public BMP180Measurement(double a_temperature, double a_pressure)
        {
            Temperature = a_temperature;
            Pressure = a_pressure;
        }

        // METHODES
        // Pas de méthodes

    }
}
