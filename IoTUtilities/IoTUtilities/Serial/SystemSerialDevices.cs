/****************************************************************************************************************************************
 * 
 * Classe SystemSerialDevices
 * Auteur : S. ALVAREZ
 * Date : 07-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe permettant la surveillance des ports série du système et offrant le lien pour la fonctionnalité de Plug & Play.
 * 
 ****************************************************************************************************************************************/

using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;

namespace IoTUtilities.Serial
{
    public class SystemSerialDevices
    {
        // PROPRIETES
        /// <summary>
        /// Objet surveillant les ports série du système et détectant la connexion ou la deconnexion des périphériques série 
        /// </summary>
        protected DeviceWatcher deviceWatcher = null;

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        public SystemSerialDevices()
        {
            deviceWatcher = DeviceInformation.CreateWatcher(SerialDevice.GetDeviceSelector());
        }

        // METHODES
        /// <summary>
        /// Démarre la surveillance des ports série du système
        /// </summary>
        public void StartWatching()
        {
            deviceWatcher.Start();
        }

        /// <summary>
        /// Arrête la surveillance des ports série du système
        /// </summary>
        public void StopWatching()
        {
            deviceWatcher.Stop();
        }

        /// <summary>
        /// Abonne un objet AbstractPlugAndPlaySerialDevice aux évenements Added et Removed de connexion/déconnexion d'un périphérque
        /// série afin d'offrir la fonctionnalité de Plug and Play
        /// </summary>
        /// <param name="a_serialDevice">Objet AbstractPlugAndPlaySerialDevice à abonner pour offrir la fonctionnalité de Plug and Play</param>
        public void HandleSerialDeviceEvents(AbstractPlugAndPlaySerialDevice a_serialDevice)
        {
            deviceWatcher.Added += a_serialDevice.AddSerialDevice;
            deviceWatcher.Removed += a_serialDevice.RemoveSerialDevice;
        }

    }
}
