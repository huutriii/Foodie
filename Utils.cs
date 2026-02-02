using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Ultils : MonoBehaviour
{
    public static List<T> GetListInChild<T>(Transform parent)
    {
        List<T> result = new List<T>();

        for (int i = 0; i < parent.childCount; i++)
        {
            var component = parent.GetChild(i).GetComponent<T>();
            if (component != null)
            {
                result.Add(component);
            }
        }

        return result;
    }

    public static List<T> TakeAndRemoveRandom<T>(List<T> source, int n)
    {
        List<T> result = new List<T>();
        n = Mathf.Min(n, source.Count);

        for (int i = 0; i < n; i++)
        {
            int randIndex = Random.Range(0, source.Count);
            result.Add(source[randIndex]);
            source.RemoveAt(randIndex);
        }

        return (result);
    }

    public static T GetRayCastUI<T>(Vector2 postion) where T : MonoBehaviour
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = postion;
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, list);

        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T component = list[i].gameObject.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }
        }

        return null;
    }
}
