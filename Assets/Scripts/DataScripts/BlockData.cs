using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData : MonoBehaviour
{
    public Material Color;
    public int Cost;

    private GameObject _childBlock1;

    private void Start()
    {
        _childBlock1 = transform.GetChild(0).gameObject;
        _childBlock1.SetActive(true);
    }

    public void ChangeColorBlock()
    {
        _childBlock1.SetActive(false);
    }
}
