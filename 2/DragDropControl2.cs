using UnityEngine;
using UnityEngine.UI;

public class DropDragControl2 : MonoBehaviour
{
    [SerializeField] Image _imageFoodDrag;

    FoodSlot _currentFood, _cacheFood;
    Vector3 _offset;

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

                _imageFoodDrag.gameObject.SetActive(true);
                _imageFoodDrag.sprite = _currentFood.GetSpriteFood;
                _imageFoodDrag.SetNativeSize();

                _offset = _currentFood.transform.position - GetWorldMousePos();

                _currentFood.OnActiveFood(false);
            }
        }

        if (_hasDrag)
        {
            _imageFoodDrag.transform.position = GetWorldMousePos() + _offset;

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
                        OnClearCacheSlot();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && _hasDrag)
        {
            FoodSlot targetSlot = Ultils.GetRayCastUI<FoodSlot>(Input.mousePosition);

            if (targetSlot != null)
            {
                if (!targetSlot.HasFood)
                {
                    targetSlot.OnSetSlot(_imageFoodDrag.sprite);
                    Debug.Log("drop");
                }
                else
                {
                    _currentFood.OnActiveFood(true);
                }
            }
            else
            {
                _currentFood.OnActiveFood(true);
            }

            _hasDrag = false;
            _currentFood = null;
            _imageFoodDrag.gameObject.SetActive(false);
        }
    }

    public void OnClearCacheSlot()
    {
        if (_cacheFood != null && _cacheFood.GetInstanceID() != _currentFood.GetInstanceID())
        {
            _cacheFood.OnHideFood();
            _cacheFood = null;
        }
    }

    public Vector3 GetWorldMousePos()
    {
        Canvas canvas = _imageFoodDrag.canvas;
        Camera uiCamera;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = null;
        }
        else
        {
            uiCamera = canvas.worldCamera;
        }

        Vector3 mouseWorldPos;
        RectTransform rectParent = (RectTransform)_imageFoodDrag.transform.parent;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectParent, Input.mousePosition, uiCamera, out mouseWorldPos);

        return mouseWorldPos;
    }

}