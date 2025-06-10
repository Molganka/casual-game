using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAppearance : CubeBasic
{
    [SerializeField] private float _multiplierDecreaseTime;
    [SerializeField] private int _quickDecrease = 10;
    [SerializeField] private float _decreaseTime;
    [SerializeField] private float _finishLevelTime = 1f;

    [SerializeField] private GameObject _confeti;
    [SerializeField] private GameObject _scoreObject;
    [SerializeField] private Transform _confetiParent;
    [SerializeField] private Transform _itemsPlace;

    [Header("Sound")]
    [SerializeField] private SoundData[] _sounds;
    [SerializeField] private AudioSource _audioSource;

    [Header("PitchSettings")]
    [SerializeField] private float _startPitch = 0.85f;
    [SerializeField] private float _maxtPitch = 1.2f;
    [SerializeField] private float _increasePitch = 0.05f;
    [SerializeField] private float _pitchStrikeResetTime = 1f;
    private float _currentPitch;
    private bool _onPitchStrikeGo;
    private Coroutine _lastPitchCoroutine;
    private Coroutine _decreaseCoroutine;

    private bool _canTransformation = true;
    private bool _isThisStart = true;
    private bool _onFinishGo = false;

    public static UnityAction OnRoadPassed;
    public static UnityAction OnFinishPassed;
    public static UnityAction OnFinishPassedAfterCoroutine;

    private Animation _animation;

    private PlayerController _playerController;
    private LevelManager _sceneLoader;

    public enum SoundsEnum
    {
        Collect,
        Damage,
        Transformation,
        Jump
    }

    private enum Layers
    {
        Default,
        UICamera
    }

    private void OnEnable()
    {
        CollisionHandler.OnPortalEntered += ChangeColorByPortal;
        CollisionHandler.OnBlockTrigged += BlockCollect;
        CollisionHandler.OnCubeTrgged += CubeAtack;

        CollisionHandler.OnBonusFinishEntered += StartFinishDecreaseCoroutine;
        CollisionHandler.OnBasicFinishEntered += StartFinishDecreaseCoroutine;

        WindowManager.ItemsWindowOpened += ChangeLayerToUICamera;
        WindowManager.StartWindowOpened += ChangeLayerToDefault;

        OnFinishPassed += SpawnConfeti;
    }

    private void OnDisable()
    {
        CollisionHandler.OnPortalEntered -= ChangeColorByPortal;
        CollisionHandler.OnBlockTrigged -= BlockCollect;
        CollisionHandler.OnCubeTrgged -= CubeAtack;

        CollisionHandler.OnBonusFinishEntered -= StartFinishDecreaseCoroutine;
        CollisionHandler.OnBasicFinishEntered -= StartFinishDecreaseCoroutine;

        WindowManager.ItemsWindowOpened -= ChangeLayerToUICamera;
        WindowManager.StartWindowOpened -= ChangeLayerToDefault;

        OnFinishPassed -= SpawnConfeti;
    }

    protected override void Awake()
    {
        base.Awake();

        _currentPitch = _startPitch;
    }

    protected override void Start()
    {
        base.Start();

        _animation = GetComponent<Animation>();
        _scoreText = GetComponentInChildren<TextMeshPro>();
        _playerController = GetComponentInParent<PlayerController>();
        _sceneLoader = FindFirstObjectByType<LevelManager>();

        ChangeColor(_startColor);
        DecreaseCube();
        _isThisStart = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void BlockCollect(GameObject block, BlockData blockData)
    {
        _animation.Stop();
        Material blockMaterial = blockData.Color;
        if (blockMaterial != _basicColor)
        {
            if (_currentColor == blockMaterial)
            {
                IncreaseCube();
            }
            else if (_currentColor == _basicColor)
            {
                ChangeColor(blockData.Color);
                IncreaseCube();
            }
            else
            {
                DecreaseCube();
            }
            blockData.ChangeColorBlock();
        }
    }

    private void CubeAtack(EnemyCube enemyCube)
    {
        if (!_canTransformation) { return; }

        if (_score >= enemyCube.Score)
        {
            IncreaseCube(enemyCube.Score);
            ChangeColor(enemyCube.CurrentColor);
            enemyCube.EnemyDead();
        }
        else
        {
            PlayerDead();
        }
    }

    public void QuickDecrease()
    {
        DecreaseCube(_quickDecrease);
    }

    public void StartDecreaseCoroutine()
    {
        _decreaseCoroutine = StartCoroutine(DecreaseCoroutine());
    }

    public void StopDecreaseCoroutine()
    {
        if (_decreaseCoroutine != null)
            StopCoroutine(_decreaseCoroutine);
        _decreaseCoroutine = null;
    }

    private IEnumerator DecreaseCoroutine()
    {
        if (!DecreaseCube())
        {
            yield return new WaitForSeconds(_decreaseTime);
            _decreaseCoroutine = StartCoroutine(DecreaseCoroutine());
        }
        else
        {
            PlayerDead();
        }
    }

    private IEnumerator FinishDecreaseCoroutine()
    {
        if (!DecreaseCube())
        {
            yield return new WaitForSeconds(_multiplierDecreaseTime);
            StartCoroutine(FinishDecreaseCoroutine());
        }
        else
        {
            FinishPassed();
        }
    }

    public void FinishPassed()
    {
        StopAllCoroutines();
        OnFinishPassed?.Invoke();
        StartCoroutine(OnLevelFinishCoroutine());
    }

    private IEnumerator OnLevelFinishCoroutine()
    {
        yield return new WaitForSeconds(_finishLevelTime);
        OnFinishPassedAfterCoroutine?.Invoke();
    }

    private void StartFinishDecreaseCoroutine()
    {
        _onFinishGo = true;
        OnRoadPassed?.Invoke();
        StartCoroutine(FinishDecreaseCoroutine());
    }

    private void ChangeColorByPortal(PortalData portalData)
    {
        if (!_canTransformation) { return; }

        _animation.Play("Transformation");
        PlaySound(SoundsEnum.Transformation);
        ChangeColor(portalData.Color);
    }

    private void ChangeColor(Material color)
    {
        _renderer.material = color;
        _currentColor = color;
        _isAnimationPlaying = false;
    }

    public override void IncreaseCube(int increase = 1)
    {
        base.IncreaseCube(increase);

        if (!_isThisStart)
        {
            _animation.Stop();
            _animation.Play("Increase");
            IncreaseCollectPitch();
            PlaySound(SoundsEnum.Collect, _currentPitch);
        }
    }

    public override bool DecreaseCube(int decrease = 1)
    {
        if (!_isThisStart)
        {
            if (base.DecreaseCube(decrease))
            {
                if (!_onFinishGo)
                    PlayerDead();
                return true;
            }

            if (!_onFinishGo)
            {
                _animation.Stop();
                _animation.Play("Damage");
                PlaySound(SoundsEnum.Damage);
                _onPitchStrikeGo = false;
            }
            return false;
        }
        return false;
    }

    public void PlaySound(SoundsEnum sound, float pitch = 1f)
    {
        if (!GameData.OnGameSound) return;
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(_sounds[(int)sound].audioClip, _sounds[(int)sound].volume);
    }

    private void IncreaseCollectPitch()
    {
        if (_onPitchStrikeGo)
        {
            _currentPitch += _increasePitch;
            if (_currentPitch > _maxtPitch)
                _currentPitch = _maxtPitch;
            StopCoroutine(_lastPitchCoroutine);
        }
        else
        {
            _currentPitch = _startPitch;
        }

        _lastPitchCoroutine = StartCoroutine(StrikePitchCoroutine());
    }

    private IEnumerator StrikePitchCoroutine()
    {
        _onPitchStrikeGo = true;
        yield return new WaitForSeconds(_pitchStrikeResetTime);
        _onPitchStrikeGo = false;
    }

    private void SpawnConfeti()
    {
        if (CollisionHandler.FinishType == CollisionHandler.FinishTypes.Basic)
            Instantiate(_confeti, _confetiParent);
    }

    private void ChangeLayer(Layers layer)
    {
        int goalLayer = LayerMask.NameToLayer("Default");
        if (layer == Layers.Default)
            goalLayer = LayerMask.NameToLayer("Default");
        else if (layer == Layers.UICamera)
            goalLayer = LayerMask.NameToLayer("UI Camera");

        gameObject.layer = goalLayer;

        if (_itemsPlace.childCount > 0)
        {
            foreach (Transform child in _itemsPlace)
            {
                child.gameObject.layer = goalLayer;
                child.GetChild(0).gameObject.layer = goalLayer;
            }
        }
    }

    private void ChangeLayerToDefault() => ChangeLayer(Layers.Default);
    private void ChangeLayerToUICamera() => ChangeLayer(Layers.UICamera);

    public void RestartLevel()
    {
        _sceneLoader.LoadCurrentLevel();
    }

    public void SetCanTransformationTrue() => _canTransformation = true;
    public void SetCanTransformationFalse() => _canTransformation = false;

    private void PlayerDead()
    {
        StopAllCoroutines();
        _score = 0;
        ChangeColor(_basicColor);
        _playerController.StopMove();

        _animation.Stop();
        _animation["Dead"].time = 0;
        _animation.Play("Dead");
    }

    [System.Serializable]
    public struct SoundData
    {
        public AudioClip audioClip;
        public float volume;
    }
}
