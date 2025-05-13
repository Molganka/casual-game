using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FinishGemsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gemPrefab;

    [SerializeField] private int _spawnXRange;
    [SerializeField] private float _spawnYPosition;
    [SerializeField] private float _spawnZRange;    

    private int _gemPrice = 3;

    private void Awake()
    {
        GameData.GemCost = _gemPrice;
    }

    private void Start()
    {
        UiController.OnGameStarted += SpawnGems;
    }

    private void SpawnGems()
    {
        for (float z = -_spawnZRange; z < _spawnZRange; z += GameData.GemSpawnRepeat)
        {
            GameObject coin = Instantiate(_gemPrefab, transform.TransformPoint(new Vector3(Random.Range(-_spawnXRange, _spawnXRange), _spawnYPosition, z)), transform.rotation);
            coin.transform.parent = transform;
        }
    }
}
