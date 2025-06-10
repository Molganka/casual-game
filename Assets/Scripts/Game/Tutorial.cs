using CrazyGames;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private CanvasGroup _PCTutorialCanvasGroup;
    [SerializeField] private CanvasGroup _mobileTutorialCanvasGroup;
    [SerializeField] private float _timeToShowTutorial;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _showTime;

    private string[] _deviceTypes = new string[]
    {
        "desktop",
        "tablet",
        "mobile"
    };

    private enum DeviceTypesEnum
    {
        Desktop,
        Tablet,
        Mobile
    }

    private void OnEnable()
    {
        UiController.OnGameStarted += TryToCallTutorial;
        LevelManager.OnLevelChanged += TryToHideStartWindowElements;
    }

    private void OnDisable()
    {
        UiController.OnGameStarted -= TryToCallTutorial;
        LevelManager.OnLevelChanged -= TryToHideStartWindowElements;
    }

    private void Start()
    {
        _PCTutorialCanvasGroup.alpha = 0f;
        _mobileTutorialCanvasGroup.alpha = 0f;
    }

    //при загрузке уровня проверяем превый ли уровень и если да то прячем некоторые элементы для тутора
    private void TryToHideStartWindowElements()
    {
        if (LevelManager.CurrentLevel == LevelManager.LevelScenes.Level1)
        {
            UiController.Instance.HideStartWindowElements();
        }
        else
        {
            UiController.Instance.ShowStartWindowElements();
        }
    }

    private void TryToCallTutorial()
    {
        if (LevelManager.CurrentLevel == LevelManager.LevelScenes.Level1)
            StartTimeToShowTutorialCoroutine();
    }

    private void StartTimeToShowTutorialCoroutine()
    {
        _PCTutorialCanvasGroup.alpha = 0;
        _mobileTutorialCanvasGroup.alpha = 0;
        StopAllCoroutines();
        StartCoroutine(TimeToShowTutorialCoroutine());
    }

    //короутина делает задержку перед показом туториала в GameWindow
    private IEnumerator TimeToShowTutorialCoroutine()
    {
        yield return new WaitForSeconds(_timeToShowTutorial);
        ShowTutorial();
    }

    private void ShowTutorial()
    {
        if (WindowManager.CurrentWindow == WindowManager.WindowsEnum.Game)
        {
            if (CrazySDK.User.SystemInfo.device.type == _deviceTypes[(int)DeviceTypesEnum.Desktop])
                StartCoroutine(ShowTutorialCoroutine(_PCTutorialCanvasGroup));
            else
                StartCoroutine(ShowTutorialCoroutine(_mobileTutorialCanvasGroup));
        }
    }

    //короутина показывает тутор определеное время
    private IEnumerator ShowTutorialCoroutine(CanvasGroup canvasGroup)
    {
        FadeIn(canvasGroup);
        yield return new WaitForSeconds(_showTime);
        FadeOut(canvasGroup);
    }

    //постепеное появление
    private void FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.OutQuad);
    }

    //постепеное исчезновение
    private void FadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.OutQuad);
    }
}
