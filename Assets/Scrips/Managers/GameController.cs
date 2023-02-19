 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameController : MonoBehaviour
{
    Board m_gameBoard;
    Spawner m_spawner;
    Shape m_activeShape;

    float m_dropInterval = 0.5f;
    float m_timeToDrop;

    float m_timeToNextKey;
    [Range(0.02f, 1f)]
    public float m_keyRepeatRate = 0.25f;
    
    void Start()
    {
        // khởi tạo thông qua Tag 
        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined");
        }
        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined");
        }
        else // tạo shape và lặp lại tại vị trí đó 
        {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
            if (!m_activeShape)
            {
                m_activeShape = m_spawner.SpawnerShape();
            }
            
        }
    }

    void Update()
    {
        if (!m_gameBoard || !m_spawner || !m_activeShape)
        {
            return;
        }
        PlayerInput();
    }


    private void PlayerInput() // Điều khiển sử dụng tap || hold để di chuyển trái phải 
    {
        if (Input.GetButton("MoveRight") && (Time.time > m_timeToNextKey) || Input.GetButtonDown("MoveRight"))
        {
            m_activeShape.MoveRight();
            m_timeToNextKey = Time.time + m_keyRepeatRate;

            if (!m_gameBoard.InValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
            }
        }

        else if (Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKey) || Input.GetButtonDown("MoveLeft"))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKey = Time.time + m_keyRepeatRate;

            if (!m_gameBoard.InValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
            }
        }           

        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKey))
        {
            m_activeShape.RotateRight();
            m_timeToNextKey = Time.time + m_keyRepeatRate;
            if (!m_gameBoard.InValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
            }
        }

        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKey) || (Time.time > m_timeToDrop)) //Khi shape chạm đến đáy board sẽ dừng lại 
        {
            //
            m_timeToNextKey = Time.time + m_keyRepeatRate;
            // khởi tạo thời gian rơi = thời gian trong 1 fram + thời gian thả 
            m_timeToDrop = Time.time + m_dropInterval;
            // Sau khi shape di chuyển xuống dưới cùng của board , vào hàm mà t/m khớp thì moveup 
            m_activeShape.MoveDown();
            if (!m_gameBoard.InValidPosition(m_activeShape))
            {
                    LandShape();
            }
        }


    }

    private void LandShape()
    { 
        m_timeToNextKey = Time.time;
        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnerShape(); // tạo lại shape sau khi đã hoàn thành xong DK
        //m_gameBoard.CleanAllRows(); // xóa hàng cuối
    }
    
    //void Restart()
    //{
    //    Debug.Log("Restarted");
    //    Application.LoadLevel(Application.loadedLevel);
    //} 



  
}
