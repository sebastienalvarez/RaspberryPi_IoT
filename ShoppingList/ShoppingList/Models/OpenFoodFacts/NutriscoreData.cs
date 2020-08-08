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
    public class NutriscoreData
    {
        public double is_water { get; set; }
        public double proteins { get; set; }
        public double fruits_vegetables_nuts_colza_walnut_olive_oils { get; set; }
        public double positive_points { get; set; }
        public string grade { get; set; }
        public double saturated_fat_ratio { get; set; }
        public double saturated_fat_points { get; set; }
        public double proteins_value { get; set; }
        public double saturated_fat_ratio_points { get; set; }
        public double saturated_fat_value { get; set; }
        public double sugars { get; set; }
        public double saturated_fat { get; set; }
        public double saturated_fat_ratio_value { get; set; }
        public double is_fat { get; set; }
        public double negative_points { get; set; }
        public double fruits_vegetables_nuts_colza_walnut_olive_oils_value { get; set; }
        public double proteins_points { get; set; }
        public double is_beverage { get; set; }
        public double fiber_points { get; set; }
        public double sodium { get; set; }
        public double energy_value { get; set; }
        public double sugars_value { get; set; }
        public double sodium_value { get; set; }
        public double energy_points { get; set; }
        public double is_cheese { get; set; }
        public double energy { get; set; }
        public double fiber_value { get; set; }
        public double sugars_points { get; set; }
        public double fiber { get; set; }
        public double sodium_points { get; set; }
        public double score { get; set; }
        public double fruits_vegetables_nuts_colza_walnut_olive_oils_points { get; set; }
    }
}
