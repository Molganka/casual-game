using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static UnityAction OnLevelChanged;

    [SerializeField] private float _transitionTimeAnimation = 1f;
    private int _randomLevelCount;

    private UiController _uiController;
    private GameObject _loadingScreen;
    
    private LevelScenes _currentLevel;
    private LevelScenes _lastRandomLevel;
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

        _currentLevel = FindAnyObjectByType<LevelData>().Level;
        _uiController.ChangeLevel((int)_currentLevel-1);
    }

    public void LoadNextLevel()
    {
        if (_currentLevel == LevelScenes.Level20 && !_onRandomLevels)
        {
            _onRandomLevels = true;
            _randomLevelCount = 20;
        }

        if (_onRandomLevels)
        {
            _lastRandomLevel = _currentLevel;
            LevelScenes randomLevel = _currentLevel;
            
            while(randomLevel == _currentLevel)
            {
                randomLevel = (LevelScenes)UnityEngine.Random.Range((int)_minRandomLevel, (int)_maxRandomLevel + 1);
            }

            StartCoroutine(LoadSceneCoroutine(randomLevel));
        }
        else
            StartCoroutine(LoadSceneCoroutine(_currentLevel + 1));
    }

    public void LoadCurrentLevel()
    {
        StartCoroutine(LoadSceneCoroutine(_currentLevel));
    }

    private IEnumerator LoadSceneCoroutine(LevelScenes sceneIndex)
    {
        _loadingScreen.SetActive(true);
        yield return new WaitForSeconds(_transitionTimeAnimation);
        Debug.Log("ANIMATION END. TRANSITION: " + _transitionTimeAnimation);

        // �������� ������� ������

        SceneManager.UnloadSceneAsync((int)_currentLevel);
        yield return new WaitUntil(() => !SceneManager.GetSceneByBuildIndex((int)_currentLevel).isLoaded);

        // ������� PlayerScene, ���� ��� ��� ���������
        if (SceneManager.GetSceneByName("PlayerScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PlayerScene");
            yield return new WaitUntil(() => !SceneManager.GetSceneByName("PlayerScene").isLoaded);
            Debug.Log("PlayerScene ���������.");
        }

        // ��������� ����� ����� ������ � PlayerScene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive);
        AsyncOperation asyncLoadPlayer = SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone || !asyncLoadPlayer.isDone)
        {
            yield return null;
        }

        Debug.Log("SCENES LOADED");

        OnLevelChanged?.Invoke();
        if (_onRandomLevels)
        {
            if(sceneIndex != _currentLevel)
                _uiController.ChangeLevel(++_randomLevelCount);
        }
        else
            _uiController.ChangeLevel((int)sceneIndex - 1);

        _currentLevel = sceneIndex;
        Debug.Log("kkk: " + _currentLevel);
        _loadingScreen.SetActive(false);
    }
}
