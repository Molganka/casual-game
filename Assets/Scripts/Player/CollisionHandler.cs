using UnityEngine;
using UnityEngine.Events;

public class CollisionHandler : MonoBehaviour
{
    private PlayerAppearance _playerAppearance;
    private PlayerController _playerController;

    public static UnityAction<GameObject, BlockData> OnBlockTrigged;
    public static UnityAction<PortalData> OnPortalEntered;
    public static UnityAction<EnemyCube> OnCubeTrgged;
    public static UnityAction OnFinish1Entered;
    public static UnityAction OnFinish2Entered;

    private bool _onCheckDown = false;
    private float _rayLength = 3f;
    private string _targetTag = "Multiplier";
    private FinishBlockData _lastFinishBlockData;
    private UiController _uiController;
    private LevelCompleter _levelCompleter;

    public static byte FinishType { get; private set; }
    public static float Multiplier { get; private set; }

    private void Awake()
    {
        FinishType = 0;
    }

    private void Start()
    {
        _playerAppearance = GetComponentInChildren<PlayerAppearance>();
        _playerController = GetComponent<PlayerController>();
        _uiController = FindFirstObjectByType<UiController>();
        _levelCompleter = FindFirstObjectByType<LevelCompleter>();

        PlayerAppearance.OnFinishPassed += StopCheckDown;
    }

    private void Update()
    {
        if (_onCheckDown)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _rayLength))
            {
                if (hit.collider.CompareTag(_targetTag))
                {
                    FinishBlockData currentFinishBlockData = hit.collider.gameObject.GetComponent<FinishBlockData>();

                    if (currentFinishBlockData != _lastFinishBlockData)
                    {
                        Multiplier = currentFinishBlockData.Multiplier;
                        currentFinishBlockData.Activate();
                    }

                    _lastFinishBlockData = currentFinishBlockData;
                }
            }

            // Для визуализации луча в редакторе
            Debug.DrawRay(transform.position, Vector3.down * _rayLength, Color.red);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_playerController.CanMove) { return; }

        if (other.TryGetComponent<BlockData>(out BlockData blockData))
        {
            OnBlockTrigged?.Invoke(blockData.gameObject, blockData);
        }
        else if (other.CompareTag("Portal"))
        {
            PortalData portalData = other.gameObject.GetComponentInParent<PortalData>();

            OnPortalEntered?.Invoke(portalData);
        }
        else if (other.CompareTag("Cube"))
        {
            Debug.Log("Cube touch");
            EnemyCube enemyCube = other.GetComponentInChildren<EnemyCube>();
           
            OnCubeTrgged?.Invoke(enemyCube);
        }
        else if (other.CompareTag("Spike"))
        {
            _playerAppearance.QuickDecrease();
        }
        else if (other.TryGetComponent<Coin>(out Coin coin))
        {
            coin.GetCoin();
            //если сейчас не находится на финише
            if (FinishType == 0)
            {
                Debug.Log("FinishType 0 call");
                _uiController.AddMoney();          
            }
            else if(FinishType == 1)
            {
                Debug.Log("FinishType 1 call");
                _levelCompleter.AddCoins();
            }       
        }
        else if (other.CompareTag("Jump"))
        {
            _playerController.Jump(other.gameObject.GetComponent<SpringPlatfomData>().SpringForce);
            other.gameObject.GetComponentInChildren<Animation>().Play();
        }
        else if (other.CompareTag("Finish1"))
        {
            FinishType = 1;
            OnFinish1Entered?.Invoke();   
        }
        else if (other.CompareTag("Finish2"))
        { 
            FinishType = 2;
            OnFinish2Entered?.Invoke();
            _onCheckDown = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_playerController.CanMove) { return; }

        if (other.gameObject.CompareTag("Blade"))
        {
            if (!_playerAppearance.OnDecrease)
            {
                Debug.Log("OnDecrease true");
                _playerAppearance.OnDecrease = true;
                _playerAppearance.StartDecreaseCoroutines();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_playerController.CanMove) { return; }

        if (other.gameObject.CompareTag("Blade"))
        {
            Debug.Log("OnDecrease false");
            _playerAppearance.OnDecrease = false;
        }
    }

    private void StopCheckDown()
    {
        _onCheckDown = false;
    }
}
