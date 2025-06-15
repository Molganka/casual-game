using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // LEVELS
    public static void SaveLevel(LevelManager.LevelScenes level)
    {
        PlayerPrefs.SetInt("LevelIndex", (int)level);
        PlayerPrefs.Save();
    }
    public static LevelManager.LevelScenes LoadLevel(LevelManager.LevelScenes defaultLevel = LevelManager.LevelScenes.Level1)
    {
        return (LevelManager.LevelScenes)PlayerPrefs.GetInt("LevelIndex", (int)defaultLevel);
    }

    // SETTINGS
    public static void SaveSettings()
    {
        PlayerPrefs.SetFloat("Settings_Sensitivity", GameData.Sensitivity);
        PlayerPrefs.SetInt("Settings_OnGameSound",  Convert.ToInt32(GameData.OnGameSound));
        PlayerPrefs.SetInt("Settings_OnUISound", Convert.ToInt32(GameData.OnUISound));
        PlayerPrefs.Save();
    }
    public static void LoadSettings()
    {
        GameData.Sensitivity = PlayerPrefs.GetFloat("Settings_Sensitivity", GameData.Sensitivity);
        GameData.OnGameSound = Convert.ToBoolean(PlayerPrefs.GetInt("Settings_OnGameSound", Convert.ToInt32(GameData.OnGameSound)));
        GameData.OnUISound = Convert.ToBoolean(PlayerPrefs.GetInt("Settings_OnUISound", Convert.ToInt32(GameData.OnUISound)));
    }

    // COINS
    public static void SaveMoney(int money)
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }
    public static int LoadMoney(int defaultMoney = 0)
    {
        return PlayerPrefs.GetInt("Money", defaultMoney);
    }

    // UPGRADES
    public static void SaveCoinUpgradeIndex(int index)
    {
        PlayerPrefs.SetInt("Upgrade_Coin", index);
        PlayerPrefs.Save();
    }

    public static void SaveGemUpgradeIndex(int index)
    {
        PlayerPrefs.SetInt("Upgrade_Gem", index);
        PlayerPrefs.Save();
    }

    public static int LoadCoinUpgradeIndex()
    {
        return PlayerPrefs.GetInt("Upgrade_Coin", 0);
    }

    public static int LoadGemUpgradeIndex()
    {
        return PlayerPrefs.GetInt("Upgrade_Gem", 0);
    }

    // ITEMS
    public static void SaveItems(ItemsWindow.Type typeStruct, int type)//type - 1, 2
    {
        string json = JsonUtility.ToJson(typeStruct);
        if (type == 1)
            PlayerPrefs.SetString("Items_Type1", json);
        else if (type == 2)
            PlayerPrefs.SetString("Items_Type2", json);
        else
            Debug.LogError("Wrong type");
        PlayerPrefs.Save();
    }

    public static ItemsWindow.Type LoadItems(int type)
    {
        ItemsWindow.Type emptyType = new ItemsWindow.Type
        {
            InaccessibleItems = null,
            AccessibleItems = null
        };

        string playerPrefsName = "";

        if (type == 1)
            playerPrefsName = "Items_Type1";
        else if (type == 2)
            playerPrefsName = "Items_Type2";
        else
            Debug.LogError("Wrong type");

        if (PlayerPrefs.HasKey(playerPrefsName))
        {
            string json = PlayerPrefs.GetString(playerPrefsName);
            return JsonUtility.FromJson<ItemsWindow.Type>(json);
        }
        else
        {
            return emptyType;
        }
    }

    public static void SaveSelectedItem(int type, int itemNum)
    {
        string playerPrefsName = "";

        if (type == 1)
            playerPrefsName = "SelectedItem_Type1";
        else if (type == 2)
            playerPrefsName = "SelectedItem_Type2";
        else
            Debug.LogError("Wrong type");

        PlayerPrefs.SetInt(playerPrefsName, itemNum);
        PlayerPrefs.Save();
    }

    public static int LoadSelectedItem(int type)
    {
        string playerPrefsName = "";

        if (type == 1)
            playerPrefsName = "SelectedItem_Type1";
        else if (type == 2)
            playerPrefsName = "SelectedItem_Type2";
        else
            Debug.LogError("Wrong type");

        return PlayerPrefs.GetInt(playerPrefsName, -1);
    }

    public static void SaveOpenItemButtonIndex(int index)
    {
        PlayerPrefs.SetInt("OpenItemButtonIndex", index);
        PlayerPrefs.Save();
    }

    public static int LoadOpenItemButtonIndex()
    {
        return PlayerPrefs.GetInt("OpenItemButtonIndex", 0);
    }
}
