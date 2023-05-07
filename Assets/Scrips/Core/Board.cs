using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform m_emptySprite;

    public int m_height = 30;
    public int m_width = 10;
    public int m_header = 8;

    int m_heightAndWidth;

    Transform[,] m_grid;

    public int m_completedRows = 0;

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
        return (x >= 0 && x < m_width && y >= 0); // xem kĩ lại
    }

    bool IsOccupied(int x, int y, Shape shape) // kiểm tra vị trí hiện tại đã có shape chưa 
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

            if (IsOccupied((int)pos.x, (int)pos.y, shape))
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
            for (int y = 0; y < m_height - m_header; y++)
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

    public void StoreShapeInGrid(Shape shape) // Lưu shape tại vị trí trên board
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



    bool IsComplete(int y) // Kiểm tra hoàn thiện của hàng ngang
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] == null)
            {
                return false;
            }
            
        }
        Debug.Log("Đủ");
        return true;
    }

    void ClearRow(int y) // Xóa hàng
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] != null)
            {
                Destroy(m_grid[x, y].gameObject);
                Debug.Log("Đã xóa !!!");
            }
            m_grid[x,y] = null;
        }
    }

    void ShiftOneRowDown(int y) // Chuyển xuống 1 hàng 
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] != null)
            {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    void ShiftRowsDown(int startY) // Chuyển xuống nhiều hàng
    {
        for (int i = startY; i < m_height; ++i)
        {
            ShiftOneRowDown(i);
        }
    }

    public void ClearAllRows() // Xóa các hàng đã hoàn thiện 
    {
        m_completedRows = 0; //neu khong co dong nay se lien tiep ++ diem

        for (int y = 0; y < m_height; ++y)
        {
            if (IsComplete(y))
            {
                m_completedRows++;
                ClearRow(y);
                ShiftRowsDown(y + 1);
                y--;
            }
        }
    }



    public bool IsOverLimit(Shape shape) // Kiểm tra vượt quá giới hạn
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= (m_height - m_header - 1))
            {
                return true;
            }
            //Debug.Log(" vuot qua gioi hạn !!!");
        }
        return false;
    }

}
