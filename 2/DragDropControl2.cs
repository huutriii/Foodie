using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NUnit.Framework.Constraints;

public class DropDragControl2 : MonoBehaviour
{
    [SerializeField] Image _imgFoodDrag;

    [SerializeField] FoodSlot _currentFood, _cacheFood;

    Vector3 _offSet = Vector3.zero;
    [SerializeField] bool _hasDrag;

    [SerializeField] float _timeCheckSuggest;
    [SerializeField] float _countTime;

    float _scaleMultiple = 1.2f;
    float _speed = 10f;

    Vector3 _originScale;
    Vector3 _targetScale;

    bool _isSelected = false;

    private void Start()
    {
        _originScale = transform.localScale;
        _targetScale = _originScale;
    }

    private void Update()
    {
        _countTime += Time.deltaTime;

        if (_countTime >= _timeCheckSuggest)
        {
            _countTime = 0;
            GameManager.Instance?.OnCheckAndShake();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //_countTime = 0;
            _currentFood = Ultils.GetRayCastUI<FoodSlot>(Input.mousePosition);

            if (_currentFood != null && _currentFood.HasFood)
            {
                AudioManager.Instance.PlayPickUp();

                _hasDrag = true;
                _cacheFood = _currentFood;

                _imgFoodDrag.gameObject.SetActive(true);
                _imgFoodDrag.sprite = _currentFood.GetSpriteFood;
                _imgFoodDrag.SetNativeSize();

                _imgFoodDrag.transform.localScale = Vector3.one;
                _imgFoodDrag.transform.DOScale(Vector3.one * 1.25f, 0.15f).SetEase(Ease.OutBack);

                _offSet = _currentFood.transform.position - GetWorldMousePos();
                _offSet.z = 0;

                _currentFood.OnActiveFood(false);
            }
        }

        if (_hasDrag)
        {
            _imgFoodDrag.transform.position = _offSet + GetWorldMousePos();

            FoodSlot hoverFadeFood = Ultils.GetRayCastUI<FoodSlot>(Input.mousePosition);

            if (hoverFadeFood != null)
            {
                if (hoverFadeFood != _cacheFood)
                {
                    _cacheFood?.OnActiveFood(false);
                    _cacheFood = null;
                    // _cacheFood = null;

                    if (!hoverFadeFood.HasFood)
                    {
                        _cacheFood = hoverFadeFood;
                        _cacheFood.OnSetSlot(_imgFoodDrag.sprite);
                        _cacheFood.OnHideFood();
                        _cacheFood.OnFadeFood();
                    }
                }
            }
            else
            {
                _cacheFood?.OnActiveFood(false);
                _cacheFood = null;
            }
        }

        if (Input.GetMouseButtonUp(0) && _hasDrag)
        {
            _hasDrag = false;

            FoodSlot targetSlot = Ultils.GetRayCastUI<FoodSlot>(Input.mousePosition);

            if (targetSlot != null)
            {
                if (!targetSlot.HasFood)
                {
                    _imgFoodDrag.transform.DOMove(targetSlot.transform.position, 0.1f).OnComplete(() =>
                      {
                          targetSlot.OnHideFood();
                          targetSlot.OnSetSlot(_imgFoodDrag.sprite);
                          //_currentFood = null;
                          _imgFoodDrag.gameObject.SetActive(false);

                          _currentFood.AutoFillSlot();

                          AudioManager.Instance.PlayPopSmoke();

                          targetSlot.OnMerge();
                      });
                }
                else
                {
                    FoodSlot availableSlot = targetSlot.GetSlotNull;

                    if (availableSlot != null)
                    {
                        _imgFoodDrag.transform.DOMove(availableSlot.transform.position, 0.1f).OnComplete(() =>
                        {
                            availableSlot.OnHideFood();
                            availableSlot.OnSetSlot(_imgFoodDrag.sprite);

                            _imgFoodDrag.gameObject.SetActive(false);
                            _currentFood.AutoFillSlot();

                            AudioManager.Instance.PlayPopSmoke();

                            availableSlot.OnMerge();
                        });
                    }
                    else
                    {
                        _imgFoodDrag.transform.DOMove(_currentFood.transform.position, 0.1f).OnComplete(() =>
                        {

                            _imgFoodDrag.gameObject.SetActive(false);
                            _currentFood.OnActiveFood(true);
                        });
                    }
                }
            }
            else
            {
                _imgFoodDrag.transform.DOMove(_currentFood.transform.position, 0.1f).OnComplete(() =>
                {

                    _imgFoodDrag.gameObject.SetActive(false);
                    _currentFood.OnActiveFood(true);
                });
            }
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
        Vector3 worldMousePos;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectParent, Input.mousePosition, uiCamera, out worldMousePos);

        return worldMousePos;
    }
}