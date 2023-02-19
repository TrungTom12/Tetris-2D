using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform  m_emptySprite;

    public int m_height = 22;
    public int m_width = 10;
    int m_heightAndWidth;

    Transform[,] m_grid;

    void Awake()
    {
        m_grid = new Transform[m_width, m_height];    
    }
    void Start()
    {
        DrawEmptyCells();
    }

   
    void Update()
    {
        
    }

    bool IsWithinBoard(int x, int y) // kiểm tra giới hạn của board 
    {
        return (x >= 0 && x < m_width && y >= 0 );
    }

    bool IsOccupied(int x, int y, Shape shape)
    {
        return (m_grid[x, y] != null && m_grid[x, y].parent != shape.transform);
    }

    public bool InValidPosition(Shape shape) // kiểm tra xem shape đã chạm dến giới hạn của board hay chưa 
    {
        foreach (Transform child in shape.transform) // check các biến shape con 
        {
            Vector2 pos = Vectorf.Round(child.position); // gán vector cho vị trí shape con 

            if (!IsWithinBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if (IsOccupied((int)pos.x,(int) pos.y,shape))
            {
                return false;
            }
        }
        return true;
    }


    void DrawEmptyCells() // Tạo Bảng game 
    {
        if (m_emptySprite != null)
        {
            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Transform clone;
                    clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform; // Tạo ra Obj ảo theo vector3 
                    clone.name = "Board Space( x = " + x.ToString() + "y =" + y.ToString() + ")"; //Thêm tên cho Oj ảo (có sử dung ép kểu int -> string)
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("WARNING");
        }

    }

    public void StoreShapeInGrid(Shape shape) //Check vaf shape chồng shape
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            m_grid[(int)pos.x, (int)pos.y] = child;
        }
    }



    //bool IsComplete(int y)
    //{
    //    for (int x = 0; x < m_width; ++x)
    //    {
    //        if (m_grid[x,y] == null)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //void CleanRow(int y)
    //{
    //    for (int x = 0; x < m_width; x++)
    //    {
    //        if (m_grid[x,y] != null)
    //        {
    //            Destroy(m_grid[x, y].gameObject);
    //        }
    //    }
    //    m_grid= null;
    //}

    //void ShiftOneRowDown(int y)
    //{
    //    for (int x = 0; x < m_width; x++)
    //    {
    //        if (m_grid[x, y] != null)
    //        {
    //            m_grid[x, y - 1] = m_grid[x, y];
    //            m_grid[x, y] = null;
    //            m_grid[x, y - 1].position += new Vector3(0, -1, 0);
    //        }   
    //    }
    //}

    //void ShiftRowDown(int StartY)
    //{
    //    for (int i = 0; i < m_width; i++)
    //    {
    //        ShiftOneRowDown(i);
    //    }
    //}

    //public void CleanAllRows()
    //{
    //    for (int y = 0; y < m_width; ++y)
    //    {
    //        if (IsComplete(y))
    //        {
    //            CleanRow(y);
    //            ShiftRowDown(y+1);
    //            y--;
    //        }
    //    }
    //}

}
