/****************************************************************************************************************************************
 * 
 * Classe ProductList
 * Auteur : S. ALVAREZ
 * Date : 08-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe représentant une liste de produits.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace ShoppingList.Models
{
    public class ProductList : Collection<Product>
    {
        // PROPRIETES
        private const char CSV_SEPARATOR = ';';
        private static StorageFolder folder = null;
        private static readonly string FILENAME = "Shopping List";

        /// <summary>
        /// Singleton
        /// </summary>
        public static ProductList Instance = new ProductList();

        /// <summary>
        /// Flag de configuration du filtrage : true = les produit de quantité 0 sont filtrés, false = aucun filtrage
        /// </summary>
        public bool IsFiltered { get; set; }

        private bool isModified;
        /// <summary>
        /// Flag indiquant si un produit de la liste a été modifié pour la persistance des données sur disque
        /// </summary>
        public bool IsModified
        {
            get 
            {
                lock (isModifiedLock)
                {
                    return isModified;
                }
            }
            set 
            {
                lock (isModifiedLock)
                {
                    isModified = value;
                }
            }
        }

        private object isModifiedLock = new object();

        private Timer savingTimer = null;

        // EVENEMENTS
        /// <summary>
        /// Délégué pour la gestion de l'ajout d'un nouveau produit dans la liste
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="product">Nouveau produit ajouté</param>
        /// <returns></returns>
        public delegate Task ProductChangedEventHandlerAsync(object sender, Product product);

        /// <summary>
        /// Délégué pour la gestion de l'ajout d'un produit déjà existant dans la liste (incrémentation de la quantité du produit)
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="barCode">Code barre du produit existant ajouté</param>
        /// <returns></returns>
        public delegate Task BarCodeAddedEventHandlerAsync(object sender, string barCode);

        /// <summary>
        /// Evenement pour gestion de l'ajout d'un nouveau produit dans la liste, cet évenement peut notifier une éventuelle classe View model
        /// </summary>
        public event ProductChangedEventHandlerAsync ProductChanged;

        /// <summary>
        /// Evenement pour gestion de l'ajout d'un produit existant dans la liste, cet évenement peut notifier une éventuelle classe View model
        /// </summary>
        public event BarCodeAddedEventHandlerAsync BarCodeAdded;

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        protected ProductList()
        {
            savingTimer = new Timer(SavingTicks, null, new TimeSpan(0, 0, 30), new TimeSpan(0, 0, 10));
            IsFiltered = true;
        }

        // METHODES
        /// <summary>
        /// Ajoute un nouveau produit correspondant au code barre à la liste ou incrémente la quantité du produit correspondant au code barre,
        /// lève les évenements de notification
        /// </summary>
        /// <param name="a_barCode">Code barre du produit à ajouter</param>
        public void AddBarCode(string a_barCode)
        {
            Product product = Items.Where(p => p.BarCode == a_barCode).FirstOrDefault();
            if(product == null)
            {
                product = Product.CreateProductFromBarCode(a_barCode);
                Items.Add(product);
                ProductChanged?.Invoke(this, product);
            }
            product.Number++;
            BarCodeAdded?.Invoke(this, a_barCode);
        }

        /// <summary>
        /// Ajoute un nouveau produit à la liste et lève un évenement de notification
        /// </summary>
        /// <param name="index">Index dans la liste</param>
        /// <param name="item">Produit à ajouter</param>
        protected override void InsertItem(int index, Product item)
        {
            base.InsertItem(index, item);
            ProductChanged?.Invoke(this, item);
        }

        /// <summary>
        /// Déplace un item dans la collection
        /// </summary>
        /// <param name="a_formerIndex">Ancien index</param>
        /// <param name="a_newIndex">Nouveau index</param>
        public void ChangePosition(int a_formerIndex, int a_newIndex)
        {
            lock (isModifiedLock)
            {
                Product productToMove = Items[a_formerIndex];
                RemoveAt(a_formerIndex);
                base.InsertItem(a_newIndex, productToMove);
                isModified = true;
            }
        }

        /// <summary>
        /// Efface la liste des produits
        /// </summary>
        public void ClearList()
        {
            foreach(Product product in Items)
            {
                product.Number = 0;
            }
        }

        /// <summary>
        /// Exécute la persistance des données des produits sur le disque
        /// </summary>
        /// <param name="state">Etat (non utilisé)</param>
        private void SavingTicks(object state)
        {
            List<string> products = null;

            // Récupération des produits de la liste si et seulement si il y a eu une modification
            lock (isModifiedLock)
            {
                if (isModified)
                {
                    products = GetProductsAsCsv();
                    isModified = false;
                }
            }

            // Exécution de la persistance des données si modification
            if(products != null && products.Count > 0)
            {
                SaveAsCsvAsync(products);
            }
        }

        /// <summary>
        /// Récupère les données des produits de la liste et génère une liste de string au format csv
        /// </summary>
        /// <returns>Liste de string au format csv</returns>
        private List<string> GetProductsAsCsv()
        {
            List<string> products = new List<string>();
            products.Add("Code barre" + CSV_SEPARATOR + "Quantité" + CSV_SEPARATOR + "Nom");
            foreach(Product product in Items)
            {
                products.Add(product.BarCode + CSV_SEPARATOR + product.Number + CSV_SEPARATOR + product.Name);
            }
            return products;
        }

        /// <summary>
        /// Sauvegarde les données des produits au format csv sur disque
        /// </summary>
        /// <param name="products"></param>
        private async Task SaveAsCsvAsync(List<string> products)
        {
            StorageFolder appFolder = await GetAppFolder();
            string fileName = FILENAME + ".csv";
            IStorageItem storageItem = await appFolder.TryGetItemAsync(fileName);

            StorageFile file;
            if(storageItem == null)
            {
                file = await appFolder.CreateFileAsync(fileName);
            }
            else
            {
                file = (StorageFile)storageItem;
            }
            await FileIO.WriteLinesAsync(file, products);
            Debug.WriteLine("Liste des produits enregistrée");
        }

        /// <summary>
        /// Récupère le dossier de l'application (celui-ci ne nécessite pas d'inscription au manifest)
        /// </summary>
        /// <returns>Dossier sous forme d'un objet StorageFolder</returns>
        private async static Task<StorageFolder> GetAppFolder()
        {
            if(folder == null)
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(FILENAME, CreationCollisionOption.OpenIfExists);
            }
            return folder;
        }

        /// <summary>
        /// Récupère les données des produits à partir de données au format csv
        /// </summary>
        /// <returns></returns>
        public async Task LoadCsvFileAsync()
        {
            IList<string> products = await LoadProductFromCsvData();
            if(products != null && products.Count> 0)
            {
                products.RemoveAt(0);
                foreach(string product in products)
                {
                    Add(Product.CreateFromCsvData(product, CSV_SEPARATOR));
                }
            }
        }

        /// <summary>
        /// Récupère les données au format csv des produits
        /// </summary>
        /// <returns>Collection des données au format csv des produits</returns>
        private async Task<IList<string>> LoadProductFromCsvData()
        {
            StorageFolder appFolder = await GetAppFolder();
            string fileName = FILENAME + ".csv";
            IStorageItem storageItem = await appFolder.TryGetItemAsync(fileName);
            if(storageItem != null)
            {
                return await FileIO.ReadLinesAsync((StorageFile)storageItem);
            }
            return null;
        }

    }
}
