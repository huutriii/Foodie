using System.Collections.Generic;
using UnityEngine;

public class GrillStation : MonoBehaviour
{
    [SerializeField] private Transform _trayContainer;
    [SerializeField] private Transform _slotContainer;

    public List<TrayItem> _listTray;
    public List<FoodSlot> _listSlot;

    private void Awake()
    {
        _listTray = Ultils.GetListInChild<TrayItem>(_trayContainer);
        _listSlot = Ultils.GetListInChild<FoodSlot>(_slotContainer);
    }

    public void OnInitGrill(int totalTray, List<Sprite> listFood)
    {
        // xử lí set giá trị cho bếp trước
        int foodCount = Random.Range(1, _listSlot.Count + 1);
        List<Sprite> listSlot = Ultils.TakeAndRemoveRandom<Sprite>(listFood, foodCount);

        for (int i = 0; i < listSlot.Count; i++)
        {
            FoodSlot slot = this.RandomSlot();
            if (slot != null)
                slot.OnSetSlot(listSlot[i]);
            else
                break;
        }

        // set giá trị cho đĩa
        List<List<Sprite>> remindFood = new List<List<Sprite>>();

        for (int i = 0; i < totalTray - 1; i++)
        {
            if (listFood.Count == 0) break;
            remindFood.Add(new List<Sprite>());
            int n = Random.Range(0, listFood.Count);
            remindFood[i].Add(listFood[n]);
            listFood.RemoveAt(n);
        }

        while (listFood.Count > 0)
        {
            int rans = Random.Range(0, remindFood.Count);

            if (remindFood[rans].Count < 4)
            {
                int n = Random.Range(0, listFood.Count);
                remindFood[rans].Add(listFood[n]);
                listFood.RemoveAt(n);

            }
        }

        for (int i = 0; i < _listTray.Count; i++)
        {
            bool active = i < remindFood.Count;
            _listTray[i].gameObject.SetActive(active);

            if (active)
            {
                _listTray[i].OnSetFood(remindFood[i]);
            }
        }
    }

    public void OnCheckDrop(Sprite spr)
    {
        FoodSlot slotAvailable = this.GetSlotNull();
        if (slotAvailable != null)
        {
            slotAvailable.OnSetSlot(spr);
            slotAvailable.OnHideFood();
        }
    }

    public FoodSlot GetSlotNull()
    {
        for (int i = 0; i < _listSlot.Count; i++)
        {
            if (!_listSlot[i].HasFood)
                return _listSlot[i];
        }

        return null;
    }

    public FoodSlot RandomSlot()
    {
        List<FoodSlot> emptySlots = _listSlot.FindAll(s => !s.HasFood);
        if (emptySlots.Count == 0) return null;

        int n = Random.Range(0, emptySlots.Count);
        return emptySlots[n];
    }
}
