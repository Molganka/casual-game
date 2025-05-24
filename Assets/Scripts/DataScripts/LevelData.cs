using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private int _level;
    public int Level { get { return _level; } private set { _level = value; } }
}
