using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;

    public void Start()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-1775506807889064~7837986057";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-1775506807889064~7837986057";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
        this.RequestInterstitial();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1775506807889064/3371470897";
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111"; // 테스트
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-1775506807889064/3371470897";
#else
            string adUnitId = "unexpected_platform";
#endif


        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1775506807889064/5872090277";
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // 테스트
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-1775506807889064/5872090277";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    public void ShowAd()
    {
        if (this.interstitial.IsLoaded())
            this.interstitial.Show();
    }
}
