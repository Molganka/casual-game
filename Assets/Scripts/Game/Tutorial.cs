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

    //��� �������� ������ ��������� ������ �� ������� � ���� �� �� ������ ��������� �������� ��� ������
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

    //��������� ������ �������� ����� ������� ��������� � GameWindow
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

    //��������� ���������� ����� ����������� �����
    private IEnumerator ShowTutorialCoroutine(CanvasGroup canvasGroup)
    {
        FadeIn(canvasGroup);
        yield return new WaitForSeconds(_showTime);
        FadeOut(canvasGroup);
    }

    //���������� ���������
    private void FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.OutQuad);
    }

    //���������� ������������
    private void FadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.OutQuad);
    }
}
