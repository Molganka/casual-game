using UnityEngine;

public class FinishBlockData : MonoBehaviour
{
    [SerializeField] private Material _avtivateMaterial;
    private MeshRenderer _meshRenderer => GetComponent<MeshRenderer>();

    public void Activate()
    {
        GetComponent<Animation>().Play();
        SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Block);
        _meshRenderer.material = _avtivateMaterial;
    }
}
