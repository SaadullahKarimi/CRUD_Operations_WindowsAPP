using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SuperMarketWindowsAPP
{
    class Connectivity
    {
        public static SqlConnection cn = new SqlConnection("Data source = MYADMIN;  initial catalog=SuperMarket; Integrated Security=true; user =rasouli; password=1234@asdf");

        public static void Connect()
        {
            if (cn.State == System.Data.ConnectionState.Closed)
                cn.Open();
        }
        public static void Disconnect()
        {
            if (cn.State == System.Data.ConnectionState.Open)

                cn.Close();
        }
    }
}
