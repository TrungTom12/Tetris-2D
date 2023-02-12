 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameController : MonoBehaviour
{
    Board m_gameBoard;

    Spawner m_spawner;

    Shape m_activeShape;

    float m_dropInterval = 0.25f;
    float m_timeToDrop;
    
    void Start()
    {
        //m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        // khởi tạo thông qua Tag 
        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();

        if (m_spawner) // tạo shape và lặp lại tại vị trí đó 
        {
            if (m_activeShape == null)
            {
                m_activeShape = m_spawner.SpawnerShape();
            }
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined");
        }
        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined");
        }
    }

    void Update()
    {
        // Nếu không có 1 trong hai thì sẽ không chạy game
        if (!m_gameBoard || !m_spawner)
        {
            return;
        }

        if (Time.time > m_timeToDrop) //Khi shape chạm đến đáy board sẽ dừng lại 
        {
            // khởi tạo thời gian rơi = thời gian trong 1 fram + thời gian thả 
            m_timeToDrop = Time.time + m_dropInterval;
            if (m_activeShape)
            {
                // 
                m_activeShape.MoveDown();
                // Sau khi shape di chuyển xuống dưới cùng của board , vào hàm mà t/m khớp thì moveup
                if (!m_gameBoard.InValidPosition(m_activeShape))
                {
                    //Shape landing 
                    m_activeShape.MoveUp();
                    m_gameBoard.StoreShapeInGrid(m_activeShape);

                    if (m_spawner) // tạo lại shape sau khi đã hoàn thành xong DK 
                    {
                        m_activeShape = m_spawner.SpawnerShape();
                    }
                }
            }
        }
    }
}
