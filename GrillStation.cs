using System.Collections.Generic;
using UnityEngine;

public class GrillStation : MonoBehaviour
{
    [SerializeField] private Transform _trayContainer;
    [SerializeField] private Transform _slotContainer;

    List<TrayItem> _totalTray;
    List<FoodSlot> _totalSlot;

    private void Awake()
    {
        _totalTray = Ultils.GetListInChild<TrayItem>(_trayContainer);
        _totalSlot = Ultils.GetListInChild<FoodSlot>(_slotContainer);
    }

    public void OnInitGrill(int totalTray, List<Sprite> listFood)
    {
        // xử lí set giá trị cho bếp trước
        int foodCount = Random.Range(1, _totalSlot.Count + 1);
        List<Sprite> list = listFood;
        List<Sprite> listSlot = Ultils.TakeAndRemoveRandom<Sprite>(list, foodCount);

        for (int i = 0; i < listSlot.Count; i++)
        {
            FoodSlot slot = this.RandomSlot();
            slot.OnSetSlot(listSlot[i]);
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

        for (int i = 0; i < _totalTray.Count; i++)
        {
            bool active = i < remindFood.Count;
            _totalTray[i].gameObject.SetActive(active);

            if (active)
            {
                _totalTray[i].OnSetFood(remindFood[i]);
            }
        }
    }

    public FoodSlot RandomSlot()
    {
    reRand: int n = Random.Range(0, _totalSlot.Count);
        if (_totalSlot[n].HasFood) goto reRand;

        return _totalSlot[n];
    }
}
