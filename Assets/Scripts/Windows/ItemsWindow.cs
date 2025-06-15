using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemsWindow : MonoBehaviour
{
    public static ItemsWindow Instance;

    [SerializeField] private GameObject[] _itemTypes;
    [SerializeField] private GameObject[] _buttons;
    [SerializeField] private Color _textColorForActiveButton;
    [SerializeField] private Color _textColorForDeactiveButton;
    [SerializeField] private int _buttonAlphaValue;

    private PlayerItems _playerItems;

    public Type[] Types;
    public static int[] SelectedItemsUI = new int[2] {-1, -1};
    private GameObject _currentType;
    private GameObject _currentButton;
    private int _currentIndexType;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        LevelManager.OnLevelChanged += FindPlayerItems;
        LevelManager.OnLevelChanged += LoadItems;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelChanged -= FindPlayerItems;
        LevelManager.OnLevelChanged -= LoadItems;
    }

    private void FindPlayerItems()
    {
        _playerItems = FindFirstObjectByType<PlayerItems>();
    }

    private void LoadItems()
    {
        _currentButton = _buttons[0];
        ChangeType(1);

        for (int i = 1; i <= 2; ++i)
        {
            Type loadType = SaveManager.LoadItems(i);
            if (loadType.InaccessibleItems != null && loadType.AccessibleItems != null)
            {
                Types[i - 1] = loadType;
                foreach (GameObject item in Types[i - 1].AccessibleItems)
                {
                    OpenItem(item);
                }
            }

            int loadItem = SaveManager.LoadSelectedItem(i);
            if (loadItem != -1)
            {
                ChangeItem(i - 1, loadItem);
            }
        }

        LevelManager.OnLevelChanged -= LoadItems;
    }

    public void ChangeType(int num)
    {
        num -= 1; // becasuase index from 0
        _currentIndexType = num;
        ChangeItemsType(_itemTypes[num]);
        ChangeButtonAppearance(_buttons[num]);
    }
    private void ChangeItemsType(GameObject type)
    {
        if(type != _currentType)
        {
            if (_currentType != null)
                _currentType.SetActive(false);

            type.SetActive(true);
            _currentType = type;
        }        
    }
    private void ChangeButtonAppearance(GameObject button)
    {
        Image image1 = _currentButton.GetComponent<Image>();
        Color color1 = image1.color;
        color1.a = 0f;
        image1.color = color1;
        _currentButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = _textColorForDeactiveButton;

        Image image2 = button.GetComponent<Image>();
        Color color2 = image2.color;
        color2.a = _buttonAlphaValue / 255.0f; //convert alpha from inspector (0-255) to code (0-1)
        image2.color = color2;
        button.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = _textColorForActiveButton;
        _currentButton = button;    
    }

    public void OpenRandomItem()
    {
        Type type = Types[_currentIndexType];
        if (type.InaccessibleItems.Count != 0)
        {
            GameObject randomItem = type.InaccessibleItems[UnityEngine.Random.Range(0, type.InaccessibleItems.Count)];
            OpenItem(randomItem);
            type.AccessibleItems.Add(randomItem);
            type.InaccessibleItems.Remove(randomItem);
            SaveManager.SaveItems(type, _currentIndexType+1);
        }
    }
    private void OpenItem(GameObject item)
    {
        item.transform.GetChild(0).gameObject.SetActive(true);
        item.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ChangeItem(int type, int itemNum)
    {
        GameObject itemObject = Types[type].AccessibleItems[itemNum];
        ItemUI itemUI = itemObject.GetComponent<ItemUI>();
        if (SelectedItemsUI[type] != -1)
        {
            Types[type].AccessibleItems[SelectedItemsUI[type]].GetComponent<ItemUI>().SetSelectOff();
        }

        if (SelectedItemsUI[type] != -1 && itemObject == Types[type].AccessibleItems[SelectedItemsUI[type]])
        {
            SelectedItemsUI[type] = -1;

            if (itemUI.ItemType == ItemUI.ItemTypes.Object)
                _playerItems.ChangeItem(null);
            else if (itemUI.ItemType == ItemUI.ItemTypes.Material)
                _playerItems.ChangeTrailMaterial(null);
            SaveManager.SaveSelectedItem(type+1, -1);
        }
        else
        {
            itemUI.SetSelectOn();
            SelectedItemsUI[type] = itemNum;

            if (itemUI.ItemType == ItemUI.ItemTypes.Object)
                _playerItems.ChangeItem(itemUI);
            else if (itemUI.ItemType == ItemUI.ItemTypes.Material)
                _playerItems.ChangeTrailMaterial(itemUI);
            SaveManager.SaveSelectedItem(type+1, itemNum);
        }
    }

    public bool IsThereAccessibleItems()
    {
        return Types[_currentIndexType].InaccessibleItems.Count > 0;
    }

    [Serializable]
    public struct Type
    {
        public List<GameObject> InaccessibleItems;
        public List<GameObject> AccessibleItems;
    }
}
