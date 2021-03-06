﻿/****************************************************************************************************************************************
 * 
 * Classe PlugAndPlaySerialDeviceViewModel
 * Auteur : S. ALVAREZ
 * Date : 07-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe View Model pour un objet AbstractPlugAndPlaySerialDevice.
 * 
 ****************************************************************************************************************************************/

using IoTUtilities.ViewModel;
using System;
using System.Threading.Tasks;

namespace IoTUtilities.Serial
{
    public class PlugAndPlaySerialDeviceViewModel : ViewModelBase
    {
        // PROPRIETES
        AbstractPlugAndPlaySerialDevice model = null; // Objet AbstractPlugAndPlaySerialDevice représentant le périphérique série à gérer

        /// <summary>
        /// Flag indiquant l'état connecté du périphérique série
        /// </summary>
        public bool IsConnected 
        {
            get
            {
                return model.IsConnected;
            } 
        }

        /// <summary>
        /// Flag indiquant l'état ouvert (en communication) du périphérique série
        /// </summary>
        public bool IsOpened
        {
            get
            {
                return model.IsOpened;
            }
        }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_model">Objet AbstractPlugAndPlaySerialDevice représentant le périphérique série à gérer</param>
        public PlugAndPlaySerialDeviceViewModel(AbstractPlugAndPlaySerialDevice a_model)
        {
            model = a_model;
            model.OnStateChanged += Model_OnStateChangedAsync;
        }

        // METHODES
        /// <summary>
        /// Gère les changement d'état du périphérique série et notifie l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="state">Etat concerné par le changement</param>
        private async Task Model_OnStateChangedAsync(object sender, SerialDeviceState state)
        {
            switch(state)
            {
                case SerialDeviceState.CONNECTED:
                    await coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        OnPropertyChanged(nameof(IsConnected));
                    });
                    break;
                case SerialDeviceState.OPENED:
                    await coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        OnPropertyChanged(nameof(IsOpened));
                    });
                    break;
            }
        }

    }
}
