using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishBlockData : MonoBehaviour
{
    [SerializeField] private Material _newMaterial;
    private MeshRenderer _meshRenderer;
    private TextMeshPro _multiplierText;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _multiplierText = GetComponentInChildren<TextMeshPro>();
    }

    public void Activate()
    {
        GetComponent<Animation>().Play();
        SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Block);
        _meshRenderer.material = _newMaterial;
    }

    public void SetMultiplierText(float multiplier)
    {
        _multiplierText.SetText($"x{multiplier}");
    }
}
