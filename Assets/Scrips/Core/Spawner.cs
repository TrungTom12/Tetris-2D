using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;

    void Start()
    {
        InitQueue();

        Vector2 originalVector = new Vector2(4.3f, 1.3f);
        Vector2 newVector = Vectorf.Round(originalVector);

        Debug.Log(newVector.ToString());
    }

    Shape GetRandomShape() //Khởi tạo hàm random , lấy random các shape 
    {
        int i = Random.Range(0, m_allShapes.Length);
        if (m_allShapes[i])
        {
            return m_allShapes[i];
        }
        else
        {
            Debug.LogWarning("WARNING! Invalid shape is spawner");
            return null;
        }
    }

    public Shape SpawnerShape() // lặp lại các shape
    {
        Shape shape = null; // tạo biến shape

        //sinh ra cac shape theo queueShape
        //shape = GetQueueShape();
        //shape.transform.position = transform.position;
        //shape.transform.localScale = Vector3.one;

        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape; // tạo shape 
        if (shape)
        {
            return shape;
        }
        else
        {
            Debug.LogWarning("WARNING! Invalid shape in spawner");
            return null;
        }
    }

    public Transform[] m_queueXforms = new Transform[3];
    Shape[] m_queueShapes = new Shape[3];

    float m_queueScale = 0.4f;

    void InitQueue()
    {
        for (int i = 0; i < m_queueShapes.Length; i++)
        {
            m_queueShapes[i] = null;

        }
        FillQueue();
    }

    void FillQueue()
    {
        for (int i = 0; i < m_queueShapes.Length; i++)
        {
            if (!m_queueShapes[i])
            {
                m_queueShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
                m_queueShapes[i].transform.position = m_queueXforms[i].position + m_queueShapes[i].m_queueOffset;
                m_queueShapes[i].transform.localScale = new Vector3(m_queueScale, m_queueScale, m_queueScale);
            }
        }    
    }

    Shape GetQueueShape()
    {
        Shape firstShape = null;

        if (m_queueShapes[0])
        {
            firstShape = m_queueShapes[0];
        }

        for (int i = 1; i < m_queueShapes.Length; i++)
        {
            m_queueShapes[i-1] = m_queueShapes[i];
            m_queueShapes[i-1].transform.position = m_queueXforms[i-1].position + m_queueShapes[i].m_queueOffset;
        }

        m_queueShapes[m_queueShapes.Length - 1] = null;

        FillQueue();

        return firstShape;
    }

    

    void Update()
    {
        
    }
}

