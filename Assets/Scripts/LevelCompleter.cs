using DG.Tweening;
using System;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleter : MonoBehaviour
{   
    [SerializeField] private Animation _animation;
    [SerializeField] private Button _button1;
    [SerializeField] private Button _button2;
    [SerializeField] private CoinCollectEffect _coinCollectEffect;
    [SerializeField] private GameObject _totalMoneyObject;
    [SerializeField] private TextMeshProUGUI _basicMoneyText;
    [SerializeField] private TextMeshProUGUI _bonusMoneyText;
    [SerializeField] private TextMeshProUGUI _totalMoneyText;

    [SerializeField] private float _timeToCountTotalMoney;
    [SerializeField] private float _duration = 2f;

    private int _totalCoins = 0;
    private int _doubleCoins = 0;

    private void Start()
    {
        UiController.OnGameStarted += EnableButtons;
    }

    public void StartLevelComplete()
    {
        Invoke(nameof(CountTotalMoney), _timeToCountTotalMoney);
    }

    private void CountTotalMoney()
    {
        if(FinishWindow.Instance.Gems > 0)
        {
            _totalCoins = FinishWindow.Instance.Gems * GameData.GemCost;
        }
        else
        {
            _totalCoins = Convert.ToInt32(GameData.Multiplier * CollisionHandler.Multiplier);
        }

        _doubleCoins = _totalCoins * 2;
        Debug.Log("Double Money: " + _doubleCoins);
        _basicMoneyText.SetText(_totalCoins.ToString());
        _bonusMoneyText.SetText(_doubleCoins.ToString());

        StartCounting(_totalCoins);
    }

    private void StartCounting(int targetNumber)
    {
        int currentNumber = 0;
        DOTween.To(() => currentNumber, x => currentNumber = x, targetNumber, _duration)
               .OnUpdate(() => 
               {
                   _totalMoneyText.text = currentNumber.ToString();
               })
               .OnComplete(() =>
               {
                   Debug.Log("Counting completed!");
                   // ��������� ��������� ��������
                   PlayNextAnimation();
               })
               .SetEase(Ease.OutQuad); // ������������� ���������� � �����
    }
    
    private void PlayNextAnimation()
    {
        _animation.Play("ShowLevelCompleteWindow2");
    }

    public void CollectCoins()
    {
        Debug.Log("COLLECT BASIC");
        _totalMoneyObject.SetActive(false);
        _coinCollectEffect.StartShowCoins(_totalCoins);
        DisableButtons();
    }

    public void CollectDoubleCoins()
    {
        Debug.Log("COLLECT DOUBLE");
        _totalMoneyObject.SetActive(false);
        _coinCollectEffect.StartShowCoins(_doubleCoins);
        DisableButtons();
    }

    private void DisableButtons()
    {
        _button1.enabled = false;
        _button2.enabled = false;
    }

    private void EnableButtons()
    {
        _button1.enabled = true;
        _button2.enabled = true;
    }
}
