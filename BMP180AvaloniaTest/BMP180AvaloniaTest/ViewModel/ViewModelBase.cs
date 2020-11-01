/****************************************************************************************************************************************
 * 
 * Classe ViewModelBase
 * Auteur : S. ALVAREZ
 * Date : 31-10-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe de base implémentant l'interface INotifyPropertyChanged pour les classes View Model.
 * 
 ****************************************************************************************************************************************/

using System.ComponentModel;

namespace BMP180AvaloniaTest.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        // PROPRIETES
        // Pas de propriétés...

        // EVENEMENT
        /// <summary>
        /// Evenement pour le changement de valeur d'une propriété
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        public ViewModelBase()
        {
        }

        // METHODES
        /// <summary>
        /// Lève un évenement PropertyChanged
        /// </summary>
        /// <param name="a_propertyName">Nom de la propriété sur laquelle lever un évenement (laisser vide pour l'ensemble des propriétés de la classe)</param>
        protected virtual void OnPropertyChanged(string a_propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(a_propertyName));
        }

    }
}
