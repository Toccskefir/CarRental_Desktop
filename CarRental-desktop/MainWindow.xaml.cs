using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarRental_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Statisztika service;
        public MainWindow()
        {
            InitializeComponent();
            ReadData();
        }

        private void ReadData()
        {
            try
            {
                service = new Statisztika();
                service.LoadCars();
                dataGrid.ItemsSource = service.Cars;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            Car selected = dataGrid.SelectedItem as Car;
            if (selected == null)
            {
                MessageBox.Show("Törléshez előbb válasszon ki autót");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Biztos szeretné törölni a kiválasztott autót?",
                "Törlés", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (this.service.Delete(selected.Id))
                {
                    MessageBox.Show("Sikeres törlés");
                    ReadData();
                } 
                else
                {
                    MessageBox.Show("Hiba a törlés során");
                }
            }
        }
    }
}