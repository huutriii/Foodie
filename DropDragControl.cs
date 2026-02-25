using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DropDragControl : MonoBehaviour
{
    [SerializeField] Image _imgFoodDrag;
    FoodSlot _currentFood, _cacheFood;
    Vector3 _offSet;
    bool _hasDrag;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentFood = Ultils.GetRayCastUI<FoodSlot>(Input.mousePosition);

            if (_currentFood != null && _currentFood.HasFood)
            {
                _hasDrag = true;
                _cacheFood = _currentFood;

                _imgFoodDrag.gameObject.SetActive(true);
                _imgFoodDrag.sprite = _currentFood.GetSpriteFood;
                _imgFoodDrag.SetNativeSize();

                _offSet = _currentFood.transform.position - this.GetWorldMousePos();

                _currentFood.OnActiveFood(false);
            }

        }

        if (_hasDrag)
        {
            _imgFoodDrag.transform.position = GetWorldMousePos() + _offSet;

            FoodSlot slot = Ultils.GetRayCastUI<FoodSlot>(Input.mousePosition);
            if (slot != null)
            {
                if (!slot.HasFood)
                {
                    if (_cacheFood == null || _cacheFood.GetInstanceID() != slot.GetInstanceID())
                    {
                        _cacheFood?.OnHideFood();
                        _cacheFood = slot;
                        _cacheFood.OnFadeFood();
                        _cacheFood.OnSetSlot(_currentFood.GetSpriteFood);
                    }
                }
                else
                {
                    FoodSlot slotAvailable = slot.GetSlotNull;

                    if (slotAvailable != null)
                    {
                        _cacheFood?.OnHideFood();
                        _cacheFood = slot;
                        _cacheFood.OnFadeFood();
                        _cacheFood.OnSetSlot(_currentFood.GetSpriteFood);
                    }
                    else
                    {
                        OnClearCache();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && _hasDrag)
        {

            if(_cacheFood != null)
            {
                _cacheFood.OnMerge();
                _imgFoodDrag.transform.DOMove(_cacheFood.transform.position, 0.15f).OnComplete(() =>
                {
                    _imgFoodDrag.gameObject.SetActive(false);
                    _cacheFood.OnSetSlot(_currentFood.GetSpriteFood);
                    _cacheFood.OnActiveFood(true);
                    _cacheFood =  null;
                }
                );
            }

            FoodSlot targetSlot = Ultils.GetRayCastUI<FoodSlot>(Input.mousePosition);

            _imgFoodDrag.gameObject.SetActive(false);
            _hasDrag = false;
            _currentFood = null;
        }
    }

    public void OnClearCache()
    {
        if (_cacheFood != null && _cacheFood.GetInstanceID() != _currentFood.GetInstanceID())
        {
            _cacheFood.OnHideFood();
            _cacheFood = null;
        }
    }

    public Vector3 GetWorldMousePos()
    {
        Canvas canvas = _imgFoodDrag.canvas;
        Camera uiCamera;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            uiCamera = null;
        else
        {
            uiCamera = canvas.worldCamera;
        }

        RectTransform rectParent = (RectTransform)_imgFoodDrag.transform.parent;
        Vector3 mousePosWorld;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectParent, Input.mousePosition, uiCamera, out mousePosWorld);


        return mousePosWorld;
    }
}
