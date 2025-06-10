using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speedMove = 5f;
    [SerializeField] private float _bonusFinishSpeed;
    [SerializeField] private float _basicFinishSpeed = 7f;
    [SerializeField, Range(0, 1)] private float _smoothTurn = 0.5f;
    [SerializeField] private float _keysControllerSensitivity;

    private PlayerAppearance _playerAppearance => GetComponentInChildren<PlayerAppearance>();

    private float _rangeX = 8.5f;
    private float _currentRangeX;
    private float _targetPositionX;

    private bool _canMove = false;
    private bool _canInput = true;

    private float _currentXOffset = 0f; // Текущее смещение куба
    private Vector2 _lastTouchPosition;

    [Header("Physics")]
    [SerializeField] private float _velocity;
    [SerializeField] private float _gravityScale;

    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private bool _isOnGround = false;

    public bool CanMove { get { return _canMove; } }
    public bool CanInput { get { return _canInput; } }

    private enum ControlType : byte
    {
        PC,
        Mobile
    }

    private void OnEnable()
    {
        UiController.OnGameStarted += StartMove;
        PlayerAppearance.OnFinishPassed += StopMove;

        CollisionHandler.OnBonusFinishEntered += BonusFinishSettings;
        CollisionHandler.OnBasicFinishEntered += BasicFinishSettings;

        _targetPositionX = transform.position.x;
    }

    private void OnDisable()
    {
        UiController.OnGameStarted -= StartMove;
        PlayerAppearance.OnFinishPassed -= StopMove;

        CollisionHandler.OnBonusFinishEntered -= BonusFinishSettings;
        CollisionHandler.OnBasicFinishEntered -= BasicFinishSettings;
    }

    private void Update()
    {
        if (!_canInput || !_canMove)
        {
            return;
        }

        _targetPositionX = GetInput();

        SetRangeByScale();
        // Ограничиваем движение по диапазону
        _targetPositionX = Mathf.Clamp(_targetPositionX, -_currentRangeX, _currentRangeX);
        _currentXOffset = Math.Clamp(_currentXOffset, -_currentRangeX, _currentRangeX);
    }

    private void LateUpdate()
    {
        if (!_canMove)
        {
            return;
        }

        ForwardMove();
        Gravity();
        GravityMove();

        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Lerp(currentPosition.x, _targetPositionX, _smoothTurn); // Плавность движения для Mobile         

        transform.position = currentPosition;
    }

    private float GetInput()
    {
        // Управление с клавиш A и D
        float keyboardInput = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
        if (keyboardInput != 0)
        {
            _currentXOffset += keyboardInput * _rangeX * GameData.Sensitivity * _keysControllerSensitivity * Time.deltaTime;
            return _currentXOffset;
        }

        //управление касанием и мышкой
        if (!Input.GetMouseButton(0) && Input.touchCount == 0) return _currentXOffset;

        Vector2 inputPosition = Input.touchCount > 0 ? (Vector2)Input.GetTouch(0).position : (Vector2)Input.mousePosition;

        // Запоминаем точку начала свайпа
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            _lastTouchPosition = inputPosition;

        // Выполняем свайп
        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            float screenScaleReference = Mathf.Min(Screen.width, Screen.height); // Нормализация по меньшей стороне
            float deltaX = (inputPosition.x - _lastTouchPosition.x) / screenScaleReference;
            _currentXOffset += deltaX * _rangeX * GameData.Sensitivity; // Чувствительность всё ещё можно вручную менять
            _lastTouchPosition = inputPosition;
        }

        return _currentXOffset;
    }

    private void SetRangeByScale()
    {
        _currentRangeX = _rangeX - transform.localScale.x;
    }

    private void ForwardMove()
    {
        transform.Translate(Vector3.forward * _speedMove * Time.deltaTime);
    }

    private void GravityMove()
    {
        transform.Translate(new Vector3(0, _velocity, 0) * Time.deltaTime);
    }

    private void Gravity()
    {
        _velocity += Physics.gravity.y * _gravityScale * Time.deltaTime;

        _isOnGround = Physics.OverlapBox(_groundChecker.position, _groundChecker.localScale / 2, Quaternion.identity, _groundLayer).Length > 0 && _velocity < 0;

        if (_isOnGround)
        {
            _velocity = 0f;
        }
    }

    public void Jump(float jumpForce)
    {
        _playerAppearance.PlaySound(PlayerAppearance.SoundsEnum.Jump);
        Debug.Log("Jump");
        if (_isOnGround)
            Debug.Log("Jump2");
        _velocity = jumpForce;
    }

    private void StartMove()
    {
        _canMove = true;
    }

    public void StopMove()
    {
        _canMove = false;
    }

    private void BonusFinishSettings()
    {
        _rangeX = 9.5f;
        _speedMove = _bonusFinishSpeed;
    }

    private void BasicFinishSettings()
    {
        _rangeX = 9.5f;

        _canInput = false;
        _targetPositionX = 0f;
        _speedMove = _basicFinishSpeed;

        Debug.Log("BasicFinish settings end");
    }
}
