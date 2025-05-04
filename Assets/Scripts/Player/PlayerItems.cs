using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;
    private GameObject _currentItem;

    private void Start()
    {
        ChangeItem(ItemsWindow.SelectedItemsUI[0]);
        ChangeTrailMaterial(ItemsWindow.SelectedItemsUI[1]);
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
            _trailRenderer.emitting = true;
        }
        else
        {
            _trailRenderer.emitting = false;
        }

    }
}
