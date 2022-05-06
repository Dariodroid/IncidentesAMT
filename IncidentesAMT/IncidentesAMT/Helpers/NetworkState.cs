using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace IncidentesAMT.Helpers
{
    public class NetworkState
    {
        public static bool IsConnected = false;

        public void iHaveInternet()
        {
            NetworkAccess current = Connectivity.NetworkAccess;
            determineState(current);

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            determineState(e.NetworkAccess);
        }

        public void determineState(NetworkAccess state)
        {
            if(state == NetworkAccess.Internet)
            {
                IsConnected = true;
            }
            else if(state == NetworkAccess.Local)
            {
                IsConnected= true;
            }
            else if(state == NetworkAccess.ConstrainedInternet)
            {
                IsConnected = false;
            }
            else
            {
                IsConnected = false;
            }
        }
    }
}
