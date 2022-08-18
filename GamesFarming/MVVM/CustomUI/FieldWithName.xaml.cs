using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace GamesFarming.MVVM.CustomUI
{
    /// <summary>
    /// Логика взаимодействия для FieldWithName.xaml
    /// </summary>
    public partial class FieldWithName : UserControl, INotifyPropertyChanged
    {

        public FieldWithName()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string Input
        {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty InputProperty =
            DependencyProperty.Register("Input", typeof(string),
                typeof(FieldWithName),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnInputPropertyChanged)));

        public static readonly RoutedEvent InputChangedEvent = EventManager.RegisterRoutedEvent("InputChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<string>),
            typeof(FieldWithName));
        public event RoutedPropertyChangedEventHandler<string> InputChanged
        {
            add { AddHandler(InputChangedEvent, value); }
            remove { RemoveHandler(InputChangedEvent, value); }
        }

        private static void OnInputPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FieldWithName)
            {
                var d = sender as FieldWithName;
                d.RaiseEvent(new RoutedPropertyChangedEventArgs<string>((string)e.OldValue, (string)e.NewValue, InputChangedEvent));
            }

        }

        public string Sign
        {
            get { return (string)GetValue(SignProperty); }
            set { SetValue(SignProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty SignProperty =
            DependencyProperty.Register("Sign", typeof(string),
                typeof(FieldWithName),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSignPropertyChanged)));

        public static readonly RoutedEvent SignChangedEvent = EventManager.RegisterRoutedEvent("SignChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<string>),
            typeof(FieldWithName));
        public event RoutedPropertyChangedEventHandler<string> SignChanged
        {
            add { AddHandler(SignChangedEvent, value); }
            remove { RemoveHandler(SignChangedEvent, value); }
        }

        private static void OnSignPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FieldWithName)
            {
                var d = sender as FieldWithName;
                d.RaiseEvent(new RoutedPropertyChangedEventArgs<string>((string)e.OldValue, (string)e.NewValue, SignChangedEvent));
            }

        }
    }
}
