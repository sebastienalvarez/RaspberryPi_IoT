/****************************************************************************************************************************************
 * 
 * Classe I2cDeviceHelper
 * Auteur : S. ALVAREZ
 * Date : 27-10-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe static permettant de chercher et de se connecter à un I2cDevice à partir de son addresse.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace IoTUtilities.I2C
{
    public static class I2cDeviceHelper
    {
        /// <summary>
        /// Recherche un périphérique I2C avec l'adresse spécifiée et renvoit un objet I2cDevice si trouvé ou null si non trouvé
        /// </summary>
        /// <param name="a_slaveAddress">Adresse du périphérique I2C</param>
        /// <returns>Objet I2cDevice si périphérique trouvé ou null si périphérique non trouvé</returns>
        public static async Task<I2cDevice> GetI2cDeviceBySlaveAddressAsync(int a_slaveAddress)
        {
            // Récupération des périphériques I2C connectés sur la plateforme
            string advancedQuerySyntaxString = I2cDevice.GetDeviceSelector();
            DeviceInformationCollection deviceInformationCollection = await DeviceInformation.FindAllAsync(advancedQuerySyntaxString);
            if (deviceInformationCollection != null && deviceInformationCollection.Count > 0)
            {
                // Paramétrage de la connection
                I2cConnectionSettings i2cConnectionSettings = new I2cConnectionSettings(a_slaveAddress);
                // Test de chaque périphérique connecté sur la plateforme
                foreach (var deviceInformation in deviceInformationCollection)
                {
                    string deviceId = deviceInformationCollection[0].Id;
                    I2cDevice i2cDevice = await I2cDevice.FromIdAsync(deviceId, i2cConnectionSettings);
                    if (i2cDevice != null)
                    {
                        return i2cDevice;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Lit 2 octets à l'adresse spécifié sur le périphérique I2C
        /// </summary>
        /// <param name="a_i2dDevice">périphérique I2C</param>
        /// <param name="register">Adresse du registre à lire</param>
        /// <returns>Valeur stockée dans le registre spécifié de type short (2 octets)</returns>
        public static short ReadShort(I2cDevice a_i2dDevice, byte register)
        {
            short value = 0;
            try
            {
                byte[] buffer = new byte[2];
                a_i2dDevice.WriteRead(new byte[] { register }, buffer);
                // Vérification de la communication les 2 octets ne doivent pas être = 0x0 ou 0xff
                if(!((buffer[0] == 0x0 && buffer[1] == 0x0) || (buffer[0] == 0xff && buffer[1] == 0xff)))
                {
                    value = BitConverter.ToInt16(new byte[] { buffer[1], buffer[0] }, 0);
                }
            }
            catch (Exception)
            {
            }
            return value;
        }

    }
}
