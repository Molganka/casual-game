using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animation _animation => GetComponent<Animation>();
    private BoxCollider _boxCollider => GetComponent<BoxCollider>();
    private AudioSource _audioSource => GetComponent<AudioSource>();   

    //Call in animation event
    public void HideCoin() => transform.parent.gameObject.SetActive(false);

    public void GetCoin()
    {
        _boxCollider.enabled = false;
        _animation.Play("CoinCollect");
        _audioSource.Play();
    }
}

