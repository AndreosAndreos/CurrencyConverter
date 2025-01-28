using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CurrencyConverter_Static.MainWindow;

namespace CurrencyConverterAPI
{
    public class Root
    {
        public Rate rates { get; set; } // use the same name as in json file 
        public long timestamp;
        public string license;
    }
}
