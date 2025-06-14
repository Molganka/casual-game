using UnityEngine;
using UnityEngine.Events;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private Window[] _windows;

    public static WindowsEnum CurrentWindow;
    
    private LevelCompleter _levelCompleter;

    public static UnityAction ItemsWindowOpened;
    public static UnityAction StartWindowOpened;

    public enum WindowsEnum
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
        for(int i = 1; i < _windows.Length; ++i)
        {
            _windows[i].Hide();
        }           
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
        ChangeWindow(WindowsEnum.LevelComplete);
        _levelCompleter.StartLevelComplete();
    }

    private void ChangeWindow(WindowsEnum window)
    {
        _windows[(int)CurrentWindow].Hide();
        _windows[(int)window].Show();

        CurrentWindow = window;
    }
}
