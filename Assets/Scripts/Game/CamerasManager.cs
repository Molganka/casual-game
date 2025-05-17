using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    [SerializeField] private GameObject _backCamera;
    [SerializeField] private GameObject _sideCamera;

    private void OnEnable()
    { 
        CollisionHandler.OnBonusFinishEntered += ChangeToBackCamera;
        CollisionHandler.OnBasicFinishEntered += ChangeToSideCamera;
    }

    private void OnDisable()
    {
        CollisionHandler.OnBonusFinishEntered -= ChangeToBackCamera;
        CollisionHandler.OnBasicFinishEntered -= ChangeToSideCamera;
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
