using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusFinishWindow : MonoBehaviour
{
    public static BonusFinishWindow Instance;

    [SerializeField] private TextMeshProUGUI _gemsText;

    private Animation _animation => _gemsText.GetComponentInChildren<Animation>();
    private int _gems = 0;
    public int Gems { get { return _gems; } }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        UiController.OnGameStarted += ResetGems;
    }

    private void OnDisable()
    {
        UiController.OnGameStarted -= ResetGems;
    }

    private void Start()
    {
        UpdateGemsText();
    }

    public void AddGems(int gems = 1)
    {
        _gems += gems;
        UpdateGemsText();
    }      

    private void ResetGems()
    {
        _gems = 0;
        UpdateGemsText();
    }
    
    private void UpdateGemsText()
    {
        _gemsText.SetText($"{_gems}");
        _animation.Play();
    }
}
