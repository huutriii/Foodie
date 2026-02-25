using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get; private set;
    }

    [SerializeField] AudioSource _bgmSource;
    [SerializeField] AudioClip _bgmClip;

    [SerializeField] AudioSource _sfxSource;
    [SerializeField] AudioClip _changeCharClip;
    [SerializeField] AudioClip _mergeClip;
    [SerializeField] AudioClip _pickUpClip;
    [SerializeField] AudioClip _popSmokeClip;
    [SerializeField] AudioClip _shuffleFoodClip;

    bool _isMute;

    const string MUSIC_KEY = "MusicEnable";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        _bgmSource.clip = _bgmClip;
        _bgmSource.loop = true;

        int savedVal = PlayerPrefs.GetInt(MUSIC_KEY, 1);

        if (savedVal == 1)
        {
            _isMute = false;
            _bgmSource.Play();
        }
        else
        {
            _isMute = true;
            _bgmSource.Stop();
        }
    }

    public void SetMusic(bool enable)
    {
        if (enable == true)
        {
            _isMute = false;
            PlayerPrefs.SetInt(MUSIC_KEY, 1);
            _bgmSource.Play();
        }
        else
        {
            _isMute = true;
            PlayerPrefs.SetInt(MUSIC_KEY, 0);
            _bgmSource.Stop();
        }


        PlayerPrefs.Save();
    }

    public void PlayChangeChar() => _sfxSource.PlayOneShot(_changeCharClip);
    public void PlayMerge()=>_sfxSource.PlayOneShot(_mergeClip);
    public void PlayPickUp()=>_sfxSource.PlayOneShot(_pickUpClip);
    public void PlayPopSmoke()=>_sfxSource.PlayOneShot(_popSmokeClip);
    public void PlayShuffleFood()=>_sfxSource.PlayOneShot(_shuffleFoodClip);

    public int GetMusicKey() => PlayerPrefs.GetInt(MUSIC_KEY, 1);

}
