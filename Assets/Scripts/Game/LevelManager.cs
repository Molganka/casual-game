using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static UnityAction OnLevelChanged;

    public static LevelScenes CurrentLevel;

    [SerializeField] private LevelScenes _startLevel;
    [SerializeField] private float _transitionTimeAnimation = 1f;
    private int _randomLevelCount;

    private UiController _uiController;
    private GameObject _loadingScreen;
    private GameObject _startLoadingScreen;

    private LevelScenes _minRandomLevel = LevelScenes.Level10;
    private LevelScenes _maxRandomLevel = LevelScenes.Level20;
    private bool _onRandomLevels = false;

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
        Level20
    }

    private void Start()
    {       
        _uiController = FindObjectOfType<UiController>();
        _loadingScreen = transform.GetChild(0).gameObject;
        _startLoadingScreen = transform.GetChild(1).gameObject;

        StartCoroutine(LoadSceneCoroutine(SaveManager.LoadLevel(_startLevel), true));
    }

    public void LoadNextLevel()
    {
        if (CurrentLevel == LevelScenes.Level20 && !_onRandomLevels)
        {
            _onRandomLevels = true;
            _randomLevelCount = 20;
        }

        if (_onRandomLevels)
        {
            LevelScenes randomLevel = CurrentLevel;
            
            while(randomLevel == CurrentLevel)
            {
                randomLevel = (LevelScenes)UnityEngine.Random.Range((int)_minRandomLevel, (int)_maxRandomLevel + 1);
            }

            StartCoroutine(LoadSceneCoroutine(randomLevel, false));
        }
        else
            StartCoroutine(LoadSceneCoroutine(CurrentLevel + 1, false));
    }

    public void LoadCurrentLevel()
    {
        StartCoroutine(LoadSceneCoroutine(CurrentLevel, false));
    }

    private IEnumerator LoadSceneCoroutine(LevelScenes sceneIndex, bool isStartLoad)
    {
        if (isStartLoad)
        {
            _startLoadingScreen.SetActive(true);
        }
        else
        {
            _loadingScreen.SetActive(true);
            yield return new WaitForSeconds(_transitionTimeAnimation);
        }

        // Выгрузка старого уровня

        if (!isStartLoad)
        {
            SceneManager.UnloadSceneAsync((int)CurrentLevel);
            yield return new WaitUntil(() => !SceneManager.GetSceneByBuildIndex((int)CurrentLevel).isLoaded);
        }

        // Удаляем PlayerScene, если она уже загружена
        if (SceneManager.GetSceneByName("PlayerScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PlayerScene");
            yield return new WaitUntil(() => !SceneManager.GetSceneByName("PlayerScene").isLoaded);
        }

        // Загружаем новую сцену уровня и PlayerScene
        Debug.Log("d: " + sceneIndex);
        Debug.Log("d:d " + (int)sceneIndex);
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive);
        AsyncOperation asyncLoadPlayer = SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone || !asyncLoadPlayer.isDone)
        {
            yield return null;
        }
        Debug.Log("COMPLETE2");

        if (_onRandomLevels)
        {
            if(sceneIndex != CurrentLevel)
                _uiController.ChangeLevel(++_randomLevelCount);
        }
        else
            _uiController.ChangeLevel((int)sceneIndex - 1);

        CurrentLevel = sceneIndex;
        _startLoadingScreen.SetActive(false);
        _loadingScreen.SetActive(false);
        SaveManager.SaveLevel(sceneIndex);
        
        OnLevelChanged?.Invoke();
    }
}
