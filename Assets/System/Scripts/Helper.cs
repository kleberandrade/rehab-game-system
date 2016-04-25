using UnityEngine;

public class Helper
{
    

    public static bool HasInternetConnection
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
                || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
        }
    }
}
