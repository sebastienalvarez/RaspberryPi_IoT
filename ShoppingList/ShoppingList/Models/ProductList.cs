﻿/****************************************************************************************************************************************
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Models
{
    public class ProductList : Collection<Product>
    {
        // PROPRIETES
        /// <summary>
        /// Singleton
        /// </summary>
        public static ProductList Instance = new ProductList();

        /// <summary>
        /// Flag de configuration du filtrage : true = les produit de quantité 0 sont filtrés, false = aucun filtrage
        /// </summary>
        public bool IsFiltered { get; set; }

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
            Product productToMove = Items[a_formerIndex];
            RemoveAt(a_formerIndex);
            base.InsertItem(a_newIndex, productToMove);
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

    }
}
