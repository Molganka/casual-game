using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smooth;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _xRange = 2f;

    private Vector3 _currentPosition;
    private float _goalXPosition;

    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, _goalXPosition, _smooth * Time.deltaTime), _player.transform.position.y, _player.transform.position.z) + _offset;
    }

    private void Update()
    {
        if (_player.transform.position.x > _xRange)
        {
            _goalXPosition = _xRange;
        }
        else if (_player.transform.position.x < -_xRange)
        {
            _goalXPosition = -_xRange;
        }
        else
        {
            _goalXPosition = _player.transform.position.x;
        }
    }
}
