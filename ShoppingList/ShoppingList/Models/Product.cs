/****************************************************************************************************************************************
 * 
 * Classe Product
 * Auteur : S. ALVAREZ
 * Date : 08-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe représentant un produit associé à un code barre.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShoppingList.Models
{
    public class Product
    {
        // PROPRIETES
        /// <summary>
        /// Code barre du produit
        /// </summary>
        public string BarCode { get; private set; }

        /// <summary>
        /// URL de l'image représentant le produit
        /// </summary>
        public string ImageUrl { get; private set; }

        private string name;
        /// <summary>
        /// Nom du produit
        /// </summary>
        public string Name
        {
            get { return name; }
            set 
            {
                if(value != name)
                {
                    name = value;
                    OnProductChanged?.Invoke(this, nameof(Name));
                }
            }
        }

        private uint number;
        /// <summary>
        /// Nombre de produit
        /// </summary>
        public uint Number
        {
            get { return number; }
            set 
            { 
                if(value != number)
                {
                    number = value;
                    OnProductChanged?.Invoke(this, nameof(Number));
                }
            }
        }

        // EVENEMENTS
        /// <summary>
        /// Délégué pour la gestion du changement de valeur d'une propriété du produit
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="propertyName">Nom de la propriété modifiée</param>
        /// <returns></returns>
        public delegate Task ProductChangedEventHandlerAsync(object sender, string propertyName);

        /// <summary>
        /// Evenement pour gestion du changement de valeur d'une propriété du produit, cet évenement peut notifier une éventuelle classe View model
        /// </summary>
        public event ProductChangedEventHandlerAsync OnProductChanged;

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_barCode">Code barre du produit</param>
        protected Product(string a_barCode)
        {
            BarCode = a_barCode;
        }

        // METHODES
        /// <summary>
        /// Crée un produit à partir d'un code barre
        /// </summary>
        /// <param name="a_barCode">Code barre</param>
        /// <returns>Object Product représentant le produit</returns>
        public static Product CreateProductFromBarCode(string a_barCode)
        {
            Product product = new Product(a_barCode);
            // Appel asynchrone sans attendre le retour du résultat pour la fluidité de l'IU (pas de await)
            product.LoadDataFromOpenFoodFactsAsync();
            return product;
        }

        /// <summary>
        /// Récupère les données du produit via l'API du site http://fr.openfoodfacts.org
        /// </summary>
        /// <returns></returns>
        private async Task LoadDataFromOpenFoodFactsAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"https://world.openfoodfacts.org/api/v0/product/{BarCode}.json");
            string jsonData = await response.Content.ReadAsStringAsync();            
            if (!string.IsNullOrEmpty(jsonData))
            {
                OpenFoodFacts.Root data = JsonConvert.DeserializeObject<OpenFoodFacts.Root>(jsonData);
                if(data != null && data.status == OpenFoodFacts.Root.StatusOK)
                {
                    if (string.IsNullOrEmpty(Name) && (!string.IsNullOrEmpty(data.product.product_name) || !string.IsNullOrEmpty(data.product.quantity) || !string.IsNullOrEmpty(data.product.brands)))
                    {
                        Name = $"{data.product.product_name} - {data.product.quantity} - {data.product.brands}";
                        OnProductChanged?.Invoke(this, nameof(Name));
                    }
                    ImageUrl = data.product.image_thumb_url;
                    if (!string.IsNullOrEmpty(ImageUrl))
                    {
                        OnProductChanged?.Invoke(this, nameof(ImageUrl));
                    }
                }
            }
        }

    }
}
