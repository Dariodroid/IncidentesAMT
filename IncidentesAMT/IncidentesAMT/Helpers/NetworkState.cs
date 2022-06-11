using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace IncidentesAMT.Helpers
{
    public static class NetworkState
    {
        public static bool IsConnected = false;

        public static bool iHaveInternet()
        {
            NetworkAccess current = Connectivity.NetworkAccess;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            return determineState(current);

        }

        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            determineState(e.NetworkAccess);
        }

        public static bool determineState(NetworkAccess state)
        {
            if(state == NetworkAccess.Internet)
            {
                return IsConnected = true;
            }
            else if(state == NetworkAccess.Local)
            {
                return IsConnected = true;
            }
            else if(state == NetworkAccess.ConstrainedInternet)
            {
                return IsConnected = false;
            }
            else
            {
                return IsConnected = false;
            }
        }
    }
}
