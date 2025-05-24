using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UiController : MonoBehaviour
{
    public static UiController Instance;

    [SerializeField] private GameObject _startWindow;
    [SerializeField] private GameObject _itemsWindow;
    [SerializeField] private TextMeshProUGUI[] _moneyTextes;
    [SerializeField] private TextMeshProUGUI _levelText;

    public static UnityAction OnGameStarted;

    [SerializeField] private int _money;
    public int Money { get { return _money; } set { _money = value; } }

    private enum DeviceType : byte
    {
        Pc,
        Mobile
    }

    private void Awake()
    {
        Instance = this;
        //_money = 0;
    }

    private void Start()
    {
        ChangeAllMoneyDisplay();
    }

    private void Update()
    {
        ChangeAllMoneyDisplay();
    }

    public static void AnyPlaceTouched()
    {
        OnGameStarted?.Invoke();
    }

    private void ChangeAllMoneyDisplay()
    {
        foreach (TextMeshProUGUI moneyText in _moneyTextes)
        {
            moneyText?.SetText($"{_money}");
        }
    }

    public void ChangeLevel(int level)
    {
        _levelText.SetText($"LEVEL {level}");
    }

    public void AddMoney(int value = 1)
    {
        _money += value;
    }

    public void RemoveMoney(int value = 1)
    {
        _money -= value;
    }
}
