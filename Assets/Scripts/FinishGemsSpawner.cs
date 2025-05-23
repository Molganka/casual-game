using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FinishGemsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gemPrefab;

    [SerializeField] private int _spawnXRange;
    [SerializeField] private float _spawnYPosition;
    [SerializeField] private float _spawnZRange;
    [SerializeField] private int _zDifference;

    private int _gemPrice = 5;

    private void Awake()
    {
        GameData.GemCost = _gemPrice;
    }

    private void OnEnable()
    {
        UiController.OnGameStarted += SpawnGems;
    }

    private void OnDisable()
    {
        UiController.OnGameStarted -= SpawnGems;
    }

    private void SpawnGems()
    {
        int pastX = 99;
        for (float z = -_spawnZRange; z < _spawnZRange; z += GameData.GemSpawnRepeat)
        {
            pastX = GetRandomExcept(-_spawnXRange, _spawnXRange+1, pastX, _zDifference);
            GameObject gem = Instantiate(_gemPrefab, transform.TransformPoint(new Vector3(pastX, _spawnYPosition, z)), transform.rotation);
            gem.transform.parent = transform;
        }
    }

    private int GetRandomExcept(int minValue, int maxValue, int pastValue, int difference)
    {
        List<int> validValues = new List<int>();

        for(int i = minValue; i <= maxValue; i++)
        {
            if(i > pastValue+difference || i < pastValue-difference)
                validValues.Add(i);
        }

        return validValues[Random.Range(0, validValues.Count-1)];
    }
}
