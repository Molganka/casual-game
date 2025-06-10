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

            });
        }
    }

    public static void ShowRewardedAd()
    {
        CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded, () => // or CrazyAdType.Rewarded
        {
        }, (error) =>
        {
        }, () =>
        {
            LevelCompleter.Instance.CollectDoubleCoinsAfterAd();
        });
    }
}
