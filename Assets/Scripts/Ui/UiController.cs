using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject _startWindow;
    [SerializeField] private GameObject _itemsWindow;
    [SerializeField] private GameObject _detectTouchObject;
    [SerializeField] private TextMeshProUGUI[] _moneyTextes;
    [SerializeField] private TextMeshProUGUI _levelText;

    public static UnityAction OnGameStarted;
  
    public int Money;

    private enum DeviceType : byte
    {
        Pc,
        Mobile
    }

    private void Awake()
    {
        Money = 0;
    }

    private void Update()
    {
        foreach(TextMeshProUGUI moneyText in _moneyTextes)
        {
            moneyText?.SetText($"{Money}");
        }
    }

    public static void AnyPlaceTouched()
    {
        OnGameStarted?.Invoke();
    }

    public void ChangeLevel(int level)
    {
        _levelText.SetText($"LEVEL {level}");
    }

    public void AddMoney(int value = 1)
    {
        Money += value;
    }

}
