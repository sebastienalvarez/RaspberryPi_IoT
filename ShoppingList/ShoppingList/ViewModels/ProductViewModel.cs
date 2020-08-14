/****************************************************************************************************************************************
 * 
 * Classe ProductViewModel
 * Auteur : S. ALVAREZ
 * Date : 08-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe View Model pour l'interface IU d'une donnée Product.
 * 
 ****************************************************************************************************************************************/

using IoTUtilities.ViewModel;
using IoTUtilities.ViewModel.Commands;
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace ShoppingList.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        // PROPRIETES
        private Product model = null; // Objet Product

        /// <summary>
        /// Code barre du produit
        /// </summary>
        public string BarCode 
        {
            get
            {
                return model.BarCode;
            } 
        }

        /// <summary>
        /// Nom du produit
        /// </summary>
        public string Name 
        {
            get
            {
                return model.Name;
            }
            set
            {
                model.Name = value;
            }
        }

        /// <summary>
        /// Quantité de produits
        /// </summary>
        public uint Number 
        {
            get
            {
                return model.Number;
            }
        }

        private BitmapImage thumbnailImage;
        /// <summary>
        /// Image miniature du produit
        /// </summary>
        public BitmapImage ThumbnailImage 
        {
            get
            {
                return thumbnailImage;
            } 
        }

        public bool IsVisible 
        { 
            get { return ProductList.Instance.IsFiltered ? Number > 0 : true; } 
        }

        public ICommand AddOneProduct { get; private set; }
        public ICommand RemoveOneProduct { get; private set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_model">Objet Product</param>
        public ProductViewModel(Product a_model)
        {
            model = a_model;
            model.OnProductChanged += Model_OnProductChangedAsync;

            AddOneProduct = new CommandBase((parameter) => {
                model.Number++;
            });
            RemoveOneProduct = new CommandBase((parameter) => {
                if(model.Number > 0)
                {
                    model.Number--;
                }
            });
        }

        // METHODES
        /// <summary>
        /// Lève les évenements de notification à l'IU
        /// </summary>
        /// <param name="sender">Objet ayant levé l'événement</param>
        /// <param name="propertyName">Nom de la propriété modifiée</param>
        /// <returns></returns>
        private async Task Model_OnProductChangedAsync(object sender, string propertyName)
        {
            await coreDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                switch (propertyName)
                {
                    case nameof(Product.Name):
                        OnPropertyChanged(nameof(Name));
                        break;
                    case nameof(Product.ImageUrl):
                        if (!string.IsNullOrEmpty(model.ImageUrl))
                        {
                            thumbnailImage = new BitmapImage(new Uri(model.ImageUrl));
                        }
                        else if(thumbnailImage == null)
                        {
                            thumbnailImage = new BitmapImage();
                        }
                        OnPropertyChanged(nameof(ThumbnailImage));
                        break;
                    case nameof(Product.Number):
                        OnPropertyChanged(nameof(Number));
                        OnPropertyChanged(nameof(IsVisible));
                        break;
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        internal void UpdateVisibility()
        {
            OnPropertyChanged(nameof(IsVisible));
        }

    }
}
