using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FoodSlot : MonoBehaviour
{
    [SerializeField] Image _imgFood;
    public Image ImageFood => _imgFood;

    Color _nomalColor = new Color(1f, 1f, 1f, 1f);
    Color _fadeColor = new Color(1f, 1f, 1f, 0.6f);

    GrillStation _grillControl;

    Vector3 _originLocalPos;

    void Awake()
    {
        _imgFood = transform.GetChild(0).GetComponent<Image>();
        _imgFood.gameObject.SetActive(false);
        _originLocalPos = _imgFood.transform.localPosition;

        _grillControl = transform.parent.parent.GetComponent<GrillStation>();
    }

    public void OnSetSlot(Sprite spr)
    {
        _imgFood.gameObject.SetActive(true);
        _imgFood.sprite = spr;
        _imgFood.SetNativeSize();

        _imgFood.color = _nomalColor;
        _imgFood.transform.localPosition = _originLocalPos;
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

    public void OnMerge()
    {
        _grillControl?.OnMerge();
    }

    public void OnActiveFood(bool active)
    {
        _imgFood.gameObject.SetActive(active);
        _imgFood.color = _nomalColor;
    }

    public void OnReFillSlot(Image food)
    {
        Sprite sprite = food.sprite;
        Vector3 startPos = food.transform.position;

        food.gameObject.SetActive(false);

        OnSetSlot(sprite);
        
        Vector3 targetLocalPos = _imgFood.transform.localPosition;
        
        _imgFood.transform.position = startPos;

        // Tăng thời gian animation để tạo khoảng cách giữa các món
        _imgFood.transform.DOLocalMove(targetLocalPos, 1.5f).OnComplete(() =>
        {
            _imgFood.color = _nomalColor;
        });
    }

    public void AutoFillSlot()
    {
        _grillControl.AutoFillSlot();
    }
    
    public void DoShake()
    {
        _imgFood.transform.DOShakePosition(0.5f, 10, 10, 180f);
    }

    public void PlayMergeAnim()
    {
        _imgFood.transform.DOLocalMoveY(_originLocalPos.y + 100f, 0.5f);

        _imgFood.DOFade(0f, 0.5f).OnComplete(() =>
        {
            _imgFood.color = _nomalColor;
            _imgFood.transform.localPosition = _originLocalPos;

            this.OnActiveFood(false);
        });
    }

    public FoodSlot GetSlotNull => _grillControl.GetSlotNull();
    public bool HasFood => _imgFood.gameObject.activeInHierarchy && _imgFood.color == _nomalColor;
    public Sprite GetSpriteFood => _imgFood.sprite;
}
