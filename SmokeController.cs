using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(IE_AutoDeactive());
    }

    public IEnumerator IE_AutoDeactive()
    {
        yield return null;

        float duration = _animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(duration);

        this.gameObject.SetActive(false);
    }
}
