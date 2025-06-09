using UnityEngine;

public class Gem : MonoBehaviour
{
    private Animation _animation => GetComponent<Animation>();
    private BoxCollider _boxCollider => GetComponentInChildren<BoxCollider>();
    private AudioSource _audioSource => GetComponent<AudioSource>();

    public void DestroyGem() => Destroy(gameObject);

    public void GetGem()
    {
        _boxCollider.enabled = false;
        _animation.Play("GemCollect");
        _audioSource.Play();
    }
}
