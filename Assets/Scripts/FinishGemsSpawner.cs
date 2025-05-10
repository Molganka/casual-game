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

    [SerializeField] private float _repeatZ = 1f;

    private void Start()
    {
        for(float z = -_spawnZRange; z < _spawnZRange; z += _repeatZ)
        {
            GameObject coin = Instantiate(_gemPrefab, transform.TransformPoint(new Vector3(Random.Range(-_spawnXRange, _spawnXRange), _spawnYPosition, z)), transform.rotation);
            coin.transform.parent = transform;
        }
    }
}
