using UnityEngine;

public class BasicFinishWindow : MonoBehaviour
{
    public static float Money { get; private set; }   

    public static void AddMoney()
    {
        Money += GameData.BlockCost;
    }  
}
