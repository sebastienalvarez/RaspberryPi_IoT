/****************************************************************************************************************************************
 * 
 * Classe Root
 * Auteur : S. ALVAREZ
 * Date : 08-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe permettant la serialization json des données d'un produit issues de http://fr.openfoodfacts.org/api/v0/{BarCode}.json.
 *         Seules les données intéressantes pour le projet ont été conservées.
 * 
 ****************************************************************************************************************************************/

namespace ShoppingList.Models.OpenFoodFacts
{
    public class Root
    {
        public const string StatusVerboseOk = "product found";
        public const int StatusOK = 1;

        /// <summary>
        /// Code barre
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Statut du produit : si il est trouvé, la valeur est égale à product found
        /// </summary>
        public string status_verbose { get; set; }

        /// <summary>
        /// Objet Product contenant les informations sur le produit (désignation, quantité, score nutrition, images...)
        /// </summary>
        public Product product { get; set; }

        /// <summary>
        /// Statut du produit : si il est trouvé, la valeur est égale à 1
        /// </summary>
        public int status { get; set; }

    }
}
