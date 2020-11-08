using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using System;
using System.Windows.Input;

namespace AvaloniaCustomThemes
{
    public class MainWindow : Window
    {
        // PROPRIETES
        private Button themeButton;
        ICommand themeCommand;

        IStyle[] switchStyles = new StyleInclude[]
        {
            new StyleInclude(new Uri("avares://Citrus.Avalonia/Citrus.xaml")) { Source = new Uri("avares://Citrus.Avalonia/Citrus.xaml") },
            new StyleInclude(new Uri("avares://Citrus.Avalonia/Sea.xaml")) { Source = new Uri("avares://Citrus.Avalonia/Sea.xaml") },
            new StyleInclude(new Uri("avares://Citrus.Avalonia/Rust.xaml")) { Source = new Uri("avares://Citrus.Avalonia/Rust.xaml") },
            new StyleInclude(new Uri("avares://Citrus.Avalonia/Candy.xaml")) { Source = new Uri("avares://Citrus.Avalonia/Candy.xaml") },
            new StyleInclude(new Uri("avares://Citrus.Avalonia/Magma.xaml")) { Source = new Uri("avares://Citrus.Avalonia/Magma.xaml") }
        };
        int currentStyle;

        // CONSTRUCTEUR
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            themeButton = this.FindControl<Button>("ThemeButton");
            currentStyle = 0;
            //this.Styles.Insert(0, switchStyles[currentStyle]);

            themeCommand = new ActionCommand((parameter) => {
                if (currentStyle < switchStyles.Length - 1)
                {
                    currentStyle++;
                }
                else
                {
                    currentStyle = 0;
                }
                this.Styles[0] = switchStyles[currentStyle];
            });

            themeButton.Command = themeCommand;
        }
    }
}
