using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    int m_score = 0;
    int m_line;
    public int m_level = 1;

    public int m_linePerLevel = 5;

    const int m_minLines = 1;
    const int m_maxLines = 4;

    public Text m_lineText;
    public Text m_levelText;
    public Text m_scoreText;

    public bool m_didLeveUp = false; 
    public void ScoreLines(int n)
    {
        m_didLeveUp= false;
         n = Mathf.Clamp(n, m_minLines,m_maxLines);

        switch (n)
        {
            case 1: 
                m_score += 40 * m_level; 
                break;
            case 2:
                m_score += 100 * m_level;
                break;
            case 3:
                m_score += 300 * m_level;
                break;
            case 4:
                m_score += 1200 * m_level;
                break;

        }
        m_line -= n;

        if (m_line <= 0)
        {
            LevelUp();
        }

        UpdateUIText();
        
    }

    public void Reset()
    {
        m_level = 1;
        m_line = m_linePerLevel * m_level;

        UpdateUIText() ;
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    void UpdateUIText()
    {
        if (m_lineText)
        {
            m_lineText.text = m_line.ToString();
        }

        if (m_levelText)
        {
            m_levelText.text = m_level.ToString();
        }

        if (m_scoreText)
        {
            m_scoreText.text = PadZero(m_score, 5);//m_score.ToString();
        }
    }

    string PadZero(int n , int padDigits)
    {
        string nStr = n.ToString();

        while (nStr.Length < padDigits)
        {
            nStr = "0" + nStr;
        }
        return nStr;
    }

    public void LevelUp()
    {
        m_level++;
        m_line = m_linePerLevel * m_level;
        m_didLeveUp = true;
    }
}
