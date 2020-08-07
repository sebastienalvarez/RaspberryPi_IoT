/****************************************************************************************************************************************
 * 
 * Classe abstraite AbstractPlugAndPlaySerialDevice
 * Auteur : S. ALVAREZ
 * Date : 07-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe abstraite représentant un périphérique plug and play. Les classes dérivées doivent définir les paramètres du port série
 *         et le traitements des données reçues
 * 
 ****************************************************************************************************************************************/

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace IoTUtilities.Serial
{
    public abstract class AbstractPlugAndPlaySerialDevice
    {
        // PROPRIETES
        /// <summary>
        /// Objet SystemSerialDevices static permettant l'abonnement aux événements de connexion/déconnexion des périphériques série
        /// </summary>
        public static SystemSerialDevices SerialDevices = new SystemSerialDevices();
        
        private bool isOpeningAutomatic; // Flag indiquant si le périphérique série doit être ouvert automatiquement à la connexion
        private uint maxBytesToRead; // Nombre maximum d'octets à lire (buffer)
        private DeviceInformation deviceInformation = null; // Informations sur le périphérique série connecté/déconnecté lors de la réception d'un évenement de connexion/déconnexion
        private SerialDevice serialDevice = null; // Objet représentant le périphérique série, la classe "wrappe" cet objet
        private object serialDeviceLock = new object(); // Objet pour la synchronisation des taches parallèles (l'objet serialDevice ne doit être accédé que par une tâche parallèle à la fois)
        private CancellationTokenSource cancellationTokenSource = null; // Jeton d'annulation d'une tâche parallèle

        /// <summary>
        /// Paramètres du port série
        /// </summary>
        public SerialPortParameters Parameters { get; private set; }
        
        /// <summary>
        /// Flag indiquant si le périphérique série est connecté
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return deviceInformation != null;
            }
        }

        /// <summary>
        /// Flag indiquant si le périphérique série est ouvert
        /// </summary>
        public bool IsOpened
        {
            get
            {
                lock (serialDeviceLock)
                {
                    return serialDevice != null;
                }
            }
        }

        // EVENEMENTS
        /// <summary>
        /// Délégué pour la gestion du changement d'état du périphérique série pour la fonctionnalité Plug & Play
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="state">Etat concerné par le changement</param>
        public delegate void SerialDeviceStateEventHandler(object sender, SerialDeviceState state);

        /// <summary>
        /// Evenement pour gestion du changement d'état du périphérique série pour la fonctionnalité Plug & Play
        /// </summary>
        public event SerialDeviceStateEventHandler OnStateChanged;

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_parameters">Paramètres du port série</param>
        /// <param name="a_isOpeningAutomatic">Flag indiquant si le périphérique série doit être ouvert automatiquement à la connexion</param>
        /// <param name="a_maxBytesToRead">Nombre maximum d'octets à lire (buffer)</param>
        public AbstractPlugAndPlaySerialDevice(SerialPortParameters a_parameters, bool a_isOpeningAutomatic, uint a_maxBytesToRead)
        {
            Parameters = a_parameters;
            isOpeningAutomatic = a_isOpeningAutomatic;
            maxBytesToRead = a_maxBytesToRead;

            // Abonnement aux événements de connexion/déconnexion : c'est cet abonnement qui offre la fonctionnalité de Plug & Play
            SerialDevices.HandleSerialDeviceEvents(this);
        }

        // METHODES
        /// <summary>
        /// Traite un événement de connexion : si l'identifiant du périphérique correspond, récupération des informations du périphérique, 
        /// le périphérique série est alors dans l'état connecté et un événement de changement d'état est levé vers la classe utilisatrice, 
        /// la communication est établie si isOpeningAutomatic est à true
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="args">Informations sur le périphérique série connecté</param>
        public void AddSerialDevice(DeviceWatcher sender, DeviceInformation args)
        {
            if (!IsConnected && args.Id.StartsWith(Parameters.Id))
            {
                deviceInformation = args;
                OnStateChanged?.Invoke(this, SerialDeviceState.CONNECTED);
                if (isOpeningAutomatic)
                {
                    OpenAsync();
                }
            }
        }

        /// <summary>
        /// Traite un événement de déconnexion : si l'identifiant du périphérique correspond, récupération des informations du périphérique, 
        /// libération des ressources, le périphérique série est alors dans l'état déconnecté et un événement de changement d'état est levé vers 
        /// la classe utilisatrice
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="args">Informations sur le périphérique série déconnecté</param>
        public void RemoveSerialDevice(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            if (IsConnected && args.Id == deviceInformation.Id)
            {
                Close();
                deviceInformation = null;
                OnStateChanged?.Invoke(this, SerialDeviceState.CONNECTED);
            }
        }

        /// <summary>
        /// Ouvre la communication du périphérique série via une lecture en continue des données et lève un événement changement d'état vers 
        /// la classe utilisatrice 
        /// </summary>
        private async void OpenAsync()
        {
            Debug.WriteLine($"Trying to open of serial device: {deviceInformation.Name} with Id: {deviceInformation.Id}");
            try
            {
                // Récupération du périphérique série par son identifiant
                SerialDevice temporarySerialDevice = await SerialDevice.FromIdAsync(deviceInformation.Id);
                if (temporarySerialDevice != null)
                {
                    // Synchronisation des tâches parallèles : une seule tâche peut accéder au périphérique série
                    lock (serialDeviceLock)
                    {
                        serialDevice = temporarySerialDevice;
                    }
                    Debug.WriteLine($"Successful opening of serial device: {deviceInformation.Name} with Id: {deviceInformation.Id}");
                    serialDevice.BaudRate = Parameters.BaudRate;
                    serialDevice.Parity = Parameters.Parity;
                    serialDevice.StopBits = Parameters.StopBit;
                    serialDevice.DataBits = Parameters.DataBits;
                    serialDevice.Handshake = Parameters.Handshake;
                    serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(Parameters.ReadingDuration);
                    serialDevice.WriteTimeout = TimeSpan.FromMilliseconds(Parameters.WritingDuration);
                    OnStateChanged?.Invoke(this, SerialDeviceState.OPENED);
                    if (Parameters.ReadingDuration > 0)
                    {
                        ReadInContinueAsync();
                    }
                }
                else
                {
                    Debug.WriteLine($"Failed to open serial device: {deviceInformation.Name} with Id: {deviceInformation.Id}");
                }
            }
            catch (Exception ex)
            {
                lock (serialDeviceLock)
                {
                    serialDevice = null;
                }
                Debug.WriteLine($"Error during opening of serial device: {deviceInformation.Name} with Id: {deviceInformation.Id}. Error is {ex.Message} (type: {ex.GetType()})");
            }
        }

        /// <summary>
        /// Lit en continue les données du périphérique série
        /// </summary>
        private async void ReadInContinueAsync()
        {
            Debug.WriteLine($"Reading of serial device: {deviceInformation.Name} with Id: {deviceInformation.Id}");
            cancellationTokenSource = new CancellationTokenSource();
            DataReader dataReader = new DataReader(serialDevice.InputStream);
            dataReader.InputStreamOptions = InputStreamOptions.Partial;
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                uint readBytesNumber = 0;
                try
                {
                    readBytesNumber = await dataReader.LoadAsync(maxBytesToRead).AsTask(cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during reading of serial device: {deviceInformation.Name} with Id: {deviceInformation.Id}. Error is {ex.Message} (type: {ex.GetType()})");
                }
                if (readBytesNumber > 0)
                {
                    ProcessData(dataReader);
                }
            }
        }

        /// <summary>
        /// Traite les données lues (méthode à définir dans les classes dérivées dépendant de la nature du périphérique série)
        /// </summary>
        /// <param name="dataReader">Objet DataReader avec les données lues</param>
        protected abstract void ProcessData(DataReader dataReader);

        /// <summary>
        /// Ferme la communication avec le périphérique série et lève un événement changement d'état vers la classe utilisatrice
        /// </summary>
        private void Close()
        {
            Debug.WriteLine($"Closing serial device: {deviceInformation.Name} with Id: {deviceInformation.Id}");
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }
            Dispose();
            OnStateChanged?.Invoke(this, SerialDeviceState.OPENED);
        }

        /// <summary>
        /// Libère proprement les ressources du périphérique série pour lequel fermer la communication
        /// </summary>
        protected virtual void Dispose()
        {
            lock (serialDeviceLock)
            {
                serialDevice.Dispose();
            }
            serialDevice = null;
        }

        /// <summary>
        /// Envoie les octets passés en argument au périphérique série
        /// </summary>
        /// <param name="a_bytes">Octets à envoyer</param>
        /// <returns>Flag indiquant le succès de l'opération</returns>
        protected async Task<bool> SendDataAsync(byte[] a_bytes)
        {
            bool isSuccessful = false;
            if (IsOpened && a_bytes.Length > 0)
            {
                DataWriter dataWriter = new DataWriter(serialDevice.OutputStream);
                try
                {
                    dataWriter.WriteBytes(a_bytes);
                    isSuccessful = await dataWriter.StoreAsync() == a_bytes.Length;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during writing of serial device: {deviceInformation.Name} with Id: {deviceInformation.Id}. Error is {ex.Message} (type: {ex.GetType()})");
                }
                finally
                {
                    dataWriter.DetachStream();
                }
            }
            return isSuccessful;
        }

    }
}
