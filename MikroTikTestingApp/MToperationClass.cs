﻿using System;
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
        public static string MTserialNo = string.Empty;

        //old mt test
        public static bool isOlderMT;
        public static bool dontTestWLAN;
        public static bool alterMT_APIread;

        static MK mikrotik;
        static List<string> netInterfaces;
        static string response = string.Empty;
        static string contResponse = string.Empty;

        // ---------------------- CONNECTION TEST -------------------------------
        public static List<string> MikroTikGetInterfaces(out string errOutput)
        {
            netInterfaces = new List<string>();
            contResponse = string.Empty;
            try { mikrotik = new MK(IP); }
            catch (System.Net.Sockets.SocketException sEx)
            {
                errOutput = sEx.ToString();
                //errOutput = "Cannot connect to Host";
                return null;
            }
            bool logged = mikrotik.Login(Login, Password, out contResponse);
            GetInterfaces(logged);
            mikrotik.Close();
            errOutput = string.Empty;
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

            for (int i = 0; i < respArray.Count; i++)
            {
                if (respArray[i] == "name")
                    netInterfaces.Add(respArray[i + 1]);
            }
        }
        //-----------------------------------------------------
        //--------------------------- SERIAL NO ---------------
        public static void MikrotikGetSerial()
        {
            contResponse = string.Empty;
            MTserialNo = string.Empty;
            try { mikrotik = new MK(IP); }
            catch (System.Net.Sockets.SocketException)
            {
                MTserialNo = "conn err";
            }
            bool logged = mikrotik.Login(Login, Password, out contResponse);
            GetSerial(logged);
            mikrotik.Close();
        }
        static void GetSerial(bool logged)
        {
            if (!logged)
                return;
            MTserialNo = "no serial!";

            mikrotik.Send("/system/routerboard/print", true);

            response = string.Empty;
            foreach (string h in mikrotik.Read())
            {
                response += h;
            }
            List<string> respArray = (response.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            respArray.RemoveAt(0);
            respArray.RemoveAt(respArray.Count - 1);

            for (int i = 0; i < respArray.Count; i++)
            {
                if (respArray[i] == "serial-number")
                {
                    MTserialNo = respArray[i + 1];
                    break;
                }
            }
        }
        //-----------------------------------------------------
        //--------------------------- RECV BYTES TEST ---------------
        public static string MikrotikRecvBytes()
        {
            contResponse = string.Empty;
            try { mikrotik = new MK(IP); }
            catch (System.Net.Sockets.SocketException)
            {
                return "conn err";
            }
            bool logged = mikrotik.Login(Login, Password, out contResponse);
            string bytes = GetRbytes(logged);
            mikrotik.Close();
            return bytes;
        }
        static string GetRbytes(bool logged)
        {
            if (!logged)
                return "log err";

            mikrotik.Send("/system/script/environment/print",true);

            response = string.Empty;
            foreach (string h in mikrotik.Read())
            {
                response += h;
            }
            List<string> respArray = (response.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            respArray.RemoveAt(0);
            //respArray.RemoveAt(respArray.Count - 1);

            string txB = string.Empty;
            for (int i = 0; i < respArray.Count; i++)
            {
                if (respArray[i] == "rxBytes")
                {
                    txB = respArray[i + 2];
                    break;
                }
            }
            if (txB == string.Empty)
                return "none";
            else return txB.Substring(0,txB.Length-5);
        }
        //-----------------------------------------------------
        //--------------------------- UPTIME ---------------
        public static string MikrotikGetUpTime()
        {
            contResponse = string.Empty;
            try { mikrotik = new MK(IP); }
            catch (System.Net.Sockets.SocketException)
            {
                return "conn err";
            }
            bool logged = mikrotik.Login(Login, Password, out contResponse);
            string up = GetUpTime(logged);
            mikrotik.Close();
            return up;
        }
        static string GetUpTime(bool logged)
        {
            if (!logged)
                return "log err";

            mikrotik.Send("/system/resource/print",true);

            response = string.Empty;
            foreach (string h in mikrotik.Read())
            {
                response += h;
            }
            List<string> respArray = (response.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            respArray.RemoveAt(0);
            respArray.RemoveAt(respArray.Count - 1);

            string up = string.Empty;
            for (int i = 0; i < respArray.Count; i++)
            {
                if (respArray[i] == "uptime")
                {
                    up = respArray[i + 1];
                    break;
                }
            }
            if (up == string.Empty)
                return "uptime error";
            else return up;
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

        //---------------------------- WIRELESS SIGNAL and RATES -------------------------------------------------------------------------------------------
        public static string MikrotikGetWirelessSignal(out string wirelessRates)
        {
            if (dontTestWLAN)
            {
                wirelessRates = "conn err";
                return "conn err";
            }
            contResponse = string.Empty;
            try { mikrotik = new MK(IP); }
            catch (System.Net.Sockets.SocketException)
            {
                wirelessRates = "conn err";
                return "conn err";
            }
            bool logged = mikrotik.Login(Login, Password, out contResponse);
            string signal = GetWirelessSignal(logged, out wirelessRates);
            mikrotik.Close();
            return signal;
        }
        static string GetWirelessSignal(bool logged,out string WIRrates)
        {
            if (!logged)
            {
                WIRrates = "log err";
                return "log err";
            }
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
            WIRrates = string.Empty;
            for (int i = 0; i < respArray.Count; i++)
            {
                if (respArray[i] == "signal-strength")
                {
                    signal = respArray[i + 1];
                    if(alterMT_APIread)
                        WIRrates = "Tx:"+respArray[i-9] +"\nRx:"+ respArray[i-7];//Tx/Rx
                    else WIRrates = "Tx:" + respArray[i - 7] + "\nRx:" + respArray[i - 5];
                    break;
                }
            }
            if (signal == string.Empty)
            {
                WIRrates = "no signal";
                return "no signal";
            }
            else
            {
                return signal;
            }
        }
    }
}
