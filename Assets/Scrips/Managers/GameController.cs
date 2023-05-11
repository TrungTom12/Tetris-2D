﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    GameManager m_gameManager;
    Board m_gameBoard;
    Spawner m_spawner;
    Shape m_activeShape;
    SoundManager m_soundManager;
    ScoreManager m_scoreManager;
    Ghost m_ghost;
    Hold m_hold;
    public float m_dropInterval = 0.1f;
    float m_dropIntervalModded;

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
      
    public bool m_isPaused = false;

    public GameObject m_pausePanel;


    

    void Start()
    {
        // find spawner and board with GameObject.FindWithTag plus GetComponent; make sure tag your object correctly
        // find spawner and board with generic version of GameObject.FindObjectOfTyp, slower but less typing 
        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        m_ghost = GameObject .FindObjectOfType<Ghost>();    
        m_hold = GameObject.FindObjectOfType<Hold>();

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

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }

        if (diagnosticText)
        {
            diagnosticText.text = "";
        }
        m_dropIntervalModded = m_dropInterval;


    }

    void Update()
    {
        if (!m_ghost || !m_gameBoard || !m_spawner || !m_activeShape || m_gameOver || !m_soundManager || !m_scoreManager)
        {
            return;
        }
        PlayerInput();
        
        
    }

    void LateUpdate()
    {
        m_ghost.DrawGhost(m_activeShape, m_gameBoard);
    }

    enum Direction {none,left,right,up,down };
    Direction m_swipeDirection = Direction.none;
    Direction m_swipeEndDirection = Direction.none; 
    private void PlayerInput() // Điều khiển sử dụng tap || hold các nút để di chuyển trái phải 
    {
        //Sử dụng cho PC
        if ((Input.GetButton("MoveRight") && (Time.time > m_timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveRight"))
        {
            MoveRight();
        }

        else if ((Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftRight )) || Input.GetButtonDown("MoveLeft"))
        {
            MoveLeft();
        }

        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKeyRotate))
        {
            Rotate();
        }

        else if ((Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown)) || (Time.time > m_timeToDrop)) //Khi shape chạm đến đáy board sẽ dừng lại 
        {
            MoveDown();

        }

        //Sử dụng cho Smartphone 
        else if ((m_swipeDirection == Direction.right && Time.time > m_timeToNextKeyLeftRight) || m_swipeEndDirection == Direction.right)
        {
            MoveRight();
            m_swipeDirection = Direction.none;
            m_swipeEndDirection = Direction.none;
        }

        else if ((m_swipeDirection == Direction.left && Time.time > m_timeToNextKeyLeftRight) || m_swipeEndDirection == Direction.left)
        {
            MoveLeft();
            m_swipeDirection = Direction.none;
            m_swipeEndDirection = Direction.none;
        }

        else if (m_swipeEndDirection == Direction.up)
        {
            Rotate();
            m_swipeEndDirection = Direction.none;
        }

        else if (m_swipeDirection == Direction.down && Time.time > m_timeToNextKeyDown)
        {
            MoveDown();
            m_swipeDirection = Direction.none;
        }

        //Active button
        else if (Input.GetButtonDown("RotateChange")) // Gán trực tiếp nút Rotate
        {
            ToogleRotDirection();
        }

        else if (Input.GetButtonDown("Pause")) // Gán trực tiếp nút Pause
        {
            TogglePause();
        }

        else if (Input.GetButtonDown("Restart"))
        {
            Restart();
        }

    }

    //Move
    private void MoveDown()
    {
        m_timeToDrop = Time.time + m_dropIntervalModded;
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

    private void Rotate()
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

    private void MoveLeft()
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

    private void MoveRight()
    {
        m_activeShape.MoveRight();
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;



        if (!m_gameBoard.InValidPosition(m_activeShape))
        {
            m_activeShape.MoveLeft();
            PlaySound(m_soundManager.m_errorSound, 0.5f);
        }
        else
        {
            PlaySound(m_soundManager.m_moveSound, 0.5f);  //chạy âm thanh khi di chuyển 
        }
    }

    void PlaySound(AudioClip clip , float volMultiplier)
    {
        if (clip && m_soundManager.m_fxEnabled) // Xử lý âm thanh khi di chuyển trái || phải || lên || xuống 
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));
        }
    }

    public bool m_isGameOver = false;
    private void GameOver()
    {
        m_activeShape.MoveUp();
        
        Debug.LogWarning(m_activeShape.name + " is over the limit ");

        m_isGameOver = !m_isGameOver;

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);

        }

        PlaySound(m_soundManager.m_gameOverSound, 5f);
        PlaySound(m_soundManager.m_gameOverVocalClips, 5f);

        if (m_soundManager)
        {
            m_soundManager.m_musicSource.volume = (m_isGameOver) ? m_soundManager.m_musicVolume * 0f : m_soundManager.m_musicVolume;
        }
        Time.timeScale = (m_isGameOver) ? 0 : 1;
        //PlaySound(m_soundManager.m_dropSound, 0.75f); //chạy âm thanh khi di chuyển 
        // set the gameOver condition to true
        m_gameOver = true;

    }

    private void LandShape()
    { 
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        
        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnerShape(); // tạo lại shape sau khi đã hoàn thành xong DK
        
        m_gameBoard.ClearAllRows(); // xóa hàng cuối

        if (m_ghost)
        {
            m_ghost.Reset();
        }

        //if (m_soundManager.m_fxEnabled && m_soundManager.m_dropSound) // Xử lý âm thanh khi drop shape
        //{
        //    AudioSource.PlayClipAtPoint(m_soundManager.m_dropSound, Camera.main.transform.position, m_soundManager.m_fxVolume);
        //}

        PlaySound(m_soundManager.m_dropSound, 0.5f);  //chạy âm thanh khi di chuyển 

        if (m_gameBoard.m_completedRows > 0)
        {
            m_scoreManager.ScoreLines(m_gameBoard.m_completedRows); // lay cach tinh diem 

            if (m_scoreManager.m_didLeveUp)
            {
                //Xóa 1 hoặc nhiều hàng và tăng cấp
                PlaySound(m_soundManager.m_levelUpVocalClip,4f);
                Debug.Log("Xóa 1 hoặc nhiều hàng và tăng cấp");
                m_dropIntervalModded = Mathf.Clamp(m_dropInterval - (((float) m_scoreManager.m_level - 1) * 0.1f),0.05f,1f);
            }
            else
            {
                if (m_gameBoard.m_completedRows > 1)
                {
                    //Xóa nhiều hơn 1 hàng
                    AudioClip randomVocal = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
                    PlaySound(randomVocal, 40f);
                    Debug.Log("Xóa nhiều hơn 1 hàng");
                }
            }

            //Xóa 1 hàng 
            PlaySound(m_soundManager.m_clearRowSound, 3f);
            Debug.Log("Xóa 1 hàng");
        }
        //PlaySound(m_soundManager.m_clearRowSound, 3f); // 
    }

    //Button

    public void Restart()
    {
        Time.timeScale = 1f;
        Debug.Log("Restarted");
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ToogleRotDirection() // gán vào button Rotate 
    {
        m_clockwise = !m_clockwise;
        if (m_rotateToggle)
        {
            m_rotateToggle.Toggle(m_clockwise);
        }
    }

    public void TogglePause()
    {
        if (m_gameOver)
        {
            return;
        }

        m_isPaused = !m_isPaused;

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(m_isPaused);

            if (m_soundManager)
            {
                m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume * 0f : m_soundManager.m_musicVolume;
            }
            Time.timeScale = (m_isPaused) ? 0: 1;
        }
    }
    
    public void Hold()
    {
        m_hold.Catch(m_activeShape);
        m_activeShape = m_spawner.SpawnerShape();
    }

    public Text diagnosticText;
    void OnEnable()
    {
        TouchController.SwipeEvent += SwipeHandler;
        TouchController.SwipeEventEnd += SwipeEndHandler;
    }
     
    private void OnDisable()
    {
        TouchController.SwipeEvent -= SwipeHandler;
        TouchController.SwipeEventEnd -= SwipeEndHandler;
    }

    void SwipeHandler(Vector2 swipeMovement)
    {
        //if (diagnosticText)
        //{
        //    diagnosticText.text = "SwipeEvent Detected";
        //}
        m_swipeDirection = GetDirection(swipeMovement);
    }

    void SwipeEndHandler(Vector2 swipeMovement)
    {
        //if (diagnosticText)
        //{
        //    diagnosticText.text = "";
        //}
        m_swipeEndDirection = GetDirection(swipeMovement);
    }

    Direction GetDirection(Vector2 swipeMovement)
    {
        Direction swipeDir = Direction.none;

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
        }
        else
        {
            swipeDir = (swipeMovement.y >= 0) ? Direction.up : Direction.down;
        }
        return swipeDir;
    }
} 
