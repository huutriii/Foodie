using System.Collections.Generic;
using UnityEngine;

public class GrillStation2 : MonoBehaviour
{
    [SerializeField] Transform _slotContainer;
    [SerializeField] Transform _trayContainer;

    List<FoodSlot> _totalSlot;
    List<TrayItem> _totalTray;

    private void Awake()
    {
        _totalSlot = Ultils.GetListInChild<FoodSlot>(_slotContainer);
        _totalTray = Ultils.GetListInChild<TrayItem>(_trayContainer);
    }

    public void OnInitGrill(List<Sprite> listFood, int totalTray)
    {
        int foodCount = Random.Range(1, _totalSlot.Count + 1);
        List<Sprite> list = listFood;

        List<Sprite> listSlot = Ultils.TakeAndRemoveRandom(list, foodCount);

        for (int i = 0; i < listFood.Count; i++)
        {
            FoodSlot slot = this.RandomSlot();
            if (slot != null)
            {
                slot.OnSetSlot(listFood[i]);
            }
            else
                break;
        }

        List<List<Sprite>> remindFood = new List<List<Sprite>>();
        for (int i = 0; i < totalTray - 1; i++)
        {
            if (remindFood.Count <= 0) break;
            remindFood.Add(new List<Sprite>());
            int randomIdx = Random.Range(0, listFood.Count);
            remindFood[i].Add(listFood[randomIdx]);
            listFood.RemoveAt(randomIdx);
        }

        while (listFood.Count > 0)
        {
            int randomIdx = Random.Range(0, remindFood.Count);
            if (remindFood[randomIdx].Count < 4)
            {
                int n = Random.Range(0, listFood.Count);
                remindFood[randomIdx].Add(listFood[n]);
                listFood.RemoveAt(n);
            }
        }

        for (int i = 0; i < _totalTray.Count; i++)
        {
            bool activeTray = i < remindFood.Count;
            _totalTray[i].gameObject.SetActive(activeTray);

            if (activeTray)
            {
                _totalTray[i].OnSetFood(remindFood[i]);
            }
        }
    }

    public FoodSlot RandomSlot()
    {
        List<FoodSlot> emptySlot = _totalSlot.FindAll(x => !x.gameObject.activeSelf);
        if (emptySlot.Count <= 0) return null;

        int randomIdx = Random.Range(0, emptySlot.Count);
        return emptySlot[randomIdx];
    }
}
