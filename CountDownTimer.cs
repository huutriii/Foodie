using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] TMP_Text _timeText;
    float _duration = 300f;

    float _timeLeft;
    bool _isRunning;

    void Start()
    {
        _timeLeft = _duration;
        _isRunning = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        int munite = Mathf.FloorToInt(_timeLeft / 60);
        int seconds = Mathf.FloorToInt(_timeLeft % 60);

        _timeText.text = string.Format("{0:00}:{1:00}", munite, seconds);
    }

    
    void Update()
    {
        if (!_isRunning) return;

        if( _timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            _timeLeft = 0;
            _isRunning = false;
            UpdateUI();
            OnTimeUp();
        }
    }

    public void OnTimeUp()
    {
        Debug.Log("Het gio");
    }
}
