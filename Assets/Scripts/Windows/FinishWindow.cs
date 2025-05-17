using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishWindow : MonoBehaviour
{
    public static FinishWindow Instance;

    [SerializeField] private TextMeshProUGUI _gemsText;

    private Animation _animation => GetComponentInChildren<Animation>();
    private int _gems = 0;
    public int Gems { get { return _gems; } }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
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
    
    private void UpdateGemsText()
    {
        _gemsText.SetText($"{_gems}");
        _animation.Play();
    }
}
