using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShape;

    Shape GetRandomShape() //Khởi tạo hàm random , lấy random các shape 
    {
        int i = Random.Range(0, m_allShape.Length);
        if (m_allShape[i])
        {
            return m_allShape[i];
        }
        else
        {
            Debug.LogWarning("WARNING! Invalid shape is spawner");
            return null;
        }
    }
    public Shape SpawnerShape() // lặp lại ca **
    {
        Shape shape = null; // tạo biến shape
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

    void Start()
    {
        Vector2 originalVector = new Vector2(4.3f, 1.3f);
        Vector2 newVector = Vectorf.Round(originalVector);

        Debug.Log(newVector.ToString());
    }

    void Update()
    {
        
    }
}
