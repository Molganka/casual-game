using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private LevelManager.LevelScenes _level;
    public LevelManager.LevelScenes Level { get { return _level; } private set { _level = value; } }
}
