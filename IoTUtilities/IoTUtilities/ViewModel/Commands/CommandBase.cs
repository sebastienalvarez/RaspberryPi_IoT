/****************************************************************************************************************************************
 * 
 * Classe CommandBase
 * Auteur : S. ALVAREZ
 * Date : 12-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe de base implémentant l'interface ICommand. Elle peut être utilisée pour ces commandes basiques.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Windows.Input;

namespace IoTUtilities.ViewModel.Commands
{
    public class CommandBase : ICommand
    {
        // PROPRIETES
        protected Action<object> commandLogic; // Delegate avec la logique de la commande 

        // EVENEMENTS
        /// <summary>
        /// Evenement pour demander si la commande est active ou pas
        /// </summary>
        public event EventHandler CanExecuteChanged;

        // CONSTRUCTEUR
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="a_commandLogic">Méthode avec la logique de la commande</param>
        public CommandBase(Action<object> a_commandLogic)
        {
            commandLogic = a_commandLogic;
        }

        // METHODES
        /// <summary>
        /// Réponse à l'évenement CanExecuteChanged, indique si la commande est active ou pas
        /// </summary>
        /// <param name="parameter">Objet pour le paramètre de la commande</param>
        /// <returns>Flag indiquant si la commande est active ou pas</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Execute la commabde
        /// </summary>
        /// <param name="parameter">Objet pour le paramètre de la commande</param>
        public void Execute(object parameter)
        {
            commandLogic(parameter);
        }

    }
}
