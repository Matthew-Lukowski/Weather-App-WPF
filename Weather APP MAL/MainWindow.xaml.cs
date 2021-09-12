using System;
using System.Windows;
using Newtonsoft.Json;
using System.Net;
using System.ComponentModel;

namespace Weather_APP_MAL {

    public partial class MainWindow : Window {

        readonly WebClient client = new();
        string content = "";

        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e) => RunSearch();


        private void SearchTermTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(e.Key == System.Windows.Input.Key.Enter)
                RunSearch();
        }


        private void RunSearch() {
            string weatherInfoString = string.Empty;

            if (!string.IsNullOrWhiteSpace(SearchTermTextBox.Text)) {
                try {
                    content = client.DownloadString($"http://api.weatherapi.com/v1/current.json?key={Key.API_KEY}&q={SearchTermTextBox.Text}&aqi=no");
                }
                catch (Exception) {
                    TextBoxWeather.Text = "Location not found";
                    return;
                }
            } else {
                TextBoxWeather.Text = "Error: Search is empty";
                return;
            }

            LocationRoot loc = JsonConvert.DeserializeObject<LocationRoot>(content);

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(loc.Location)) {
                string name = descriptor.Name;
                object value = descriptor.GetValue(loc.Location);
                weatherInfoString += $"{name}: {value}\n";
            }

            weatherInfoString += "\n";

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(loc.CurrentTemp)) {
                string name = descriptor.Name;
                object value = descriptor.GetValue(loc.CurrentTemp);
                weatherInfoString += $"{name}: {value}\n";
            }
            weatherInfoString = weatherInfoString.Replace("_", " ");

            TextBoxWeather.Text = weatherInfoString;
        }

        private void TopBarDrag(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void Button_Minimize(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }
    }
}
