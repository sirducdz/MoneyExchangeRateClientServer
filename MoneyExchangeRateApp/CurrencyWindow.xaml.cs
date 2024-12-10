using DataLayer.ModelDTOs;
using DataLayer.Models;
using Microsoft.Win32;
using MoneyExchangeRateApp.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MoneyExchangeRateApp
{
    /// <summary>
    /// Interaction logic for CurrencyWindow.xaml
    /// </summary>
    public partial class CurrencyWindow : Window
    {
        private List<Currency> globalCurrencies;
        private List<Currency> tempCurrencies;
        public CurrencyWindow()
        {
            InitializeComponent();
            //spChangeBar.IsEnabled = false;
            spChangeBar.Visibility = Visibility.Collapsed;
            //Thread executeServerThreadHistoryList = new Thread(SendToServer); // không thực thi server ở luồng chính
            //executeServerThreadHistoryList.Start("getRateHistoryList");
            //Thread executeServerThreadCurrencyList = new Thread(SendToServer); // không thực thi server ở luồng chính
            //executeServerThreadCurrencyList.Start("getCurrenciesList");
            Task taskHistoryList = Task.Run(async () => await SendToServer("getRateHistoryList"));
            //Task taskCurrencyList = Task.Run(async () => await SendToServer("getCurrenciesList"));
            //Thread.Sleep(100);
        }
        string host = "127.0.0.1";
        int port = 9999;
        private async Task SendToServer(object requestMessageParam)
        {
            try
            {
                string requestMessage = requestMessageParam as string;
                await ConnectServer(host, port, requestMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendToServer: " + ex.Message);
            }
        }
        public void LoadForm(List<RateHistoryDTO> RateHistoryDTOList)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    lvRateHistoryDTOList.ItemsSource = RateHistoryDTOList;
                }));
            }
            catch (Exception)
            {
                MessageBox.Show("Error In LoadForm");
            }
        }
        async Task ConnectServer(String server, int port, string requestMessage)
        {
            string message, responseData;
            int bytes;
            try
            {
                // Create a TepClient
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                //string requestMessage = "getRateHistoryList";
                if (requestMessage.Equals("getRateHistoryList"))
                {
                    while (true)
                    {
                        string jsonData = string.Empty;
                        var dataBytes = Encoding.UTF8.GetBytes(requestMessage);
                        await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
                        dataBytes = new byte[256];
                        int dataCount = 0;
                        //dataCount = stream.Read(dataBytes, 0, dataBytes.Length);
                        while ((dataCount = await stream.ReadAsync(dataBytes, 0, dataBytes.Length)) != 0)
                        {
                            jsonData += System.Text.Encoding.UTF8.GetString(dataBytes, 0, dataCount);
                            if (dataCount != dataBytes.Length)
                                break;
                        }
                        //var RateHistoryDTOList = JsonSerializer.Deserialize<List<RateHistoryDTO>>(jsonData);
                        var stringList = jsonData.Split("seperated");
                        var RateHistoryDTOList = JsonSerializer.Deserialize<List<RateHistoryDTO>>(stringList[0]);
                        RateHistoryDTOGlobalList = RateHistoryDTOList;
                        //LoadForm(RateHistoryDTOList);
                        LoadForm(SearchItem(searchInput, RateHistoryDTOGlobalList));
                        var currencyList = JsonSerializer.Deserialize<List<Currency>>(stringList[1]);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            tempCurrencies = currencyList;
                            //cbbSourceCurrencyName.ItemsSource = currencyList;
                            //cbbSourceCurrencyName.SelectedIndex = 0;
                            //cbbDestinationCurrencyName.ItemsSource = currencyList;
                            //cbbDestinationCurrencyName.SelectedIndex = 0;
                        }));
                        //string responseValue = jsonData;
                        //MessageBox.Show(responseValue);
                    }
                    //MessageBox.Show(responseValue);
                    //MessageBox.Show("Send Data Successfully!");
                }
                //else if (requestMessage.Equals("getCurrenciesList"))
                //{
                //    string jsonData = string.Empty;
                //    var dataBytes = UnicodeEncoding.UTF8.GetBytes(requestMessage);
                //    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
                //    dataBytes = new byte[256];
                //    int dataCount = 0;
                //    while ((dataCount = await stream.ReadAsync(dataBytes, 0, dataBytes.Length)) != 0)
                //    {
                //        jsonData += System.Text.Encoding.UTF8.GetString(dataBytes, 0, dataCount);
                //        if (dataCount != dataBytes.Length)
                //            break;
                //    }
                //    var currencyList = JsonSerializer.Deserialize<List<Currency>>(jsonData);

                //    //Dispatcher.Invoke(new Action(() =>
                //    //{
                //    //    tblContent.Text = $"Number of client connected: {++Count}\r\n";
                //    //}));
                //    string responseValue = jsonData;
                //    MessageBox.Show(responseValue);
                //}
                // Shutdown and end connection
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ConnectServer: " + ex.Message);
            }
        }//end ConnectServer

        public List<RateHistoryDTO> SearchItem(string searchInput, List<RateHistoryDTO> RateHistoryDTOList)
        {
            try
            {
                if (searchInput != null)
                {
                    searchInput = searchInput.ToLower().Trim();
                }
                if (searchInput != null && !string.IsNullOrEmpty(searchInput))
                {
                    RateHistoryDTOList = RateHistoryDTOList // expiridate here may error, nếu thiếu ngoặc() thì nó thực hiện tuần tự từ trái sang phải
               .Where(m => m.ExchangeRateId.ToString().ToLower().Contains(searchInput) || m.SourceCurrencyId.ToString().ToLower().Contains(searchInput) || m.TargetCurrencyId.ToString().ToLower().Contains(searchInput) || m.SourceCurrencyPrice.ToString().ToLower().Contains(searchInput) || m.TargetCurrencyPrice.ToString().ToLower().Contains(searchInput) || m.CurrencySourceName.ToString().ToLower().Contains(searchInput) || m.CurrencyTargetName.ToString().ToLower().Contains(searchInput) || m.CurrencySourceCode.ToString().ToLower().Contains(searchInput) || m.CurrencyTargetCode.ToString().ToLower().Contains(searchInput))
               .ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error In Search");
            }
            return RateHistoryDTOList;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (globalSelectMode == false)
                {
                    var selectedSourceItem = cbbSourceCurrencyName.SelectedItem as Currency;
                    selectedSourceItem = tempCurrencies.FirstOrDefault(c => c.CurrencyId == selectedSourceItem?.CurrencyId);
                    var selectedDestinationItem = cbbDestinationCurrencyName.SelectedItem as Currency;
                    selectedDestinationItem = tempCurrencies.FirstOrDefault(c => c.CurrencyId == selectedDestinationItem?.CurrencyId);
                    decimal result = Convert.ToDecimal(txtNumber.Text) / selectedSourceItem.Price * selectedDestinationItem.Price;
                    lbDestinationValue.Content = result.ToString("0.00");
                }
                else
                {
                    //rateHistoryDTO = lvRateHistoryDTOList.SelectedItem as RateHistoryDTO;
                    // ấn vào list thì nó sẽ bind(thủ công) vào cái combobox
                    decimal result = Convert.ToDecimal(txtNumber.Text) / rateHistoryDTO.SourceCurrencyPrice * rateHistoryDTO.TargetCurrencyPrice;
                    lbDestinationValue.Content = result.ToString("0.00");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in btnChange_Click: {ex.Message}");
            }
        }

        private void btbOpenChangeCurrency_Click(object sender, RoutedEventArgs e)
        {
            if (spChangeBar.Visibility == Visibility.Collapsed)
            {
                spChangeBar.Visibility = Visibility.Visible;
                btbOpenChangeCurrency.Content = "Hidden currency conversion function";
                globalCurrencies = tempCurrencies;
                cbbSourceCurrencyName.ItemsSource = globalCurrencies;
                if (cbbSourceCurrencyName.SelectedIndex == -1)
                    cbbSourceCurrencyName.SelectedIndex = 0;
                cbbDestinationCurrencyName.ItemsSource = globalCurrencies;
                if (cbbDestinationCurrencyName.SelectedIndex == -1)
                    cbbDestinationCurrencyName.SelectedIndex = 0;
            }
            else
            {
                spChangeBar.Visibility = Visibility.Collapsed;
                btbOpenChangeCurrency.Content = "Open currency conversion function";
            }

        }

        private void cbbSourceCurrencyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbbDestinationCurrencyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

        }
        private string searchInput;
        List<RateHistoryDTO> RateHistoryDTOGlobalList;
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchInput = txtSearch.Text;
            LoadForm(SearchItem(searchInput, RateHistoryDTOGlobalList));
        }
        private bool globalSelectMode;
        private void rbCurrentSelectMode_Checked(object sender, RoutedEventArgs e)
        {
            globalSelectMode = true;
        }

        private void rbCurrentNoSelectMode_Checked(object sender, RoutedEventArgs e)
        {
            globalSelectMode = false;
            txtNumber.Clear();
            lbDestinationValue.Content = "";
        }
        RateHistoryDTO rateHistoryDTO;
        private void lvRateHistoryDTOList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (spChangeBar.Visibility != Visibility.Collapsed)
            {
                rbCurrentSelectMode.IsChecked = true;
                rateHistoryDTO = lvRateHistoryDTOList.SelectedItem as RateHistoryDTO;
                txtNumber.Clear();
                lbDestinationValue.Content = "";
                // thay đổi index selected index trong combobox theo thằng selectedListItem
                // tìm index của thằng có id trùng với source trong cbb

                var itemCbbList = cbbSourceCurrencyName.ItemsSource as List<Currency>;
                var selectedCurrencyItem = itemCbbList?.Where(cCbb => cCbb.CurrencyId == rateHistoryDTO?.SourceCurrencyId).FirstOrDefault();
                var selectedCurrencyIndex = itemCbbList.IndexOf(selectedCurrencyItem);
                cbbSourceCurrencyName.SelectedIndex = selectedCurrencyIndex;

                selectedCurrencyItem = itemCbbList?.Where(cCbb => cCbb.CurrencyId == rateHistoryDTO?.TargetCurrencyId).FirstOrDefault();
                selectedCurrencyIndex = itemCbbList.IndexOf(selectedCurrencyItem);
                cbbDestinationCurrencyName.SelectedIndex = selectedCurrencyIndex;
            }
        }

        private void btnExportData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ExportExcelSheet exportExcelSheet = new ExportExcelSheet(lvRateHistoryDTOList, DateTime.Now);
                //MessageBox.Show(exportExcelSheet.ToString());

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml|All files (*.*)|*.*";
                saveFileDialog.ShowDialog();
                //File.WriteAllText(saveFileDialog.FileName, "ahihi");
                //MessageBox.Show(saveFileDialog.FileName);
                //MessageBox.Show(saveFileDialog.FilterIndex.ToString());
                if (saveFileDialog.FileName.ToLower().Contains(".xml"))
                {
                    using (FileStream stream = File.Create(saveFileDialog.FileName))
                    {
                        var xs = new XmlSerializer(typeof(List<RateHistoryDTO>), new XmlRootAttribute("Root"));
                        xs.Serialize(stream, RateHistoryDTOGlobalList);
                    }
                }
                else if (saveFileDialog.FileName.ToLower().Contains(".json"))
                {
                    var option = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(RateHistoryDTOGlobalList, option);
                    File.WriteAllText(saveFileDialog.FileName, jsonString);
                }
                MessageBox.Show("Save Data to file successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in btnExportData_Click {ex}");
            }
        }

        //private void btnABCD_Click(object sender, RoutedEventArgs e)
        //{
        //    GridView gridView = lvRateHistoryDTOList.View as GridView;
        //    MessageBox.Show(gridView.Columns.Count.ToString());
        //}
    }
}
