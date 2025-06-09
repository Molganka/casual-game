using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animation _animation => GetComponent<Animation>();
    private BoxCollider _boxCollider => GetComponent<BoxCollider>();
    private AudioSource _audioSource => GetComponent<AudioSource>();

    //Call in animation event
    public void DestroyCoin() => Destroy(gameObject);

    public void GetCoin()
    {
        _boxCollider.enabled = false;
        _animation.Play("CoinCollect");
        if(GameData.OnGameSound) _audioSource.Play();
    }
}

