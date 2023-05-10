using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
là một thuộc tính (attribute) trong Unity,
nó được sử dụng để yêu cầu rằng game object nhất định phải
có thành phần (component) MaskableGraphic được gắn vào.
*/

/*
khi dc thêm vào class của script được áp dụng trên game object, 
nếu game object đó không chứa thành phần MaskableGraphic, 
Unity sẽ tự động thêm component này vào. 
Điều này giúp đảm bảo rằng game object luôn có thành phần MaskableGraphic 
để sử dụng trong code hay inspector và tránh gặp lỗi khi thực thi runtime.
*/

[RequireComponent(typeof(MaskableGraphic))]

public class ScreenFader : MonoBehaviour
{
    public float m_startAlpha = 1f;
    public float m_targetAlpha = 0f;
    public float m_delay = 0f;
    public float m_timetoFade = 1f;

    float m_inc;
    float m_currentAlpha;
    MaskableGraphic m_graphic;
    Color m_originalColor;

    void Start()
    {
        m_graphic = GetComponent<MaskableGraphic>();

        m_originalColor = m_graphic.color; 

        m_currentAlpha = m_startAlpha;

        Color tempColor = new Color(m_originalColor.r, m_originalColor.g, m_originalColor.b, m_currentAlpha);

        m_graphic.color = tempColor;

        m_inc = ((m_targetAlpha - m_startAlpha) / m_timetoFade) * Time.deltaTime;

        StartCoroutine("FadeRoutine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(m_delay) ;

        while (Mathf.Abs(m_targetAlpha-m_currentAlpha) > 0.01f)
        {
            yield return new WaitForEndOfFrame();

            m_currentAlpha = m_currentAlpha + m_inc;

            Color tempColor = new Color(m_originalColor.r, m_originalColor.g, m_originalColor.b, m_currentAlpha);

            m_graphic.color = tempColor;

        }
        Debug.Log("SCREen Fader  fin "); 
    }
}
