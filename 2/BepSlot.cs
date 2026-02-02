using UnityEngine;
using UnityEngine.UI;

public class BepSlot : MonoBehaviour
{
    [SerializeField] Image _imgFood;

    private void Awake()
    {
        _imgFood = transform.GetChild(0).GetComponent<Image>();
        _imgFood.gameObject.SetActive(false);
    }

    public void OnSetSlot(Sprite spr)
    {
        _imgFood.sprite = spr;
        _imgFood.SetNativeSize();
        _imgFood.gameObject.SetActive(true);
    }

    public bool HasFood => _imgFood.gameObject.activeInHierarchy;
    public Sprite GetSpriteFood => _imgFood.sprite;

    public void OnActiveFood(bool active) => _imgFood.gameObject.SetActive(active);
}
