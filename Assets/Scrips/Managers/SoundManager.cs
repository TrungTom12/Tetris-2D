using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool m_musicEnabled = true;

    public bool m_fxEnabled = true;

    [Range(0, 1)]
    public float m_musicVolume = 1.0f;
    [Range(0, 1)]
    public float m_fxVolume = 1.0f;
    [Range(0, 1)]
    public float m_VocalClips = 1.0f;
    public AudioClip m_clearRowSound;

    public AudioClip m_moveSound;

    public AudioClip m_dropSound;

    public AudioClip m_gameOverSound;

    //public AudioClip m_backgroundMusic;

    public AudioSource m_musicSource;

   

    AudioClip m_randomMusicClip;

    private AudioClip randomClip;

    public AudioClip m_errorSound;

    public AudioClip[] m_vocalClips;

    public AudioClip m_gameOverVocalClips;

    //backGround music clip
    public AudioClip[] m_musicClips;
    //
    public IconToggle m_musicIconToggle;
    public IconToggle m_fxIconToggle;

    //private bool musicClip;
    void Start()
    {
        m_randomMusicClip = GetRandomClip(m_musicClips);
        PlayBachgroundMusic(m_randomMusicClip);

        //PlayBachroundMusic(GetRandomClip(m_musicClipsClips));

    }

    public AudioClip GetRandomClip(AudioClip[] clips) //Random nhạc
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }


    void Update()
    {

    }

    public void PlayBachgroundMusic(AudioClip musicCip)
    {
        //Trả về nếu nhạc không được kích hoạt hoặc music Source vô gtri hoặc musicClip vô gtri
        if (!m_musicEnabled /*|| !musicClip*/ || !m_musicSource)
        {
            return;
        }

        // nhạc chạy , dừng
        m_musicSource.Stop();

        m_musicSource.clip = musicCip;

        //thiết lập volume
        m_musicSource.volume = m_musicVolume;

        //lặp lại nhạc
        m_musicSource.loop = true;

        //Chơi nhạc 
        m_musicSource.Play();
    }



    void UpdateMusic()
    {
        if (m_musicSource.isPlaying != m_musicEnabled)
        {
            if (m_musicEnabled)
            {
                m_randomMusicClip = GetRandomClip(m_musicClips);
                PlayBachgroundMusic(m_randomMusicClip);
                //PlayBachgroundMusic(m_backgroundMusic);
            }
            else
            {
                m_musicSource.Stop();
            }
        }
    }



    public void ToggleMusic() // gán vào button 
    {
        m_musicEnabled = !m_musicEnabled;
        UpdateMusic();

        if (m_musicIconToggle)
        {
            m_musicIconToggle.Toggle(m_musicEnabled);
        }
    }

    public void ToggleFX()
    {
        m_fxEnabled = !m_fxEnabled;

        if (m_fxIconToggle)
        {
            m_fxIconToggle.Toggle(m_fxEnabled);
        }
    }


}
