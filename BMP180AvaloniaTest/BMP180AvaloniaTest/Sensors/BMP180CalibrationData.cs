/****************************************************************************************************************************************
 * 
 * Classe BMP180CalibrationData
 * Auteur : S. ALVAREZ
 * Date : 31-10-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe permettant de représenter les paramètres de calibration stockés dans l'EEPROM d'un capteur BMP180.
 *         Ces paramètres sont utilisés pour compenser les mesures brutes d'un capteur BMP180. Ils sont propres à chaque capteur BMP180.
 *         Les paramètres sont nommés tels que définis dans la datasheet BOSH BMP180 BST-BMP180-DS000-09.
 * 
 ****************************************************************************************************************************************/

namespace BMP180AvaloniaTest.Sensors
{
    internal class BMP180CalibrationData
    {
        // PROPRIETES
        /// <summary>
        /// Paramètre de calibration AC1
        /// </summary>
        internal short AC1 { get; set; }

        /// <summary>
        /// Paramètre de calibration AC2
        /// </summary>
        internal short AC2 { get; set; }

        /// <summary>
        /// Paramètre de calibration AC3
        /// </summary>
        internal short AC3 { get; set; }

        /// <summary>
        /// Paramètre de calibration AC4
        /// </summary>
        internal ushort AC4 { get; set; }

        /// <summary>
        /// Paramètre de calibration AC5
        /// </summary>
        internal ushort AC5 { get; set; }

        /// <summary>
        /// Paramètre de calibration AC6
        /// </summary>
        internal ushort AC6 { get; set; }

        /// <summary>
        /// Paramètre de calibration B1
        /// </summary>
        internal short B1 { get; set; }

        /// <summary>
        /// Paramètre de calibration B2
        /// </summary>
        internal short B2 { get; set; }

        /// <summary>
        /// Paramètre de calibration MB
        /// </summary>
        internal short MB { get; set; }

        /// <summary>
        /// Paramètre de calibration MC
        /// </summary>
        internal short MC { get; set; }

        /// <summary>
        /// Paramètre de calibration MD
        /// </summary>
        internal short MD { get; set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_ac1">Paramètre de calibration AC1</param>
        /// <param name="a_ac2">Paramètre de calibration AC2</param>
        /// <param name="a_ac3">Paramètre de calibration AC3</param>
        /// <param name="a_ac4">Paramètre de calibration AC4</param>
        /// <param name="a_ac5">Paramètre de calibration AC5</param>
        /// <param name="a_ac6">Paramètre de calibration AC6</param>
        /// <param name="a_b1">Paramètre de calibration B1</param>
        /// <param name="a_b2">Paramètre de calibration B2</param>
        /// <param name="a_mb">Paramètre de calibration MB</param>
        /// <param name="a_mc">Paramètre de calibration MC</param>
        /// <param name="a_md">Paramètre de calibration MD</param>
        internal BMP180CalibrationData(short a_ac1, short a_ac2, short a_ac3, ushort a_ac4, ushort a_ac5, ushort a_ac6,
                                     short a_b1, short a_b2,
                                     short a_mb, short a_mc, short a_md)
        {
            AC1 = a_ac1;
            AC2 = a_ac2;
            AC3 = a_ac3;
            AC4 = a_ac4;
            AC5 = a_ac5;
            AC6 = a_ac6;
            B1 = a_b1;
            B2 = a_b2;
            MB = a_mb;
            MC = a_mc;
            MD = a_md;
        }

        // METHODES
        // Pas de méthodes...

    }
}
