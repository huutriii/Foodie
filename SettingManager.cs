using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] float _speed = 500f;
    float targetX;
    [SerializeField] RectTransform _toggleParent;
    [SerializeField] RectTransform _toggleKnob;
    [SerializeField] GameObject _onTrackSound;
    [SerializeField] GameObject _offTrackSound;
    bool _isToggle = true;
    bool _isMoving = false;
    const float POS_ON  =  30f;   // Pos X khi bật
    const float POS_OFF  = -60f;  // Pos X khi tắt

    const string TOGGLE_KEY = "ToggleEnable";

    private void Awake()
    {
        _toggleKnob = (RectTransform)_toggleParent.GetChild(2);
        _offTrackSound = _toggleParent.GetChild(0).gameObject;
        _onTrackSound = _toggleParent.GetChild(1).gameObject;
        targetX = _toggleKnob.anchoredPosition.x;

        Init();
    }
    public void OnCloseSetting()
    {
        Scene scene = gameObject.scene;

        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene);

            AudioManager.Instance.PlayChangeChar();
        }
    }

    public void BackHome(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        AudioManager.Instance.PlayChangeChar();
    }

    private void Update()
    {
        if (!_isMoving) return;

        Vector2 pos = _toggleKnob.anchoredPosition;

        pos.x = Mathf.MoveTowards(pos.x, targetX, _speed * Time.deltaTime);
        _toggleKnob.anchoredPosition = pos;

        if (pos.x == targetX)
            _isMoving = false;
    }

    public void Init()
    {
        int savedVal = PlayerPrefs.GetInt(TOGGLE_KEY, 1);

        if (savedVal == 1)
        {
            _onTrackSound.SetActive(true);
            _offTrackSound.SetActive(false);
        }
        else
        {
            _onTrackSound.SetActive(false);
            _offTrackSound.SetActive(true);
        }
    }

    public void Toggle()
    {
        _isToggle = !_isToggle;
        AudioManager.Instance.SetMusic(_isToggle);
        PlayerPrefs.SetInt(TOGGLE_KEY, _isToggle ? 1 : 0);
        Init();
        _isMoving = true;
        targetX = _isToggle ? POS_ON : POS_OFF;
    }
}
