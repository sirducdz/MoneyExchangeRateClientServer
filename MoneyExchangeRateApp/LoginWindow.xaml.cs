using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoneyExchangeRateApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        //Prn221ProjectContext
        //
        //t;
        //public LoginWindow(Prn221ProjectContext context)
        //{
        //    _context = context;
        //    // giờ mới biết cái include này nó vẫn nhận kể cả không thỏa mãn khóa ^^
        //    //var list = _context.Currencies.Include(c => c.RatehistorySourceCurrencies).Include(c => c.RatehistoryTargetCurrencies).Where(c => c.RatehistorySourceCurrencies.Count != 0 || c.RatehistoryTargetCurrencies
        //    //.Count != 0).ToList();
        //    //MessageBox.Show(list.Count().ToString());
        //    //MessageBox.Show(_context.Currencies.Count().ToString());
        //    InitializeComponent();

        //    var list = _context.RateHistories.Include(rh => rh.SourceCurrency).Include(rh => rh.TargetCurrency).ToList();
        //}


        public LoginWindow()
        {
            InitializeComponent();
        }
    }
}
