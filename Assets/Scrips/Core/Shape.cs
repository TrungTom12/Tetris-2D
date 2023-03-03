 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool m_canRotate = true;
    // khởi tạo hướng di chuyển 
    public void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }
    // khởi khởi tạo di chuyển theo 4 hướng 
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    // khởi tạo xoay theo hướng trái && hướng phải (DK : check có thể xoay )
    public void RotateRight ()
    {
        if (m_canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }

    public void RotateLeft ()
    {
        if (m_canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }


    public void RotateClockwise (bool clockwise) 
    {
        if (clockwise)
        {
            RotateRight();
        }else
        {
            RotateLeft();
        }
    }


    void Start()
    {
        //InvokeRepeating("MoveDown",0,0.5f);
       // InvokeRepeating("MoveRight", 0, 2f);
    }

    void Update()
    {
        
    }
}
