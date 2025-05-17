using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFinish : MonoBehaviour
{
    private FinishBlockData[] _blocks => GetComponentsInChildren<FinishBlockData>();

    private void Start()
    {
        float var = 1.0f;
        foreach (FinishBlockData block in _blocks)
        {
            block.SetMultiplierText(var);
            var += 0.1f;
        }
    }
}
