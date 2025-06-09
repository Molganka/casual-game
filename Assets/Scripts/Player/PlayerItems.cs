using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;
    private GameObject _currentItem;
    private bool _isTrailRendererOn;

    private void OnEnable()
    {
        CollisionHandler.OnBasicFinishEntered += TrailOff;
        CollisionHandler.OnBonusFinishEntered += TrailOff;
    }

    private void OnDisable()
    {
        CollisionHandler.OnBasicFinishEntered -= TrailOff;
        CollisionHandler.OnBonusFinishEntered -= TrailOff;
    }

    private void Start()
    {
        ChangeItem(ItemsWindow.SelectedItemsUI[0]);
        ChangeTrailMaterial(ItemsWindow.SelectedItemsUI[1]);
        if(_isTrailRendererOn) TrailOn();
    }

    public void ChangeItem(ItemUI itemUI)
    {
        if (_currentItem != null)
        {
            Destroy(_currentItem);
            _currentItem = null;
        }

        if (itemUI != null)
        {
            _currentItem = Instantiate(itemUI.ItemObject, transform);
        }
    }

    public void ChangeTrailMaterial(ItemUI itemUI)
    {
        if (itemUI != null)
        {
            _trailRenderer.material = itemUI.ItemMaterial;
            TrailOn();
            _isTrailRendererOn = true;
        }
        else
        {
            TrailOff();
            _isTrailRendererOn = false;
        }
    }

    private void TrailOff()
    {
        _trailRenderer.emitting = false;
    }

    private void TrailOn()
    {
        _trailRenderer.emitting = true;
    }
}
