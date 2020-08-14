/****************************************************************************************************************************************
 * 
 * Classe ProductListViewModel
 * Auteur : S. ALVAREZ
 * Date : 09-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe View Model pour l'interface IU des données d'une ProductList.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IoTUtilities.ViewModel;
using IoTUtilities.ViewModel.Commands;
using ShoppingList.Models;

namespace ShoppingList.ViewModels
{
    public class ProductListViewModel : ViewModelBase
    {
        // PROPRIETES
        private ProductList model = null; // Liste des produits
        private ProductViewModel productViewModelToMove; // Objet ProductViewModel déplacé sur l'IU
        private int indexProductViewModelToMove; // Index de l'objet ProductViewModel déplacé sur l'IU

        private ObservableCollection<ProductViewModel> productViewModelList = new ObservableCollection<ProductViewModel>();
        /// <summary>
        /// Collection d'objet ProductViewModel, chaque item de la liste des produits est mappé par un item de cette collection
        /// </summary>
        public ObservableCollection<ProductViewModel> ProductViewModelList
        {
            get { return productViewModelList; }
        }

        /// <summary>
        /// Objet ProductViewModel correspondant au produit courant
        /// </summary>
        public ProductViewModel SelectedProduct { get; set; }

        /// <summary>
        /// Flag de configuration du filtrage : true = les produit de quantité 0 sont filtrés, false = aucun filtrage
        /// </summary>
        public bool IsFiltered 
        {
            get { return model.IsFiltered; } 
            set 
            {
                model.IsFiltered = value;
                // Mise à jour de l'affichage des produits de la liste
                UpdateProductList();
            }
        }

        public ICommand ClearCommand { get; private set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_model">Liste des produits</param>
        public ProductListViewModel(ProductList a_model)
        {
            model = a_model;
            model.ProductChanged += Model_ProductChanged;
            model.BarCodeAdded += Model_BarCodeAdded;
            ClearCommand = new CommandBase((parameter) => {
                model.ClearList();
            });

            productViewModelList.CollectionChanged += ProductViewModelList_CollectionChanged;
        }

        // METHODES
        /// <summary>
        /// Lève l'évenement de notification d'ajout d'un produit à l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="barCode">Code barre du produit existant ajouté</param>
        /// <returns></returns>
        private async Task Model_BarCodeAdded(object sender, string barCode)
        {
            SelectedProduct = productViewModelList.First(p => p.BarCode == barCode);
            await coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                OnPropertyChanged(nameof(SelectedProduct));
            });
        }

        /// <summary>
        /// Lève l'évenement de notification de modification de la quantité d'un produit existant à l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="product">Nouveau produit ajouté</param>
        /// <returns></returns>
        private async Task Model_ProductChanged(object sender, Product product)
        {
            productViewModelList.Add(new ProductViewModel(product));
            await coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                OnPropertyChanged(nameof(ProductViewModelList));
            });
        }

        /// <summary>
        /// Met à jour l'affichage des produits de la liste dans l'IU
        /// </summary>
        private void UpdateProductList()
        {
            foreach(var productViewModel in productViewModelList)
            {
                productViewModel.UpdateVisibility();
            }
        }

        /// <summary>
        /// Récupère les données de changement de position d'un item
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement (non utilisé)</param>
        /// <param name="e">Données de changement de position d'un item</param>
        private void ProductViewModelList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems.Count == 1)
            {
                productViewModelToMove = (ProductViewModel)e.OldItems[0];
                indexProductViewModelToMove = e.OldStartingIndex;
            }
            else if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count == 1 && e.NewItems[0] == productViewModelToMove)
            {
                model.ChangePosition(indexProductViewModelToMove, e.NewStartingIndex);
                productViewModelToMove = null;
            }
        }

    }
}
