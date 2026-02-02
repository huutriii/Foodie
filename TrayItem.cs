using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrayItem : MonoBehaviour
{
    [SerializeField] List<Image> _foodList;
    void Awake()
    {
        _foodList = Ultils.GetListInChild<Image>(this.transform);

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
}
