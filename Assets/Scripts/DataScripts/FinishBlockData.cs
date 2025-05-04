using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishBlockData : MonoBehaviour
{
    [SerializeField] private Material _newMaterial;
    private MeshRenderer _meshRenderer;

    public float Multiplier;
    private TextMeshPro _text;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _text = GetComponentInChildren<TextMeshPro>();
        _text.SetText($"×{Multiplier}");
    }

    public void Activate()
    {
        GetComponent<Animation>().Play();
        SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Block);
        _meshRenderer.material = _newMaterial;
    }
}
