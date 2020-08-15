using ShoppingList.Models;
using ShoppingList.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShoppingList.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ProductListViewModel vm = new ProductListViewModel(ProductList.Instance);
            ProductListControl.DataContext = vm;
            FilterButton.DataContext = vm;
            ClearButton.DataContext = vm;
            await ProductList.Instance.LoadCsvFileAsync();

            // Temporaire : en attendant le lecteur code barre, si fichier vide création de 3 code barre de test
            if(ProductList.Instance.Count == 0)
            {
                string barCodeTest = "3178530402728";
                ProductList.Instance.AddBarCode(barCodeTest);
                barCodeTest = "3394215961520";
                ProductList.Instance.AddBarCode(barCodeTest);
                barCodeTest = "3017620420078";
                ProductList.Instance.AddBarCode(barCodeTest);
            }
        }

        private void ProductListControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            lv.ScrollIntoView(e.AddedItems.FirstOrDefault());
        }
    }
}
