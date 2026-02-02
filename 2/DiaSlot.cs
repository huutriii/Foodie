using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaSlot : MonoBehaviour
{
    [SerializeField] List<Image> _foodList = new List<Image>();

    private void Awake()
    {
        _foodList = Ultis2.GetListInChildren<Image>(this.transform);

        foreach (var food in _foodList)
        {
            food.gameObject.SetActive(false);
        }
    }

    public void OnSetFood(List<Sprite> foodList)
    {
        if (foodList.Count <= _foodList.Count)
        {
            for (int i = 0; i < foodList.Count; i++)
            {
                _foodList[i].sprite = foodList[i];
                _foodList[i].gameObject.SetActive(true);
                _foodList[i].SetNativeSize();
            }
        }
    }

    public Image RandomSlot()
    {
        List<Image> emptySlots = _foodList.FindAll(s => !s.gameObject.activeInHierarchy);
        
        if (emptySlots.Count == 0) return null;

        int randomIdx = Random.Range(0, emptySlots.Count);
        return emptySlots[randomIdx];
    }
}
