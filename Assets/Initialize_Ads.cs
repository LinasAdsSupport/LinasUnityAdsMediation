using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Mediation;
using System.Threading.Tasks;

public class Initialize_Ads : MonoBehaviour
{
    public string androidAdUnitId;
    public string iosAdUnitId;
    IInterstitialAd interstitialAd;

    async void Start()
    {
        InitializationOptions options = new InitializationOptions();
        options.SetGameId("4873360");

        // Initialize the package to access API
        await UnityServices.InitializeAsync(options);

        // Instantiate an interstitial ad object with platform-specific Ad Unit ID
        if (Application.platform == RuntimePlatform.Android)
        {
            interstitialAd = MediationService.Instance.CreateInterstitialAd(androidAdUnitId);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            interstitialAd = MediationService.Instance.CreateInterstitialAd(iosAdUnitId);
        }
#if UNITY_EDITOR
        else
        {
            interstitialAd = MediationService.Instance.CreateInterstitialAd("myExampleAdUnitId");
        }
#endif

        // Subscribe callback methods to load events:
        interstitialAd.OnLoaded += AdLoaded;
        interstitialAd.OnFailedLoad += AdFailedToLoad;

        // Subscribe callback methods to show events:
        interstitialAd.OnShowed += AdShown;
        interstitialAd.OnFailedShow += AdFailedToShow;
        interstitialAd.OnClosed += AdClosed;
        try
        {
            // Load an ad:
            await interstitialAd.LoadAsync();
            // Here our load succeeded.

            // This is for demonstration purposes, we recommend you load the
            // ad in advance, and show when needed as load may take some time
            await ShowAd();
        }
        catch (Exception e)
        {
            // Here our load failed.
        }
    }

    // Implement load event callback methods:
    void AdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Ad loaded.");
        // Execute logic for when the ad has loaded
    }

    void AdFailedToLoad(object sender, LoadErrorEventArgs args)
    {
        Debug.Log("Ad failed to load.");
        // Execute logic for the ad failing to load.
    }

    // Implement show event callback methods:
    void AdShown(object sender, EventArgs args)
    {
        Debug.Log("Ad shown successfully.");
        // Execute logic for the ad showing successfully.
    }

    void AdFailedToShow(object sender, ShowErrorEventArgs args)
    {
        Debug.Log("Ad failed to show.");
        // Execute logic for the ad failing to show.
    }

    private void AdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad has closed");
        // Execute logic after an ad has been closed.
    }

    public async Task ShowAd()
    {
        // Ensure the ad has loaded, then show it.
        if (interstitialAd.AdState == AdState.Loaded)
        {
            try
            {
                await interstitialAd.ShowAsync();
                // Here show succeeded.
            }
            catch (Exception e)
            {
                // Here show failed.
            }
        }
    }
}