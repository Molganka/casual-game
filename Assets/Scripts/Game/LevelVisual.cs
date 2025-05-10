using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisual : MonoBehaviour
{
    [SerializeField] private Color[] _backgroundColors;
    [SerializeField] private Camera _camera;

    private void Start()
    {
        _camera.backgroundColor = _backgroundColors[Random.Range(0, _backgroundColors.Length)];
    }
}
