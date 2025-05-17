using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData: MonoBehaviour
{
    [Header("Settings")]
    public static float Sensitivity;
    public static bool OnGameSound;
    public static bool OnUISound;

    [Header("Upgrades")]
    public static float BlockCost;
    public static float GemSpawnRepeat = 1;
    public static int GemCost;
}
