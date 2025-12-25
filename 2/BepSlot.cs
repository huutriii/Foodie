using UnityEngine;
using UnityEngine.UI;

public class BepSlot : MonoBehaviour
{
    [SerializeField] Image _img;

    private void Awake()
    {
        _img = transform.GetChild(0).GetComponent<Image>();
        _img.gameObject.SetActive(false);
    }

    public void OnSetSlot(Sprite spr)
    {
        _img.sprite = spr;
        _img.gameObject.SetActive(true);
        _img.SetNativeSize();
    }

    public bool HasSlot() => _img.gameObject.activeInHierarchy;
}
