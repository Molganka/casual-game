using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSettings : MonoBehaviour
{
    private string _animationName = "MoveBladeSpin";

    [SerializeField] private Animation _animation;
    [SerializeField] private float _startValue; 

    private void Start()
    {
        _startValue = Mathf.Clamp(_startValue, 0f, _animation[_animationName].length);
        _animation[_animationName].time = _startValue;
        _animation.Play();
    }
}
