/****************************************************************************************************************************************
 * 
 * Classe Product
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
    public class Product
    {
        /// <summary>
        /// Code barre du produit
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Nom du produit
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// Nom générique du produit (à utiliser si product_name est vide) 
        /// </summary>
        public string generic_name { get; set; }

        /// <summary>
        /// Quantité avec unité
        /// </summary>
        public string quantity { get; set; }

        /// <summary>
        /// Quantité sans unité
        /// </summary>
        public int product_quantity { get; set; }

        /// <summary>
        /// Marque du produit
        /// </summary>
        public string brands { get; set; }

        /// <summary>
        /// Pays d'origine du produit
        /// </summary>
        public string origins { get; set; }

        /// <summary>
        /// Liste des ingrédients contenus dans le produit
        /// </summary>
        public string ingredients_text { get; set; }

        /// <summary>
        /// Score nutrition français du produit
        /// </summary>
        public string nutrition_grade_fr { get; set; }

        /// <summary>
        /// Score nutrition américain du produit (à utiliser si nutrition_grade_fr est vide)
        /// </summary>
        public string nutrition_grades { get; set; }

        /// <summary>
        /// Informations sur la nutrition du produit
        /// </summary>
        public NutriscoreData nutriscore_data { get; set; }

        /// <summary>
        /// Quantité de référence pour les informations sur la nutrition du produit
        /// </summary>
        public string nutrition_data_per { get; set; }

        /// <summary>
        /// URL de l'image du produit (grande taille)
        /// </summary>
        public string image_url { get; set; }

        /// <summary>
        /// URL de l'image du produit (moyenne taille)
        /// </summary>
        public string image_small_url { get; set; }

        /// <summary>
        /// URL de l'image du produit (petite taille)
        /// </summary>
        public string image_thumb_url { get; set; }

        /// <summary>
        /// URL de l'image du détail des ingrédients (grande taille)
        /// </summary>
        public string image_ingredients_url { get; set; }

        /// <summary>
        /// URL de l'image du détail des ingrédients (moyenne taille)
        /// </summary>
        public string image_ingredients_small_url { get; set; }

        /// <summary>
        /// URL de l'image du détail des ingrédients (petite taille)
        /// </summary>
        public string image_ingredients_thumb_url { get; set; }

        /// <summary>
        /// URL de l'image des informations de nutrition (grande taille)
        /// </summary>
        public string image_nutrition_url { get; set; }

        /// <summary>
        /// URL de l'image des informations de nutrition (moyenne taille)
        /// </summary>
        public string image_nutrition_small_url { get; set; }

        /// <summary>
        /// URL de l'image des informations de nutrition (petite taille)
        /// </summary>
        public string image_nutrition_thumb_url { get; set; }

    }
}
