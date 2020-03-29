using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MikroTikTestingApp
{
    static class MToperationClass
    {
        public static string IP = string.Empty;
        public static string Login = string.Empty;
        public static string Password = string.Empty;
        public static string EtherInt = string.Empty;
        public static string WirelessInt = string.Empty;

        static MK mikrotik;
        static List<string> netInterfaces;
        static string response = string.Empty;
        static string contResponse = string.Empty;

        // ---------------------- CONNECTION TEST -------------------------------
        public static List<string> MikroTikGetInterfaces()
        {
            contResponse = string.Empty;
            try { mikrotik = new MK(IP); }
            catch (System.Net.Sockets.SocketException sEx)
            {
                MessageBox.Show(sEx.ToString());
                return null;
            }
            bool logged = mikrotik.Login(Login, Password, out contResponse);
            GetInterfaces(logged);
            mikrotik.Close();
            return netInterfaces;
        }    

        static void GetInterfaces(bool logged)
        {
            if (!logged)
                return;

            mikrotik.Send("/interface/print", true);

            string response = string.Empty;
            foreach (string h in mikrotik.Read())
            {
                response += h;
            }
            List<string> respArray = (response.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            respArray.RemoveAt(0);
            respArray.RemoveAt(respArray.Count - 1);

            netInterfaces = new List<string>();
            for (int i = 0; i < respArray.Count; i++)
            {
                if (respArray[i] == "name")
                    netInterfaces.Add(respArray[i + 1]);
            }
        }
        //----------------------------------------------
        //--------------------------- ETHERNET RATE --------------------------------------------------------------------------------------------

        public static string MikrotikGetEthernetRate()
        {
            contResponse = string.Empty;
            try { mikrotik = new MK(IP); }
            catch (System.Net.Sockets.SocketException)
            {
                return "conn err";
            }
            bool logged = mikrotik.Login(Login, Password, out contResponse);
            string rate = GetEthernetRate(logged);
            mikrotik.Close();
            return rate;
        }
        static string GetEthernetRate(bool logged)
        { 
            if (!logged)
                return "log err";

            mikrotik.Send("/interface/ethernet/monitor");
            mikrotik.Send($"=numbers={EtherInt}");
            mikrotik.Send("=once=", true);

            response = string.Empty;
            foreach (string h in mikrotik.Read())
            {
                response += h;
            }
            List<string> respArray = (response.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            respArray.RemoveAt(0);
            respArray.RemoveAt(respArray.Count - 1);

            string rate = string.Empty;
            for (int i = 0; i < respArray.Count; i++)
            {
                if (respArray[i] == "rate")
                {
                    rate = respArray[i + 1];
                    break;
                }
            }
            if (rate == string.Empty)
                return "no link";
            else return rate;
        }

        //---------------------------- WIRELESS SIGNAL -------------------------------------------------------------------------------------------
        public static string MikrotikGetWirelessSignal()
        {
            contResponse = string.Empty;
            try { mikrotik = new MK(IP); }
            catch (System.Net.Sockets.SocketException)
            {
                return "conn err";
            }
            bool logged = mikrotik.Login(Login, Password, out contResponse);
            string signal = GetWirelessSignal(logged);
            mikrotik.Close();
            return signal;
        }
        static string GetWirelessSignal(bool logged)
        {
            if (!logged)
                return "log err";

            mikrotik.Send("/interface/wireless/monitor");
            mikrotik.Send($"=numbers={WirelessInt}");
            mikrotik.Send("=once=", true);

            response = string.Empty;
            foreach (string h in mikrotik.Read())
            {
                response += h;
            }
            List<string> respArray = (response.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            respArray.RemoveAt(0);
            respArray.RemoveAt(respArray.Count - 1);

            string signal = string.Empty;
            for (int i = 0; i < respArray.Count; i++)
            {
                if (respArray[i] == "signal-strength")
                {
                    signal = respArray[i + 1];
                    break;
                }
            }
            if (signal == string.Empty)
                return "no signal";
            else return signal;
        }
    }
}
