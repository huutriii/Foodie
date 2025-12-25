using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _totalFood; //tong so loai thuc an
    [SerializeField] int _totalGrill;// tong so bep
    [SerializeField] Transform _gridGrill;

    List<GrillStation> _listGrill;
    float _avgTray; // Trung binh thuc an trong mot dia

    List<Sprite> _totalSpriteFood;

    private void Awake()
    {
        _listGrill = Ultils.GetListInChild<GrillStation>(_gridGrill);

        Sprite[] reloadSprite = Resources.LoadAll<Sprite>("Item");
        _totalSpriteFood = reloadSprite.ToList();
    }
    void Start()
    {
        OnInitLevel();
    }

    void OnInitLevel()
    {
        List<Sprite> takeFood = _totalSpriteFood.OrderBy(x => Random.value).Take(_totalFood).ToList();
        List<Sprite> useFood = new List<Sprite>();

        for (int i = 0; i < takeFood.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                useFood.Add(takeFood[i]);
            }
        }

        // random tráo vị trí của các item

        for (int i = 0; i < useFood.Count; i++)
        {
            int rand = Random.Range(i, useFood.Count);

            (useFood[i], useFood[rand]) = (useFood[rand], useFood[i]); // đổi vị trí rand cho i 
        }

        _avgTray = Random.Range(1.5f, 4f);
        int totalTray = Mathf.RoundToInt(useFood.Count / _avgTray); // tính tổng số đĩa

        List<int> trayPerGrill = this.DistributeEvelyn(_totalGrill, totalTray);
        List<int> foodPerGrill = this.DistributeEvelyn(_totalGrill, takeFood.Count);

        for (int i = 0; i < _listGrill.Count; i++)
        {
            bool activeGrill = i < _totalGrill;
            _listGrill[i].gameObject.SetActive(activeGrill);

            if (activeGrill)
            {
                List<Sprite> listFood = Ultils.TakeAndRemoveRandom<Sprite>(useFood, foodPerGrill[i]);
                _listGrill[i].OnInitGrill(trayPerGrill[i], listFood);
            }
        }
    }

    List<int> DistributeEvelyn(int grillCount, int totalTrays)
    {
        List<int> result = new List<int>();

        // tính trung bình số lượng đĩa
        float avg = (float)totalTrays / grillCount;
        int low = Mathf.FloorToInt(avg);
        int high = Mathf.CeilToInt(avg);

        int highCount = totalTrays - low * grillCount; // tính số bếp được phép nhận nhiều
        int lowCount = grillCount - highCount;

        for (int i = 0; i < lowCount; i++)
            result.Add(low);


        for (int i = 0; i < highCount; i++)
            result.Add(high);

        // đảo vị trí
        for (int i = 0; i < result.Count; i++)
        {
            int rand = Random.Range(i, result.Count);
            (result[i], result[rand]) = (result[rand], result[i]);
        }

        return result;
    }
}
