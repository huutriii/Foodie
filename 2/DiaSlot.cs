using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaSlot : MonoBehaviour
{
    List<Image> _listThucAnTrenDia;
    private void Awake()
    {
        _listThucAnTrenDia = Ultis2.GetListInChildren<Image>(this.transform);

        foreach (var i in _listThucAnTrenDia)
        {
            i.gameObject.SetActive(false);
        }
    }

    public void SetThucAnChoDia(List<Image> thucAn)
    {
        if (thucAn.Count <= _listThucAnTrenDia.Count)
        {
            for (int i = 0; i < thucAn.Count; i++)
            {
                Image slot = this.RandomSlot();
                slot.gameObject.SetActive(true);
                slot.sprite = thucAn[i].sprite;
                slot.SetNativeSize();
            }
        }
    }

    public Image RandomSlot()
    {
        int n;

        while (true)
        {
            n = Random.Range(0, _listThucAnTrenDia.Count);
            if (!_listThucAnTrenDia[n].gameObject.activeInHierarchy)
                return _listThucAnTrenDia[n];
        }
    }
}
