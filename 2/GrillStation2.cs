using System.Collections.Generic;
using UnityEngine;

public class GrillStation2 : MonoBehaviour
{
    Transform _diaContainer;
    Transform _bepContainer;

    List<DiaSlot> _totalDia;
    List<BepSlot> _totalBep;

    private void Awake()
    {
        _totalBep = Ultis2.GetListInChildren<BepSlot>(_bepContainer);
        _totalDia = Ultis2.GetListInChildren<DiaSlot>(_diaContainer);
    }

    public void OnInitGrill(int totalTray, List<Sprite> listFood)
    {
        int foodCount = Random.Range(1, _totalBep.Count + 1);
        List<Sprite> foods = listFood;

        List<Sprite> listSlot = Ultis2.TakeListDistribute<Sprite>(foods, foodCount);

        for (int i = 0; i < listSlot.Count; i++)
        {
            BepSlot slot = this.RandomBepSlot();
            slot.OnSetSlot(listSlot[i]);
        }
    }

    public BepSlot RandomBepSlot()
    {
        int n;

        while (true)
        {
            n = Random.Range(0, _totalBep.Count);
            if (_totalBep[n].HasSlot())
                return _totalBep[n];
        }
    }
}
