using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int _totalFood;
    [SerializeField] int _realTotalFood;
    [SerializeField] int _allFood;
    public int TotalFood => _totalFood;

    [SerializeField] int _totalGrill;
    [SerializeField] Transform _gridGrill;
    List<GrillStation> _listGrill;
    float _avgTray;

    List<Sprite> _totalSpriteFood;
    [SerializeField] List<Sprite> _useFood;

    const string SETTING_SCENE = "Setting";

    private void Awake()
    {

        _realTotalFood = TotalFood;
        _useFood = new List<Sprite>();
        _listGrill = Ultils.GetListInChild<GrillStation>(_gridGrill);

        Sprite[] reloadSprite = Resources.LoadAll<Sprite>("Item");
        _totalSpriteFood = reloadSprite.ToList();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Start()
    {
        OnInitLevel();
    }

    void OnInitLevel()
    {
        List<Sprite> takeFood = _totalSpriteFood.OrderBy(x => Random.value).Take(_totalFood).ToList();

        for (int i = 0; i < _allFood; i++)
        {
            int n = i % takeFood.Count;

            for (int j = 0; j < 3; j++)
            {
                _useFood.Add(takeFood[n]);
            }
        }

        for (int i = 0; i < _useFood.Count; i++)
        {
            int rand = Random.Range(i, _useFood.Count);

            (_useFood[i], _useFood[rand]) = (_useFood[rand], _useFood[i]); // đổi vị trí rand cho i 
        }

        // _avgTray = Random.Range(1.5f, 4f);
        _avgTray = 2;
        int totalTray = Mathf.RoundToInt(_useFood.Count / _avgTray); // tính tổng số đĩa

        List<int> foodPerGrill = this.DistributeEvelyn(_totalGrill, _useFood.Count);
        List<int> trayPerGrill = this.DistributeEvelyn(_totalGrill, totalTray);

        for (int i = 0; i < _listGrill.Count; i++)
        {
            bool activeGrill = i < _totalGrill;
            _listGrill[i].gameObject.SetActive(activeGrill);

            if (activeGrill)
            {
                List<Sprite> listFood = Ultils.TakeAndRemoveRandom<Sprite>(_useFood, foodPerGrill[i]);
                _listGrill[i].OnInitGrill(trayPerGrill[i], listFood);
                _realTotalFood += _listGrill[i].GetFoodOnGrill();
            }

        }
    }

    public void OnMinusFood()
    {
        _allFood--;

        if (_allFood <= 0)
        {
            Debug.Log("Game complete");
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

    public void OnCheckAndShake()
    {
        Dictionary<string, List<FoodSlot>> groups = new Dictionary<string, List<FoodSlot>>();

        foreach (var grill in _listGrill)
        {
            if (grill.gameObject.activeInHierarchy)
            {
                for (int i = 0; i < grill.ListSlot.Count; i++)
                {
                    FoodSlot slot = grill.ListSlot[i];

                    if (slot.HasFood)
                    {
                        string name = slot.GetSpriteFood.name;

                        if (!groups.ContainsKey(name))
                        {
                            groups.Add(name, new List<FoodSlot>());
                        }
                        groups[name].Add(slot);
                    }
                }
            }
        }


        foreach (var group in groups)
        {
            if (group.Value.Count >= 3)
            {
                for(int i = 0; i < 3; i++)
                {
                    group.Value[i].DoShake();
                }

                return;
            }
        }
    }

    public void OnMagnet()
    {
        Dictionary<string, List<Image>> groups= new Dictionary<string, List<Image>>();

        foreach(var grill in _listGrill)
        {
            if (grill.gameObject.activeInHierarchy)
            {
                for(int i=0;i<= grill.ListSlot.Count;i++)
                {
                    FoodSlot slot = grill.ListSlot[i];
                    if (slot.HasFood)
                    {
                        string name = slot.GetSpriteFood.name;

                        if (!groups.ContainsKey(name))
                        {
                            groups.Add(name, new List<Image>());
                        }

                        groups[name].Add(slot.ImageFood);
                    }
                }
            }
        }
    }

    public void LoadSettingScene()
    {
        if (SceneManager.GetSceneByName(SETTING_SCENE).isLoaded) return;

        SceneManager.sceneLoaded += OnSettingLoaded;
        SceneManager.LoadScene(SETTING_SCENE, LoadSceneMode.Additive);

        AudioManager.Instance.PlayChangeChar();
    }

    public void OnSettingLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != SETTING_SCENE) return;

        foreach(GameObject root in scene.GetRootGameObjects())
        {
            Camera cam = root.GetComponentInChildren<Camera>(true);
            if (cam != null) cam.enabled = false;

            EventSystem es = root.GetComponentInChildren<EventSystem>(true);
            if (es != null) es.enabled = false;
        }

        SceneManager.sceneLoaded -= OnSettingLoaded;
    }
}
