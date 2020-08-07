/****************************************************************************************************************************************
 * 
 * Classe SerialPortParameters
 * Auteur : S. ALVAREZ
 * Date : 07-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe représentant les paramètres d'un port série.
 * 
 ****************************************************************************************************************************************/

using Windows.Devices.SerialCommunication;

namespace IoTUtilities.Serial
{
    public class SerialPortParameters
    {
        // PROPRIETES
        /// <summary>
        /// Nom du périphérique série
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Identifiant du périphérique série
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Vitesse de lecture/écriture des données en bit/s
        /// </summary>
        public uint BaudRate { get; private set; }

        /// <summary>
        /// Bit de parité pour le contrôle des erreurs
        /// </summary>
        public SerialParity Parity { get; private set; }

        /// <summary>
        /// Nombre standard de bit d'arrêt par octet
        /// </summary>
        public SerialStopBitCount StopBit { get; private set; }

        /// <summary>
        /// Nombre de bits de données pour chaque caractère échangé (n'inclut pas les bits de parité et d'arrêt)
        /// </summary>
        public ushort DataBits { get; private set; }

        /// <summary>
        /// Protocole de communication pour le contrôle de flux
        /// </summary>
        public SerialHandshake Handshake { get; private set; }

        /// <summary>
        /// Délai de lecture avant interruption en millisecondes, valeur à mettre à 0 si il n'y que des données en écriture
        /// </summary>
        public double ReadingDuration { get; private set; }

        /// <summary>
        /// Délai décriture avant interruption en millisecondes, valeur à mettre à 0 si il n'y que des données en lecture
        /// </summary>
        public double WritingDuration { get; private set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_name">Nom du périphérique série</param>
        /// <param name="a_id">Identifiant du périphérique série</param>
        /// <param name="a_baudRate">Vitesse de lecture/écriture des données en bit/s</param>
        /// <param name="a_parity">Bit de parité pour le contrôle des erreurs</param>
        /// <param name="a_stopBit">Nombre standard de bit d'arrêt par octet</param>
        /// <param name="a_dataBits">Nombre de bits de données pour chaque caractère échangé (n'inclut pas les bits de parité et d'arrêt)</param>
        /// <param name="a_handshake">Protocole de communication pour le contrôle de flux</param>
        /// <param name="a_readingDuration">Délai de lecture avant interruption en millisecondes, valeur à mettre à 0 si il n'y que des données en écriture</param>
        /// <param name="a_writingDuration">Délai décriture avant interruption en millisecondes, valeur à mettre à 0 si il n'y que des données en lecture</param>
        public SerialPortParameters(string a_name, string a_id, uint a_baudRate, SerialParity a_parity, SerialStopBitCount a_stopBit,
                                    ushort a_dataBits, SerialHandshake a_handshake, double a_readingDuration, double a_writingDuration)
        {
            Name = a_name;
            Id = a_id;
            BaudRate = a_baudRate;
            Parity = a_parity;
            StopBit = a_stopBit;
            DataBits = a_dataBits;
            Handshake = a_handshake;
            ReadingDuration = a_readingDuration;
            WritingDuration = a_writingDuration;
        }

        // METHODES
        // Pas de méthodes

    }
}
