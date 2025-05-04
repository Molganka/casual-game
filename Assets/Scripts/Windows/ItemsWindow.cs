using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] _itemTypes;
    [SerializeField] private GameObject[] _buttons;
    [SerializeField] private Type[] _inaccessibleItemTypes;  
    [SerializeField] private Color _textColorForActiveButton;
    [SerializeField] private Color _textColorForDeactiveButton;
    [SerializeField] private int _buttonAlphaValue;
    public static int TypesCount = 2;

    private PlayerItems _playerItems => FindFirstObjectByType<PlayerItems>();
    public static ItemUI[] SelectedItemsUI = new ItemUI[TypesCount];
    private GameObject _currentType;
    private GameObject _currentButton;
    private int _currentIndexType;

    private void Start()
    {
        _currentButton = _buttons[0];
        ChangeType(1);
    }

    public void ChangeType(int num)
    {
        Debug.Log("Change type item: " + num);
        num -= 1; // becasuase index from 0
        _currentIndexType = num;
        Debug.Log("Change current index type: " + _itemTypes[num]);
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
        Type type = _inaccessibleItemTypes[_currentIndexType];
        if (type.InaccessibleItems.Count != 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, type.InaccessibleItems.Count);
            OpenItem(type.InaccessibleItems[randomIndex]);
            type.InaccessibleItems.RemoveAt(randomIndex);
        }
    }
    private void OpenItem(GameObject item)
    {
        item.transform.GetChild(0).gameObject.SetActive(true);
        item.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ChangeItem(ItemUI itemUI)
    {
        if(SelectedItemsUI[itemUI.Type] != null)
            SelectedItemsUI[itemUI.Type].SetSelectOff();

        if (itemUI == SelectedItemsUI[itemUI.Type])
        {
            Debug.Log("1");
            SelectedItemsUI[itemUI.Type] = null;

            if (itemUI.ItemType == ItemUI.ItemTypes.Object)
                _playerItems.ChangeItem(null);
            else if (itemUI.ItemType == ItemUI.ItemTypes.Material)
                _playerItems.ChangeTrailMaterial(null);
        }
        else
        {
            Debug.Log("2");
            itemUI.SetSelectOn();
            SelectedItemsUI[itemUI.Type] = itemUI;

            if (itemUI.ItemType == ItemUI.ItemTypes.Object)
                _playerItems.ChangeItem(itemUI);
            else if (itemUI.ItemType == ItemUI.ItemTypes.Material)
                _playerItems.ChangeTrailMaterial(itemUI);
        }
    }

    [Serializable]
    public struct Type
    {
        public List<GameObject> InaccessibleItems;
    }
}
