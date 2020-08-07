/****************************************************************************************************************************************
 * 
 * Enumération SerialDeviceState
 * Auteur : S. ALVAREZ
 * Date : 07-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Enumération pour représenter les états d'un périphérique série plug and play (connecté et ouvert).
 * 
 ****************************************************************************************************************************************/

namespace IoTUtilities.Serial
{
    public enum SerialDeviceState
    {
        CONNECTED, // Pour l'état connecté
        OPENED // Pour l'état ouvert
    }
}
