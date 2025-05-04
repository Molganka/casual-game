using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class CubeBasic : MonoBehaviour
{
    [SerializeField] protected float _increaseScale = 0.02f;
    [SerializeField] protected float _decreaseScale = 0.02f;
    [SerializeField] protected float _maxScoreToIncrease = 50f;
    [SerializeField] protected float _totalDecreaseTime = 0.2f;
    [SerializeField] protected int _startScore;

    [SerializeField] protected Material _startColor;
    [SerializeField] protected Material _basicColor;
    protected Material _currentColor;

    [Header("Score")]

    protected int _score;   

    protected bool _isAnimationPlaying = false;
    public bool OnDecrease = false;

    protected Transform _transformParent;
    protected TextMeshPro _scoreText;
    protected Renderer _renderer; 

    public int Score { get { return _score; } }
    public Material CurrentColor { get { return _currentColor; } }
    
    protected virtual void Awake()
    {
        //some code
    }

    protected virtual void Start()
    {
        Debug.Log("START BASE CALLED");
        _transformParent = transform.parent;
        _scoreText = GetComponentInChildren<TextMeshPro>();
        _renderer = GetComponent<Renderer>();

        SetCubeScale();
    }

    protected virtual void Update()
    {
        UpdateScoreText();
    }

    protected void SetCubeScale()
    {
        IncreaseCube(_startScore);
    }

    public virtual void IncreaseCube(int increase = 1)
    {
        float finalScale = 0;
        int finalScore = _score + increase; //40, 20 (max: 50)

        if (finalScore <= _maxScoreToIncrease)
        { 
            finalScale = increase * _increaseScale;
        }
        else
        {
            if (_score < _maxScoreToIncrease)
            {
                finalScale = (_maxScoreToIncrease - _score) * _increaseScale; //40, 60
            }
        }

        _transformParent.localScale += new Vector3(finalScale, finalScale, finalScale);
        _transformParent.position = new Vector3(_transformParent.position.x, _transformParent.position.y + finalScale, _transformParent.position.z);

        _score = finalScore; 
    }

    public virtual bool DecreaseCube(int decrease = 1)
    {
        float finalScale;
        int finalScore = _score - decrease;  //     

        if(_score < _maxScoreToIncrease)
        {
            finalScale = decrease;
        }
        else if(finalScore < _maxScoreToIncrease)
        {
            finalScale = _maxScoreToIncrease - finalScore;
        }
        else
        {
            _score = finalScore;
            return false;
        }

        finalScale *= _increaseScale;
        _transformParent.localScale -= new Vector3(finalScale, finalScale, finalScale);
        _transformParent.position = new Vector3(_transformParent.position.x, _transformParent.position.y - finalScale, _transformParent.position.z);

        if(finalScore > 0)
        {
            _score = finalScore;
            return false;
        }
        else
        {
            _score = 0;
            return true;
        }
        
    }

    protected void UpdateScoreText()
    {
        _scoreText.SetText($"{_score}");
    }
}
