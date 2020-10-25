using AvaloniaTutorial.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace AvaloniaTutorial.ViewModels
{
    public class AddItemViewModel : ViewModelBase
    {
        string description;
        public string Description
        {
            get => description;
            set => this.RaiseAndSetIfChanged(ref description, value);
        }


        public AddItemViewModel()
        {
            var okEnabled = this.WhenAnyValue(
                x => x.Description,
                x => !string.IsNullOrWhiteSpace(x));

            Ok = ReactiveCommand.Create(
                () => new TodoItem { Description = Description },
                okEnabled);
            Cancel = ReactiveCommand.Create(() => { });
        }


        public ReactiveCommand<Unit, TodoItem> Ok { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
    }
}
