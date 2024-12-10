
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataLayer.Models;
using DataLayer.UnitOfWork;
using DataLayer.ModelDTOs;
using Microsoft.EntityFrameworkCore;

namespace MoneyExchangeRateServerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IUnitOfWork _unitOfWork;
        string host = "127.0.0.1";
        int port = 9999;
        public MainWindow(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
            Listent();
        }
        void ExecuteServer() //string host, int port
        {
            int Count = 0;
            TcpListener server = null;
            try
            {

                IPAddress localAddr = IPAddress.Parse(host);
                server = new TcpListener(localAddr, port);
                // Start listening for client requests.
                server.Start();

                Dispatcher.Invoke(new Action(() =>
                {
                    tblContent.Text = "Waiting for a connection...";
                }));
                // Enter the listening loop.
                while (true)
                {
                    // Perform a blocking call to accept requests.
                    TcpClient client = server.AcceptTcpClient();
                    Dispatcher.Invoke(new Action(() =>
                    {
                        tblContent.Text = $"Number of client connected: {++Count}\r\n";
                    }));
                    //Thread thread = new Thread(new ParameterizedThreadStart(ProcessMessage));
                    //thread.Start(client);
                    // chạy tốt hơn cái trên, nó ko gây lỗi entity framework chạy trên nhiều threads
                    Task task = Task.Run(() => ProcessMessage(client));
                    //task.Start();
                    //Mình định chia nhiều thread ở đây
                    //Thread thread = new Thread(new ParameterizedThreadStart(ProcessMessage));
                    //thread.Start(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ExecuteServer: {0}", ex.Message);
            }
            finally
            {
                server.Stop();
                //btnListent.IsEnabled = true;
            }
        }
        // random rate + hoặc - theo một tỉ lệ nhất định 
        async Task RandomCurrencyRate(List<Currency> currencies)
        {
            try
            {
                Random random = new Random();
                //currencies = await currencies.AsQueryable().Where(c => !c.Code.Equals("USD")).ToListAsync();
                foreach (Currency currency in currencies)
                {
                    if (!currency.Code.Equals("USD"))
                    {
                        int randomNumber = random.Next(0, 2);
                        if (randomNumber == 1)
                            currency.Price = currency.Price + currency.Price * 0.1m;
                        else
                            currency.Price = currency.Price - currency.Price * 0.1m;
                        _unitOfWork.CurrencyRepository.Update(currency);
                    }
                }
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error In RandomCurrencyRate: {e.Message}");
            }
        }
        async Task AddQueryToRateHistoryTable(List<Currency> currencies)
        {
            try
            {
                for (int i = 0; i < currencies.Count - 1; i++)
                {
                    for (int j = i + 1; j < currencies.Count; j++)
                    {
                        RateHistory rateHistory = new RateHistory()
                        {
                            SourceCurrencyId = currencies[i].CurrencyId,
                            TargetCurrencyId = currencies[j].CurrencyId,
                            Date = DateTime.Now,
                            SourceCurrencyPrice = currencies[i].Price,
                            TargetCurrencyPrice = currencies[j].Price,
                        };
                        _unitOfWork.RateHistoryRepository.Add(rateHistory);
                    }
                }
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error in AddQueryToRateHistoryTable {ex.Message}");
            }
        }
        async Task ProcessMessage(object parm)
        {
            string data;
            int count;
            try
            {
                TcpClient client = parm as TcpClient;
                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();
                // Loop to receive all the data sent by the client.
                string jsonData = string.Empty;
                while (true)
                {
                    count = await stream.ReadAsync(bytes, 0, bytes.Length);
                    data = System.Text.Encoding.UTF8.GetString(bytes, 0, count);
                    if (data.Equals("getRateHistoryList"))
                    {
                        var option = new JsonSerializerOptions { WriteIndented = true };
                        var dataList = await _unitOfWork.RateHistoryRepository.GetAllRateHistoryIncluded();
                        //var dataList = await _prn221ProjectContext.RateHistories.ToListAsync();
                        var RateHistoryDTOList = new List<RateHistoryDTO>();
                        foreach (var rateHistory in dataList)
                        {
                            RateHistoryDTOList.Add(new RateHistoryDTO()
                            {
                                ExchangeRateId = rateHistory.ExchangeRateId,
                                SourceCurrencyId = rateHistory.SourceCurrencyId,
                                TargetCurrencyId = rateHistory.TargetCurrencyId,
                                Date = rateHistory.Date,
                                SourceCurrencyPrice = decimal.Parse(rateHistory.SourceCurrencyPrice.ToString("0.00")),
                                TargetCurrencyPrice = decimal.Parse(rateHistory.TargetCurrencyPrice.ToString("0.00")),
                                CurrencySourceName = rateHistory.SourceCurrency.Name,
                                CurrencyTargetName = rateHistory.TargetCurrency.Name,
                                CurrencySourceCode = rateHistory.SourceCurrency.Code,
                                CurrencyTargetCode = rateHistory.TargetCurrency.Code,
                            });
                        }
                        jsonData = JsonSerializer.Serialize(RateHistoryDTOList, option);

                        var CurrenciesList = await _unitOfWork.CurrencyRepository.GetAllCurrenciesIncluded();
                        // display current table in server
                        LoadForm(CurrenciesList);
                        jsonData = jsonData + "seperated" + JsonSerializer.Serialize(CurrenciesList, option);
                        await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonData));
                        Thread.Sleep(15000);
                        await RandomCurrencyRate(CurrenciesList);
                        await AddQueryToRateHistoryTable(CurrenciesList);

                    }
                    //else if(data.Equals("userLogout")) // nhận tín hiệu từ client, kết thúc luồng chạy của server
                    //{
                    //    break;
                    //}
                    //else if (data.Equals("getCurrenciesList"))
                    //{
                    //    var option = new JsonSerializerOptions { WriteIndented = true };
                    //    var dataList = await _unitOfWork.CurrencyRepository.GetAllCurrenciesIncluded();
                    //    jsonData = JsonSerializer.Serialize(dataList, option);
                    //    //jsonData = "ahhihi";
                    //    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonData));
                    //    break;
                    //}
                }
                //var iphoneList = JsonSerializer.Deserialize<List<TblPhone>>(jsonData);
                client.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ProcessMessage: {ex.Message}");
            }
        }
        public void LoadForm(List<Currency> CurrencyList)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    lvCurrencyList.ItemsSource = CurrencyList;
                }));
            }
            catch (Exception)
            {
                MessageBox.Show("Error In LoadForm");
            }
        }

        //public List<Currency> SearchItem(string searchInput, List<Currency> CurrencyList)
        //{
        //    try
        //    {
        //        if (searchInput != null)
        //        {
        //            searchInput = searchInput.ToLower().Trim();
        //        }
        //        if (searchInput != null && !string.IsNullOrEmpty(searchInput))
        //        {
        //            CurrencyList = CurrencyList // expiridate here may error, nếu thiếu ngoặc() thì nó thực hiện tuần tự từ trái sang phải
        //       .Where(m => m.CurrencyId.ToString().ToLower().Contains(searchInput) || m.Code.ToString().ToLower().Contains(searchInput) || m.Name.ToString().ToLower().Contains(searchInput) || m.Price.ToString().ToLower().Contains(searchInput))
        //       .ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Error In Search");
        //    }
        //    return CurrencyList;
        //}

        private void Listent()
        {
            //btnListent.IsEnabled = false;
            Thread executeServerThread = new Thread(ExecuteServer); // không thực thi server ở luồng chính
            executeServerThread.Start();

        }
    }
}