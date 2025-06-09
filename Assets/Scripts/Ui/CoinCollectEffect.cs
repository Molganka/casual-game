using DG.Tweening;
using UnityEngine;

public class CoinCollectEffect : MonoBehaviour
{
    private UiController _uiController;
    private LevelManager _levelManager;

    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform _coinParent;
    [SerializeField] private Transform _startCoinTransform;
    [SerializeField] private Transform _endCoinTransform;
    [SerializeField] private float _moveDuration;
    [SerializeField] private Ease _moveEase;

    private float _coinPerDelay;
    private float _lastTime = 1;
    [SerializeField] private float _totalDelay;
    [SerializeField] private float _delayAfterComplete;
    [SerializeField] private float _offsetRange;
    [SerializeField] private float _soundCoinMinDelay;

    [SerializeField] private int _valuePerCoin = 10; // Значение одной монеты

    private void Start()
    {
        _uiController = FindFirstObjectByType<UiController>();
        _levelManager = FindFirstObjectByType<LevelManager>();
    }

    public void StartShowCoins(int totalCoinsValue)
    {
        // Рассчитываем количество монет и остаток
        int coinAmount = totalCoinsValue / _valuePerCoin;
        int remainder = totalCoinsValue % _valuePerCoin;

        // Определяем общую задержку между монетами
        _coinPerDelay = _totalDelay / (coinAmount + (remainder > 0 ? 1 : 0));

        // Создаем монеты для каждой "порции"
        for (int i = 0; i < coinAmount; i++)
        {
            float targetDelay = i * _coinPerDelay;
            ShowCoin(targetDelay, _valuePerCoin, false); // Каждая монета добавляет _valuePerCoin
        }

        // Если есть остаток, создаем одну монету для остатка
        if (remainder > 0)
        {
            float targetDelay = coinAmount * _coinPerDelay;
            ShowCoin(targetDelay, remainder, true); // Последняя монета добавляет остаток
        }

        Invoke(nameof(OnAnimationDone), (_totalDelay + _moveDuration) * _delayAfterComplete);
    }

    private void OnAnimationDone()
    {
        Debug.Log("Coin collect animation done");
        _levelManager.LoadNextLevel();
    }

    private void ShowCoin(float delay, int coinValue, bool onLastCoin)
    {
        GameObject coinObject = Instantiate(_coinPrefab, _coinParent);
        Vector3 offset = new Vector3(Random.Range(-_offsetRange, _offsetRange), Random.Range(-_offsetRange, _offsetRange), 0);
        coinObject.transform.position = _startCoinTransform.position + offset;

        coinObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        coinObject.transform.DOScale(Vector3.one, delay);

        coinObject.transform.DOMove(_endCoinTransform.position, _moveDuration).SetEase(_moveEase).SetDelay(delay).OnComplete(() =>
        {
            if(Time.time - _lastTime > _soundCoinMinDelay)
            {
                SoundUI.Instance.PlaySound(SoundUI.AudioClipsEnum.Coin);
                _lastTime = Time.time;
            }
            _uiController.AddMoney(coinValue); // Передаем значение монеты
            Destroy(coinObject);
        });
    }
}
