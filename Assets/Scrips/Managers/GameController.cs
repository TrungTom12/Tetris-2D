 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameController : MonoBehaviour
{
    Board m_gameBoard;
    Spawner m_spawner;
    Shape m_activeShape;
    SoundManager m_soundManager;
    ScoreManager m_scoreManager;

    public float m_dropInterval = 0.2f;
    float m_timeToDrop;

    //float m_timeToNextKey;
    //[Range(0.02f, 1f)]
    //public float m_keyRepeatRate = 0.25f;

    // KHai bao rieng tie cua tung huong

    float m_timeToNextKeyLeftRight;
    [Range(0.02f, 1f)]
    public float m_keyRepeatRateLeftRight = 0.15f;

    float m_timeToNextKeyDown;
    [Range(0.01f, 1f)]
    public float m_keyRepeatRateDown = 0.01f;

    float m_timeToNextKeyRotate;
    [Range(0.02f, 1f)]
    public float m_keyRepeatRateRotate = 0.10f;

    bool m_gameOver = false;

    public GameObject m_gameOverPanel;

    //Toggle button
    public IconToggle m_rotateToggle;

    bool m_clockwise = true;
    

    void Start()
    {
        // find spawner and board with GameObject.FindWithTag plus GetComponent; make sure tag your object correctly
        // find spawner and board with generic version of GameObject.FindObjectOfTyp, slower but less typing 
        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;


        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined");
        }
        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined");
        }
        if (!m_soundManager)
        {
            Debug.LogWarning("Warning! There is no soundManager defined");
        }
        if (!m_scoreManager)
        {
            Debug.Log("Warning! There is no scoreManager Defined");
        }
        else // tạo shape và lặp lại tại vị trí đó 
        {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
            if (!m_activeShape)
            {
                m_activeShape = m_spawner.SpawnerShape();
            }
        }

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }




    }

    void Update()
    {
        if (!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver ||!m_soundManager ||!m_scoreManager)
        {
            return;
        }
        PlayerInput();
    }


    private void PlayerInput() // Điều khiển sử dụng tap || hold để di chuyển trái phải 
    {
        if (Input.GetButton("MoveRight") && (Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveRight"))
        {
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            

            if (!m_gameBoard.InValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
                PlaySound(m_soundManager.m_errorSound,0.5f);
            }   
            else
            {
                PlaySound(m_soundManager.m_moveSound,0.5f);  //chạy âm thanh khi di chuyển 
            }
        }

        else if (Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftRight ) || Input.GetButtonDown("MoveLeft"))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

         

            if (!m_gameBoard.InValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);  //chạy âm thanh khi di chuyển 
            }
        }           

        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKeyRotate))
        {
            //m_activeShape.RotateRight();  
            m_activeShape.RotateClockwise(m_clockwise);
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;


            if (!m_gameBoard.InValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);  //chạy âm thanh khi di chuyển 
            }
        }

        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || (Time.time > m_timeToDrop)) //Khi shape chạm đến đáy board sẽ dừng lại 
        {
            //
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
            // khởi tạo thời gian rơi = thời gian trong 1 fram + thời gian thả 
            m_timeToDrop = Time.time + m_dropInterval;
            // Sau khi shape di chuyển xuống dưới cùng của board , vào hàm mà t/m khớp thì moveup 
            m_activeShape.MoveDown();

            //PlaySound(m_soundManager.m_dropSound, 0.75f); //chạy âm thanh khi di chuyển 

            if (!m_gameBoard.InValidPosition(m_activeShape))
            {
                if (m_gameBoard.IsOverLimit(m_activeShape))
                {
                    GameOver();
                }
                else  
                {
                    LandShape();
                }
            }
   
        }
        else if (Input.GetButtonDown("RotateChange"))
        {
            ToogleRotDirection();
        }

    }

    void PlaySound(AudioClip clip , float volMultiplier)
    {
        if (clip && m_soundManager.m_fxEnabled) // Xử lý âm thanh khi di chuyển trái || phải || lên || xuống 
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));
        }
    }

    private void GameOver()
    {
        m_activeShape.MoveUp();
        m_gameOver = true;
        Debug.LogWarning(m_activeShape.name + " is over the limit ");

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }

        PlaySound(m_soundManager.m_gameOverSound, 5f);
        PlaySound(m_soundManager.m_gameOverVocalClips, 5f);

        //PlaySound(m_soundManager.m_dropSound, 0.75f); //chạy âm thanh khi di chuyển 
        // set the gameOver condition to true

    }

    private void LandShape()
    { 
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        PlaySound(m_soundManager.m_dropSound, 0.75f);  //chạy âm thanh khi di chuyển 

        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnerShape(); // tạo lại shape sau khi đã hoàn thành xong DK
        
        m_gameBoard.ClearAllRows(); // xóa hàng cuối

        //if (m_soundManager.m_fxEnabled && m_soundManager.m_dropSound) // Xử lý âm thanh khi drop shape
        //{
        //    AudioSource.PlayClipAtPoint(m_soundManager.m_dropSound, Camera.main.transform.position, m_soundManager.m_fxVolume);
        //}

        if (m_gameBoard.m_completedRows > 0)
        {
            m_scoreManager.ScoreLines(m_gameBoard.m_completedRows); // lay cach tinh diem 

            if (m_gameBoard.m_completedRows > 1)
            {
                AudioClip randomVoval = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
                PlaySound(randomVoval,0f);
            }
        }
        PlaySound(m_soundManager.m_clearRowSound, 0f); // 
    }

    public void Restart()
    {
        Debug.Log("Restarted");
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ToogleRotDirection()
    {
        m_clockwise = !m_clockwise;
        if (m_rotateToggle)
        {
            m_rotateToggle.Toggle(m_clockwise);
        }
    }


}
