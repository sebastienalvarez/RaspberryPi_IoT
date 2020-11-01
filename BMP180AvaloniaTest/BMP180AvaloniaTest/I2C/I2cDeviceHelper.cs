/****************************************************************************************************************************************
 * 
 * Classe I2cDeviceHelper
 * Auteur : S. ALVAREZ
 * Date : 31-10-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe static permettant de chercher et de se connecter à un I2cDevice à partir de son addresse.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Device.I2c;

namespace BMP180AvaloniaTest.I2C
{
    public static class I2cDeviceHelper
    {
        public delegate bool CheckConnection(I2cDevice a_device);

        /// <summary>
        /// Recherche un périphérique I2C avec l'adresse spécifiée et renvoit un objet I2cDevice si trouvé ou null si non trouvé
        /// </summary>
        /// <param name="a_slaveAddress">Adresse du périphérique I2C</param>
        /// <returns>Objet I2cDevice si périphérique trouvé ou null si périphérique non trouvé</returns>
        public static I2cDevice GetI2cDeviceBySlaveAddress(int a_slaveAddress, CheckConnection a_checkConnection)
        {
            // Test de connexion sur le bus 1
            I2cConnectionSettings i2cSettings = new I2cConnectionSettings(1, a_slaveAddress);
            I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
            if (!a_checkConnection(i2cDevice))
            {
                // Test de connexion sur le bus 0
                i2cSettings = new I2cConnectionSettings(0, a_slaveAddress);
                i2cDevice = I2cDevice.Create(i2cSettings);
                if (!a_checkConnection(i2cDevice))
                {
                    i2cDevice = null;
                }
            }
            return i2cDevice;
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
