using UnityEngine;
using UnityEngine.Events;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private WindowsEnum _currentWindow;
    [SerializeField] private GameObject[] _windows;
    private LevelCompleter _levelCompleter;

    public static UnityAction ItemsWindowOpened;
    public static UnityAction StartWindowOpened;

    private enum WindowsEnum
    {
        Start,
        Settings,
        Items,
        Upgrade,
        Game,
        BasicFinish,
        BonusFinish,
        LevelComplete
    }

    private void Awake()
    {
        OpenUpgradeWindow();
    }

    private void Start()
    {
        _levelCompleter = FindFirstObjectByType<LevelCompleter>();

        LevelManager.OnLevelChanged += OpenStartWindow;
        UiController.OnGameStarted += OpenGameWindow;
        PlayerAppearance.OnRoadPassed += OpenFinishWindow;
        PlayerAppearance.OnFinishPassedAfterCoroutine += OpenLevelCompleteWindow;     

        HideAllWindows();
        OpenStartWindow();
    }

    private void HideAllWindows()
    {
        foreach(var window in _windows)
            window.gameObject.SetActive(false);
    }

    private void OpenFinishWindow()
    {
        if (CollisionHandler.FinishType == CollisionHandler.FinishTypes.Basic)
            OpenBasicFinishWidnow();
        else if (CollisionHandler.FinishType == CollisionHandler.FinishTypes.Bonus)
            OpenBonusFinishWindow();
    }

    public void OpenStartWindow()
    {
        ChangeWindow(WindowsEnum.Start);
        StartWindowOpened?.Invoke();
    }

    public void OpenSettingsWindow()
    {
        ChangeWindow(WindowsEnum.Settings);
    }

    public void OpenItemsWindow()
    {
        ChangeWindow(WindowsEnum.Items);
        ItemsWindowOpened?.Invoke();
    }

    public void OpenUpgradeWindow()
    {
        ChangeWindow(WindowsEnum.Upgrade);
    }

    public void OpenGameWindow()
    {
        ChangeWindow(WindowsEnum.Game);
    }

    public void OpenBasicFinishWidnow()
    {
        ChangeWindow(WindowsEnum.BasicFinish);
    }

    public void OpenBonusFinishWindow()
    {
        ChangeWindow(WindowsEnum.BonusFinish);
    }

    public void OpenLevelCompleteWindow()
    {
        Debug.Log("OpenLevelCompleteWindow");
        ChangeWindow(WindowsEnum.LevelComplete);
        _levelCompleter.StartLevelComplete();
    }

    private void ChangeWindow(WindowsEnum window)
    {
        Debug.Log("KKK: " + window);
        Debug.Log("KKKK: " + (int)window);    
        Debug.Log("KKKKk: " + _windows[(int)window]);
        _windows[(int)_currentWindow]?.SetActive(false);
        _windows[(int)window].SetActive(true);

        _currentWindow = window;
    }
}
