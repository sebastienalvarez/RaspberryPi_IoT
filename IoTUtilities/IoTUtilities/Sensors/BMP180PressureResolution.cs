/****************************************************************************************************************************************
 * 
 * Enum BMP180PressureResolution
 * Auteur : S. ALVAREZ
 * Date : 29-10-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Enum permettant de paramétrer la résolution en mesure de la pression d'un capteur BMP180.
 *         Les résolutions sont issues de la datasheet BOSH BMP180 BST-BMP180-DS000-09.
 * 
 ****************************************************************************************************************************************/

namespace IoTUtilities.Sensors
{
    public enum BMP180PressureResolution
    {
        LOW_POWER,
        STANDARD,
        HIGH_RESOLUTION,
        ULTRA_HIGH_RESOLUTION
    }
}
