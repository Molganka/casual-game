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
        if (ItemsWindow.SelectedItemsUI[0] != -1)
            ChangeItem(ItemsWindow.Instance.Types[0].AccessibleItems[ItemsWindow.SelectedItemsUI[0]].GetComponent<ItemUI>());
        if (ItemsWindow.SelectedItemsUI[1] != -1)
            ChangeTrailMaterial(ItemsWindow.Instance.Types[1].AccessibleItems[ItemsWindow.SelectedItemsUI[1]].GetComponent<ItemUI>());

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
