using System.Collections;
using System.Collections.Generic;
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
        Shop,
        Game,
        LevelComplete
    }

    private void Start()
    {
        _levelCompleter = FindFirstObjectByType<LevelCompleter>();

        LevelManager.OnLevelChanged += OpenStartWindow;
        UiController.OnGameStarted += OpenGameWindow;
        PlayerAppearance.OnLevelFinished += OpenLevelCompleteWindow;

        HideAllWindows();
        OpenStartWindow();
    }

    private void HideAllWindows()
    {
        foreach(var window in _windows)
            window.gameObject.SetActive(false);
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

    public void OpenShopWindow()
    {
        ChangeWindow(WindowsEnum.Shop);
    }

    public void OpenGameWindow()
    {
        ChangeWindow(WindowsEnum.Game);
    }

    public void OpenLevelCompleteWindow()
    {
        Debug.Log("OpenLevelCompleteWindow");
        ChangeWindow(WindowsEnum.LevelComplete);
        _levelCompleter.StartLevelComplete();
    }

    private void ChangeWindow(WindowsEnum window)
    {
        _windows[(int)_currentWindow]?.SetActive(false);
        _windows[(int)window].SetActive(true);
        _currentWindow = window;
    }
}
