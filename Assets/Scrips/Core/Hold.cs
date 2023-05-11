using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold : MonoBehaviour
{
    public Transform m_holdXform;
    public Shape m_heldShape = null;
    float m_scale = 0.5f;

    public void Catch(Shape shape)
    {
        if (m_heldShape)
        {
            Debug.LogWarning("HHHH");
            return;
        }

        if(!shape)
        {
            Debug.LogWarning("Khac shape");
            return;
            
        }

        if (m_holdXform)
        {
            shape.transform.position = m_holdXform.position + shape.m_queueOffset;
            shape.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
        }
        else
        {
            Debug.LogWarning("56565");
        }
    }
}
