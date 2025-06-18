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
}
