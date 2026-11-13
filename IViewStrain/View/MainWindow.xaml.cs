using System.Windows;
using IViewStrain.Repository.Managers;

namespace IViewStrain.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GlobalManager _globalManager;
        
        public MainWindow()
        {
            InitializeComponent();

            _globalManager = new(this);
        }
    }
}