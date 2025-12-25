using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    [SerializeField] int _totalFood;
    [SerializeField] int _totalGrill;
    [SerializeField] Transform _gridGrill;

    List<GrillStation2> _listGrill;
    float _avgTray;

    List<Sprite> _totalSpriteFood;

    private void Awake()
    {
        _listGrill = Ultis2.GetListInChildren<GrillStation2>(_gridGrill);

        Sprite[] sprites = Resources.LoadAll<Sprite>("Item");
        _totalSpriteFood = sprites.ToList();
    }

    private void Start()
    {
        OnInitLevel();
    }

    private void OnInitLevel()
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

        for (int i = 0; i < useFood.Count; i++)
        {
            int idx = Random.Range(i, useFood.Count);

            (useFood[i], useFood[idx]) = (useFood[idx], useFood[i]);
        }

        _avgTray = Random.Range(1.5f, 4f);

        int totalTray = Mathf.RoundToInt(useFood.Count / _avgTray);

    }


    public void Distribute(int grillCount, int totalTray)
    {
        List<int> result = new List<int>();

        float avg = (float)totalTray / grillCount;

        // tính toán số lượng thức ăn trên mỗi đĩa
        int high = Mathf.CeilToInt(avg);
        int low = Mathf.FloorToInt(avg);
        // tính số lượng bếp chứa nhiều và ít thức ăn
        int highCount = totalTray - low * grillCount;
        int lowCount = grillCount - highCount;

        for (int i = 0; i < highCount; i++)
        {
            result.Add(low);
        }
    }
}
