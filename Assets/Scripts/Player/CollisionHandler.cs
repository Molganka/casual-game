using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CollisionHandler : MonoBehaviour
{
    private PlayerAppearance _playerAppearance;
    private PlayerController _playerController;

    public static UnityAction<GameObject, BlockData> OnBlockTrigged;
    public static UnityAction<PortalData> OnPortalEntered;
    public static UnityAction<EnemyCube> OnCubeTrgged;
    public static UnityAction OnBonusFinishEntered;
    public static UnityAction OnBasicFinishEntered;

    private bool _onCheckDown = false;
    private float _rayLength = 3f;
    private string _targetTag = "Multiplier";
    private FinishBlockData _lastFinishBlockData;

    public static FinishTypes FinishType { get; private set; }
    public static float Multiplier { get; private set; }

    public enum FinishTypes
    {
        Basic,
        Bonus
    }

    private void Start()
    {
        _playerAppearance = GetComponentInChildren<PlayerAppearance>();
        _playerController = GetComponent<PlayerController>();

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
                        BasicFinishWindow.AddMoney();
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
            UiController.Instance.AddMoney();
        }
        else if(other.CompareTag("Gem"))
        {
            other.gameObject.GetComponentInParent<Gem>().GetGem();
            BonusFinishWindow.Instance.AddGems();
        }
        else if (other.CompareTag("Jump"))
        {
            _playerController.Jump(other.gameObject.GetComponent<SpringPlatfomData>().SpringForce);
            other.gameObject.GetComponentInChildren<Animation>().Play();
        }
        else if (other.CompareTag("FinishBasic"))
        {
            _onCheckDown = true;
            FinishType = FinishTypes.Basic;
            OnBasicFinishEntered?.Invoke();
              
        }
        else if (other.CompareTag("FinishBonus"))
        {          
            FinishType = FinishTypes.Bonus;
            OnBonusFinishEntered?.Invoke();          
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
