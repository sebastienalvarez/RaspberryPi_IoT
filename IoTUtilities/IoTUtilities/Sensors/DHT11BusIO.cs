/****************************************************************************************************************************************
 * 
 * Classe DHT11BusIO
 * Auteur : S. ALVAREZ
 * Date : 29-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe permettant de communiquer avec le bus d'un capteur de température et d'humidité DHT11. La classe fournit les durées
 *         des impulsions mesurées pendant l'acquisition, celles-ci nécessitent d'être interprétées pour déterminer la valeur des bits.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Diagnostics;
using Windows.Devices.Gpio;

namespace IoTUtilities.Sensors
{
    public class DHT11BusIO
    {
        // PROPRIETES
        protected long t1max = 0; // Valeur temporelle (en tick) pour l'étape 1 de comande
        protected long t2max = 0; // Valeur temporelle (en tick) pour l'étape 2 d'attente de la réponse du capteur
        protected long t3max = 0; // Valeur temporelle (en tick) pour l'étape 3 d'aquisition des données brutes
        protected static readonly long l1max = ConvertMicroSecondsToTicks(20000); // Valeur temporelle absolue (en tick) pour l'étape 1 de comande
        protected static readonly long l2max = ConvertMicroSecondsToTicks(1000); // Valeur temporelle absolue (en tick) pour l'étape 2 d'attente de la réponse du capteur
        protected static readonly long l3max = ConvertMicroSecondsToTicks(10000); // Valeur temporelle absolue (en tick) pour l'étape 3 d'aquisition des données brutes
        
        /// <summary>
        /// Nombre de fronts descendants à détecter pour une acquisition complète, cela correspond aux 40 bits de données passés le 1er front descendant 
        /// </summary>
        public static readonly int FALLING_EDGE_MAX_NUMBER = 41;

        /// <summary>
        /// Tableau des données brutes des longueurs en temps des impulsions (non interprétées donc)
        /// </summary>
        public long[] FallingEdgeIntervalValues { get; private set; } = new long[FALLING_EDGE_MAX_NUMBER];

        /// <summary>
        /// Index du front descendant courant
        /// </summary>
        public int CurrentFallingEdgeIndex { get; private set; }

        /// <summary>
        /// Seuil pour l'interpretation de la valeur du bit d'une impulsion : si la longueur en temps est < à ce seuil c'est un 0
        /// </summary>
        public static readonly long BIT_THRESHOLD = ConvertMicroSecondsToTicks(110);

        // CONSTRUCTEUR
        // Constructeur par défaut

        // METHODES
        /// <summary>
        /// Effectue une demande de lecture des données sur la broche passée en argument et récupère les données brutes sous forme d'un tableau de longueur en temps des impulsions (non interprétées donc)
        /// </summary>
        /// <param name="a_pin">Broche du bus du capteur DHT11</param>
        /// <returns>Objet DHT11BusIO avec le tableau FallingEdgeIntervalValues intégrant les données brutes des longueurs en temps des impulsions</returns>
        public static DHT11BusIO ReadData(GpioPin a_pin)
        {
            DHT11BusIO result = new DHT11BusIO();
            GpioPinValue currentValue;
            GpioPinValue previousValue;

            // Etape 1 - Commande de la lecture de données : la broche est placé au niveau bas pendant l1max (20ms), la datasheet indique état bas pendant au moins 18ms
            a_pin.SetDriveMode(GpioPinDriveMode.Output);
            a_pin.Write(GpioPinValue.Low);
            result.t1max = Stopwatch.GetTimestamp() + l1max;
            while(Stopwatch.GetTimestamp() < result.t1max)
            {
            }
            a_pin.SetDriveMode(GpioPinDriveMode.Input);

            // Etape 2 - Attente de l'envoi des données, un front montant est cherché dans la milliseconde, la datasheet indique 120 microsecondes max pour la détection du front montant
            previousValue = a_pin.Read();
            result.t2max = Stopwatch.GetTimestamp() + l2max;
            do
            {
                currentValue = a_pin.Read();
                if (previousValue != currentValue && currentValue == GpioPinValue.High)
                {
                    break;
                }
                previousValue = currentValue;
                if(Stopwatch.GetTimestamp() > result.t2max)
                {
                    return result;
                }
            } while (true);

            // Etape 3 - Acquisition des données, recherche des fronts descendants en 10ms, la datasheet indique 4930 microsecondes max
            result.t3max = Stopwatch.GetTimestamp() + l3max;
            do
            {
                currentValue = a_pin.Read();
                // Détection d'un front descendant
                if (previousValue == GpioPinValue.High && currentValue == GpioPinValue.Low)
                {
                    result.FallingEdgeIntervalValues[result.CurrentFallingEdgeIndex++] = Stopwatch.GetTimestamp();
                }
                previousValue = currentValue;
            } while (result.CurrentFallingEdgeIndex < FALLING_EDGE_MAX_NUMBER && Stopwatch.GetTimestamp() < result.t3max);

            return result;
        }

        /// <summary>
        /// Convertit une valeur en microsecondes en une valeur de tick dépendant de la fréquence de Stopwatch propre à la machine
        /// </summary>
        /// <param name="a_microSeconds">Valeur en microsecondes à convertir</param>
        /// <returns>Valeur de tick</returns>
        protected static long ConvertMicroSecondsToTicks(double a_microSeconds)
        {
            return Convert.ToInt64(a_microSeconds * Stopwatch.Frequency / 1e6);
        }

    }
}
