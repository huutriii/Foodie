using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrayItem2 : MonoBehaviour
{
    List<Image> _foodList;

    public void OnSetFood(List<Sprite> spr)
    {
        for (int i = 0; i < _foodList.Count; i++)
        {
            _foodList[i].sprite = spr[i];
        }
    }

    public int SetFoodList(List<FoodSlot> foods, int requireFood)
    {
        int shortageFood = 0;

        List<Image> activeFoodOnTray = _foodList.Where(x => x.gameObject.activeSelf).ToList();
        int takeCount = Mathf.Min(requireFood, activeFoodOnTray.Count);

        List<FoodSlot> subFoodsNeedFill = foods.Where(x => !x.HasFood).ToList();

        for (int i = 0; i < takeCount; i++)
        {
            subFoodsNeedFill[i].OnSetSlot(activeFoodOnTray[i].sprite);
            activeFoodOnTray[i].gameObject.SetActive(false);
        }

        if(requireFood > activeFoodOnTray.Count)
        {
            shortageFood = requireFood - takeCount;
        }

        return shortageFood;
    }
}
