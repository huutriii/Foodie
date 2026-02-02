using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ultis2 : MonoBehaviour
{
    public static List<T> GetListInChildren<T>(Transform parent)
    {
        List<T> list = new List<T>();

        for (int i = 0; i < parent.childCount; i++)
        {
            var component = parent.GetChild(i).GetComponent<T>();
            if (component != null)
                list.Add(component);
        }

        return list;
    }

    public static List<T> TakeListDistribute<T>(List<T> source, int n)
    {
        List<T> result = new List<T>();
        n = Mathf.Min(n, source.Count);
        for (int i = 0; i < n; i++)
        {
            int randIdx = Random.Range(0, source.Count);
            result.Add(source[randIdx]);
            source.RemoveAt(randIdx);
        }

        return result;
    }

    public static T GetRaycastUI<T>(Vector2 mousePos) where T : MonoBehaviour
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = mousePos;

        List<RaycastResult> hits = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, hits);

        if (hits.Count > 0)
        {
            foreach (var hit in hits)
            {
                var component = hit.gameObject.GetComponent<T>();
                if (component != null)
                    return component;
            }
        }
        return null;
    }
}
