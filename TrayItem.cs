using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class TrayItem : MonoBehaviour
{
    [SerializeField] List<Image> _foodList;
    [SerializeField] Image _imgTray;
    [SerializeField] Sprite _spriteTray;
    void Awake()
    {
        _foodList = Ultils.GetListInChild<Image>(this.transform);
        _imgTray = GetComponent<Image>();
        _spriteTray = _imgTray.sprite;
        for (int i = 0; i < _foodList.Count; i++)
        {
            _foodList[i].gameObject.SetActive(false);
        }
    }

    public void OnSetFood(List<Sprite> items)
    {
        if (items.Count <= _foodList.Count)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Image slot = RandomSlot();
                if (slot == null) break;

                slot.gameObject.SetActive(true);
                slot.sprite = items[i];
                slot.SetNativeSize();
            }
        }
    }

    public int SetFoodList(List<FoodSlot> foods, int requireFood)
    {
        int shortageFood = 0;

        List<Image> activeFoodOnTray = _foodList.Where(x => x.gameObject.activeSelf).ToList();
        int takeCount = Mathf.Min(requireFood, activeFoodOnTray.Count);

        List<FoodSlot> subFoodsNeedFill = foods.Where(x => !x.HasFood).ToList();

        if (activeFoodOnTray.Count == 0)
        {
            return requireFood;
        }

        for (int i = 0; i < takeCount; i++)
        {
            subFoodsNeedFill[i].OnReFillSlot(activeFoodOnTray[i]);
            activeFoodOnTray[i].gameObject.SetActive(false);
        }

        if (requireFood > activeFoodOnTray.Count)
        {
            shortageFood = requireFood - activeFoodOnTray.Count;
        }

        if (CheckEmptyFoods())
        {
            StartCoroutine(WaitTrayFill());
        }

        return shortageFood;
    }

    public void SetEmptyTray()
    {
        if (CheckEmptyFoods())
            gameObject.SetActive(false);
    }

    public bool CheckEmptyFoods()
    {
        foreach (var food in _foodList)
        {
            if (food.gameObject.activeSelf) return false;
        }

        return true;
    }


    Image RandomSlot()
    {
        List<Image> emptySlots = new List<Image>();
        for (int i = 0; i < _foodList.Count; i++)
        {
            if (!_foodList[i].gameObject.activeInHierarchy)
            {
                emptySlots.Add(_foodList[i]);
            }
        }

        if (emptySlots.Count == 0) return null;

        int n = Random.Range(0, emptySlots.Count);
        return emptySlots[n];
    }
    public IEnumerator WaitTrayFill()
    {
        yield return new WaitForSeconds(0.8f);

        RectTransform rect = _imgTray.rectTransform;

        rect.DOAnchorPos(rect.anchoredPosition + new Vector2(200, 0), 1f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public int GetFoodOnTray()
    {
        return _foodList.Where(x=>x.gameObject.activeSelf).Count();
    }

}
