using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    [SerializeField] Image _imgFood;

    Color _nomalColor = new Color(1f, 1f, 1f, 1f);
    Color _fadeColor = new Color(1f, 1f, 1f, 0.6f);

    GrillStation _grillControl;

    void Awake()
    {
        _imgFood = transform.GetChild(0).GetComponent<Image>();
        _imgFood.gameObject.SetActive(false);

        _grillControl = transform.parent.parent.GetComponent<GrillStation>();
    }

    public void OnSetSlot(Sprite spr)
    {
        _imgFood.gameObject.SetActive(true);
        _imgFood.sprite = spr;
        _imgFood.SetNativeSize();
    }

    public void OnFadeFood()
    {
        this.OnActiveFood(true);
        _imgFood.color = _fadeColor;
    }

    public void OnHideFood()
    {
        this.OnActiveFood(false);
        _imgFood.color = _nomalColor;
    }

    public FoodSlot GetSlotNull => _grillControl.GetSlotNull();
    public bool HasFood => _imgFood.gameObject.activeInHierarchy && _imgFood.color == _nomalColor;
    public Sprite GetSpriteFood => _imgFood.sprite;

    public void OnActiveFood(bool active)
    {
        _imgFood.gameObject.SetActive(active);
    }
}
