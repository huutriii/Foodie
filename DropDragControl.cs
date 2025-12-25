using UnityEngine;
using UnityEngine.UI;

public class DropDragControl : MonoBehaviour
{
    [SerializeField] Image _imgFoodDrag;

    FoodSlot _currentFood;
    bool _hasDrag;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentFood = Ultils.GetRayCastUI<FoodSlot>(Input.mousePosition);// check o vi tri click chuot xem co UI gan class FoodSlot khong
            if (_currentFood != null && _currentFood.HasFood)
            {
                _hasDrag = true;

                // gan sprite cho dummy image
                _imgFoodDrag.gameObject.SetActive(true);
                _imgFoodDrag.sprite = _currentFood.GetSpriteFood;
                _imgFoodDrag.SetNativeSize();
                _imgFoodDrag.transform.position = _currentFood.transform.position;
            }
        }
    }
}
