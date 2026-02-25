using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GrillStation : MonoBehaviour
{
    [SerializeField] private Transform _trayContainer;
    [SerializeField] private Transform _slotContainer;

    public List<TrayItem> _listTray;
    public List<FoodSlot> _listSlot;
    public List<FoodSlot> ListSlot => _listSlot;

    Stack<TrayItem> _stackTray;

    [SerializeField] SmokeController _smoke;

    private void Awake()
    {
        _listTray = Ultils.GetListInChild<TrayItem>(_trayContainer);
        _listSlot = Ultils.GetListInChild<FoodSlot>(_slotContainer);

        _stackTray = new Stack<TrayItem>();

        StartCoroutine(IE_Smoking());
    }

    public void OnInitGrill(int totalTray, List<Sprite> listFood)
    {
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

        List<List<Sprite>> remindFood = new List<List<Sprite>>();

        for (int i = 0; i < totalTray; i++)
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

                TrayItem item = _listTray[i];
                _stackTray.Push(item);
            }

            _listTray[i].SetEmptyTray();
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

    public void OnMerge()
    {
        if (this.GetSlotNull() == null)
        {
            if (this.CanMerge())
            {
                AudioManager.Instance.PlayMerge();
                StartCoroutine(IE_ProcessMerge());
            }
        }
    }

    public bool CanMerge()
    {
        string name = _listSlot[0].GetSpriteFood.name;

        for (int i = 0; i < _listSlot.Count; i++)
        {
            if (_listSlot[i].GetSpriteFood.name != name)
                return false;
        }

        return true;
    }

    public IEnumerator OnFillSlot()
    {
        int idx = GetActiveTray();

        while (idx >= 0)
        {
            int requireFood = RequireFood();

            if (requireFood == 0)
            {
                yield break;
            }

            int require = _listTray[idx].SetFoodList(_listSlot, requireFood);

            if (require == 0)
            {
                break;
            }

            yield return new WaitForSeconds(3.3f);

            idx--;
        }
    }

    public void OnFillSlot2()
    {
        int idx = GetActiveTray();

        if(idx >= 0)
        {
            _listTray[idx].SetFoodList(_listSlot, _listTray[idx].GetFoodOnTray());
        }
    }

    public int GetActiveTray()
    {
        for (int i = _listTray.Count - 1; i >= 0; i--)
        {
            if (_listTray[i].gameObject.activeSelf) return i;
        }
        return -1;
    }

    public int RequireFood()
    {
        int requireFood = 0;

        for (int i = 0; i < _listSlot.Count; i++)
        {
            if (!_listSlot[i].HasFood)
                requireFood++;
        }

        return requireFood;
    }

    public void AutoFillSlot()
    {
        if (CheckEmptySlot())
        {
            if (!_smoke.gameObject.activeSelf)
            {
                _smoke.gameObject.SetActive(true);
            }

            StartCoroutine(OnFillSlot());
        }
    }

    public bool CheckEmptySlot()
    {
        for (int i = 0; i < _listSlot.Count; i++)
        {
            if (_listSlot[i].HasFood)
            {
                return false;
            }

        }
        return true;
    }

    public int GetFoodOnGrill()
    {
        int foodOnSlot = _listSlot.Where(x => x.HasFood).Count();
        int foodOnTray = _listTray.Sum(x => x.GetFoodOnTray());

        return foodOnSlot + foodOnTray;
    }

    public IEnumerator IE_ProcessMerge()
    {
        for (int i = 0; i < _listSlot.Count; i++)
        {
            _listSlot[i].PlayMergeAnim();
        }

        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < _listSlot.Count; i++)
        {
            _listSlot[i].OnActiveFood(false);
        }

        GameManager.Instance.OnMinusFood();

        StartCoroutine(OnFillSlot());
    }

    public IEnumerator IE_Smoking()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 15));

            if (!_smoke.gameObject.activeSelf)
                _smoke.gameObject.SetActive(true);
        }
    }
}
