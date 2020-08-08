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
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ShoppingList.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        // PROPRIETES
        private Product model = null;

        public string BarCode 
        {
            get
            {
                return model.BarCode;
            } 
        }

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

        public uint Number 
        {
            get
            {
                return model.Number;
            }
        }

        private BitmapImage thumbnailImage;
        public BitmapImage ThumbnailImage 
        {
            get
            {
                return thumbnailImage;
            } 
        }

        // EVENEMENTS

        // CONSTRUCTEUR
        public ProductViewModel(Product a_model)
        {
            model = a_model;
            model.OnProductChanged += Model_OnProductChangedAsync;
        }


        // METHODES
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
                        break;
                    default:
                        break;
                }
            });
        }

    }
}
