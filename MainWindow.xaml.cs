using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Data;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using CurrencyConverterAPI;

namespace CurrencyConverter_Static
{
    public partial class MainWindow : Window
    {
        Root val = new Root();

        public MainWindow()
        {
            InitializeComponent();
            ClearControls();
            GetValue();
        }

        public static async Task<Root> GetData<T>(string url)
        {
            var myRoot = new Root();

            try
            {
                using(var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);       // how long the task will wait for data from http
                    HttpResponseMessage response = await client.GetAsync(url);

                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ResponseString = await response.Content.ReadAsStringAsync();
                        var ResponseObject = JsonConvert.DeserializeObject<Root>(ResponseString);

                        //  MessageBox.Show("Timestamp: " + ResponseObject.timestamp, "Information", MessageBoxButton.OK,MessageBoxImage.Information);

                        return ResponseObject;
                    }
                }
                return myRoot;
            }
            catch
            {
                return myRoot;
            }
        }

        private async void GetValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=57099f2013064e0c8c62cae48ab1f48b");
            BindCurrency();
        }

        private void BindCurrency()

        {

            DataTable dtCurrency = new DataTable();

            dtCurrency.Columns.Add("Text");

            dtCurrency.Columns.Add("Value");

            dtCurrency.Rows.Add("--SELECT--", 0);
            dtCurrency.Rows.Add("INR", val.rates.INR);
            dtCurrency.Rows.Add("USD", val.rates.USD);
            dtCurrency.Rows.Add("NZD", val.rates.NZD);
            dtCurrency.Rows.Add("JPY", val.rates.JPY);
            dtCurrency.Rows.Add("EUR", val.rates.EUR);
            dtCurrency.Rows.Add("CAD", val.rates.CAD);
            dtCurrency.Rows.Add("ISK", val.rates.ISK);
            dtCurrency.Rows.Add("PHP", val.rates.PHP);
            dtCurrency.Rows.Add("DKK", val.rates.DKK);
            dtCurrency.Rows.Add("CZK", val.rates.DKK);

            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;

            cmbFromCurrency.DisplayMemberPath = "Text";

            cmbFromCurrency.SelectedValuePath = "Value";

            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Value";
            cmbToCurrency.SelectedIndex = 0;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            double ConvertedValue;

            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus();
                return;
            }
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                cmbToCurrency.Focus();
                return;
            }

            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                ConvertedValue = double.Parse(txtCurrency.Text);

                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbFromCurrency.SelectedValue.ToString());

                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
                cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}