using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoPOD.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePicker : ContentView
    {
        public ObservableCollection<ImageSource> pickerSource { get; set; }

        public static readonly BindableProperty ButtonTextProperty =
            BindableProperty.Create("ButtonText", typeof(string), typeof(ImageButton), default(string));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public event EventHandler Clicked;

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ImageButton), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(ImageButton), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create("Source", typeof(ImageSource), typeof(ImageButton), default(ImageSource));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public ImagePicker()
        {
            InitializeComponent();

            imagePicker.SetBinding(Image.SourceProperty, new Binding("Source", source: this));

            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    Clicked?.Invoke(this, EventArgs.Empty);

                    if (Command != null)
                    {
                        if (Command.CanExecute(CommandParameter))
                            Command.Execute(CommandParameter);
                    }
                })
            });
        }

        public ImagePicker(ObservableCollection<ImageSource> source)
        {
            InitializeComponent();

            imagePicker.SetBinding(Image.SourceProperty, new Binding("Source", source: this));

            
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stack = this.Parent as StackLayout;

            stack.Children.Remove(this);

            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    Clicked?.Invoke(this, EventArgs.Empty);

                    if (Command != null)
                    {
                        if (Command.CanExecute(CommandParameter))
                            Command.Execute(CommandParameter);
                    }
                })
            });
        }
    }
}