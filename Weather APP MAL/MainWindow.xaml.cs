using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;

namespace Weather_APP_MAL {

    public partial class MainWindow : Window {

        readonly HttpClient client = new();

        public MainWindow() => InitializeComponent();

        private void Button_Close(object sender, RoutedEventArgs e) => Close();

        private void Button_Minimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void Button_Search(object sender, RoutedEventArgs e) => RunSearch();

        private void TopBarDrag(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }

        private void Search_Box_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter)
                RunSearch();
        }

        private void RunSearch() {
            string weatherInfoString = string.Empty, content;

            if (!string.IsNullOrWhiteSpace(SearchBox.Text)) {
                try {
                    content = client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Key.API_KEY}&q={SearchBox.Text}&aqi=no").Result;
                }
                catch (Exception) {
                    TextBoxWeather.Text = "Location not found";
                    return;
                }
            } else {
                TextBoxWeather.Text = "Error: Search is empty";
                return;
            }

            WeatherRequestObject loc = JsonConvert.DeserializeObject<WeatherRequestObject>(content);

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(loc.Location))
                weatherInfoString += $"{descriptor.Name}: {descriptor.GetValue(loc.Location)}\n";

            weatherInfoString += "\n";

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(loc.CurrentTemp))
                weatherInfoString += $"{descriptor.Name}: {descriptor.GetValue(loc.CurrentTemp)}\n";

            TextBoxWeather.Text = weatherInfoString.Replace("_", " ");
        }

    }
}
