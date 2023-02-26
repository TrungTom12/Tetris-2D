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

    public AudioClip m_cleanRowSound;

    public AudioClip m_moveSound;

    public AudioClip m_dropSound;

    public AudioClip m_gameOverSound;

    //public AudioClip m_backgroundMusic;

    public AudioSource m_musicSource;

    //backGround music clip
    public AudioClip[] m_musicClips;

    AudioClip m_randomMusicClip;
    private AudioClip randomClip;

    public AudioClip m_errorSound;
    
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
    }


}
