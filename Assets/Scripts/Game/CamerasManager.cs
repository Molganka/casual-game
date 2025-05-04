using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    [SerializeField] private GameObject _backCamera;
    [SerializeField] private GameObject _sideCamera;

    private void OnEnable()
    { 
        CollisionHandler.OnFinish1Entered += ChangeToBackCamera;
        CollisionHandler.OnFinish2Entered += ChangeToSideCamera;
    }

    private void OnDisable()
    {
        CollisionHandler.OnFinish1Entered -= ChangeToBackCamera;
        CollisionHandler.OnFinish2Entered -= ChangeToSideCamera;
    }

    private void ChangeToBackCamera()
    {
        _backCamera.SetActive(true);
    }

    private void ChangeToSideCamera()
    {
        _sideCamera.SetActive(true);
    }  
}
