using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static UnityAction OnLevelChanged;

    private UiController _uiController;

    private GameObject _loadingScreen;
    [SerializeField] private float _transitionTimeAnimation = 1f;
    private LevelScenes _currentLevelIndex;

    public enum LevelScenes
    {
        Level1 = 2,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Level10,
        Level11,
        Level12,
        Level13,
        Level14,
        Level15,
        Level16,
        Level17,
        Level18,
        Level19,
        Level20,
        Level21
    }

    private void Start()
    {
        _uiController = FindObjectOfType<UiController>();
        _loadingScreen = transform.GetChild(0).gameObject;

        _currentLevelIndex = FindAnyObjectByType<LevelData>().Level;
        _uiController.ChangeLevel((int)_currentLevelIndex-1);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadSceneCoroutine(_currentLevelIndex + 1));
    }

    public void LoadCurrentLevel()
    {
        StartCoroutine(LoadSceneCoroutine(_currentLevelIndex));
    }

    private IEnumerator LoadSceneCoroutine(LevelScenes sceneIndex)
    {
        _loadingScreen.SetActive(true);
        yield return new WaitForSeconds(_transitionTimeAnimation);
        Debug.Log("ANIMATION END. TRANSITION: " + _transitionTimeAnimation);

        // Выгрузка старого уровня

        SceneManager.UnloadSceneAsync((int)_currentLevelIndex);
        yield return new WaitUntil(() => !SceneManager.GetSceneByBuildIndex((int)_currentLevelIndex).isLoaded);

        // Удаляем PlayerScene, если она уже загружена
        if (SceneManager.GetSceneByName("PlayerScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PlayerScene");
            yield return new WaitUntil(() => !SceneManager.GetSceneByName("PlayerScene").isLoaded);
            Debug.Log("PlayerScene выгружена.");
        }

        // Загружаем новую сцену уровня и PlayerScene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive);
        AsyncOperation asyncLoadPlayer = SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone || !asyncLoadPlayer.isDone)
        {
            yield return null;
        }

        Debug.Log("SCENES LOADED");

        OnLevelChanged?.Invoke();
        _uiController.ChangeLevel((int)sceneIndex - 1);

        _currentLevelIndex = sceneIndex;
        Debug.Log("kkk: " + _currentLevelIndex);
        _loadingScreen.SetActive(false);
    }


}
