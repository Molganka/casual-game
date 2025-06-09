using CrazyGames;
using UnityEngine;

public class CrazyGamesSDK : MonoBehaviour
{
    private void Start()
    {
        if (CrazySDK.IsAvailable)
        {
            CrazySDK.Init(() =>
            {
                Debug.Log("CrazySDK initialized");
            });
        }
    }

    public static void ShowRewardedAd()
    {
        CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () => // or CrazyAdType.Rewarded
        {
            Debug.Log("Rewarded ad started");
        }, (error) =>
        {
            Debug.Log("Rewarded ad error");
        }, () =>
        {
            Debug.Log("Rewarded ad finished corectly");
            LevelCompleter.Instance.CollectDoubleCoinsAfterAd();
        });
    }
}
