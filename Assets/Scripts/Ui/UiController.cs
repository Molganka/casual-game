using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject _startWindow;
    [SerializeField] private GameObject _itemsWindow;
    [SerializeField] private TextMeshProUGUI[] _moneyTextes;
    [SerializeField] private GameObject[] _startWindowElementsToHide;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private int _startMoney;

    public static UiController Instance;

    public static UnityAction OnGameStarted;

    private int _money; 
    public int Money { get { return _money; } set { _money = value; } }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        LevelManager.OnLevelChanged += SaveMoney;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelChanged -= SaveMoney;
    }

    private void Start()
    {
        Money = SaveManager.LoadMoney(_startMoney);

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

    public void SaveMoney() => SaveManager.SaveMoney(Money);

    public void HideStartWindowElements()
    {
        foreach(var element in _startWindowElementsToHide) 
            element.gameObject.SetActive(false);
    }

    public void ShowStartWindowElements()
    {
        foreach(var element in _startWindowElementsToHide)
            element.gameObject.SetActive(true);
    }
}
