using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{ 
    [SerializeField] private GameObject _selectImage;
    [SerializeField] private int _type;

    public ItemTypes ItemType;
    [SerializeField] private GameObject _itemObject;
    [SerializeField] private Material _itemMaterial;

    private ItemsWindow _itemsWindow;
    private Button _button;

    public GameObject ItemObject { get { return _itemObject; } set { _itemObject = value; } }
    public Material ItemMaterial { get { return _itemMaterial; } set { _itemMaterial = value; } }
    public int Type { get { return _type; } set { _type = value; } }

    public enum ItemTypes
    {
        Object,
        Material
    }

    private void Awake()
    {
        Type--; //cause index from 0      
    }

    private void Start()
    {
        _button = transform.GetChild(0).GetComponent<Button>();
        _button.onClick.AddListener(OnClick);

        _itemsWindow = FindFirstObjectByType<ItemsWindow>();
    }

    private void OnClick()
    {
        _itemsWindow.ChangeItem(this);
    }

    public void SetSelectOn() => _selectImage.SetActive(true);
    public void SetSelectOff() => _selectImage.SetActive(false);
}
