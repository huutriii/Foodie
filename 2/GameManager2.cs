using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    [SerializeField] int _totalFood;
    [SerializeField] int _totalGrill;
    [SerializeField] Transform _gridGrill;
    List<GrillStation> _listGrill;

    float _avgTray;
    List<Sprite> _totalSriteFood;

    private void Awake()
    {
        _listGrill = Ultils.GetListInChild<GrillStation>(_gridGrill);

        Sprite[] listSprite = Resources.LoadAll<Sprite>("Item");
        _totalSriteFood = listSprite.ToList();
    }

    private void Start()
    {
        OnInitLevel();
    }
    public void OnInitLevel()
    {
        List<Sprite> takeFood = _totalSriteFood.ToList();
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
            int randomIdx = Random.Range(i, useFood.Count);
            (useFood[i], useFood[randomIdx]) = (useFood[randomIdx], useFood[i]);
        }

        _avgTray = Random.Range(1.5f, 4f);
        int totalTray = Mathf.RoundToInt(useFood.Count / _avgTray);
    }

    public List<int> Distribute(int grillCount, int totalTray)
    {
        List<int> result = new List<int>();

        float avg = (float)totalTray / grillCount;
        int low = Mathf.FloorToInt(avg);
        int high = Mathf.CeilToInt(avg);

        int highCount = totalTray - low * grillCount;
        int lowCount = grillCount - highCount;

        for (int i = 0; i < lowCount; i++)
            result.Add(low);

        for (int i = 0; i < highCount; i++)
            result.Add(high);

        for (int i = 0; i < result.Count; i++)
        {
            int randomIdx = Random.Range(i, result.Count);
            (result[i], result[randomIdx]) = (result[randomIdx], result[i]);
        }
        return result;
    }
}
