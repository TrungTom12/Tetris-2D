using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    //public delegate void MathDelagate(int num1, int num2);

    public delegate void TouchEvenHandler(Vector2 swipe);

    public static event TouchEvenHandler SwipeEvent;

    public static event TouchEvenHandler SwipeEventEnd;

    Vector2 m_touchMovement ;

    int m_minSwipeDistance = 20;
    void OnSwipe()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(m_touchMovement);
        }
        
    }

    void OnSwipeEnd()
    {
        if (SwipeEventEnd != null)
        {
            SwipeEventEnd(m_touchMovement);
        }
    }


    void AddFunction(int num1, int num2)
    {
        int result = num1 + num2;
        Debug.Log("The sum is " + result.ToString() + ".");
    }

    void MultiplyFunction(int num1, int num2)
    {
        int result = num1 * num2;
        Debug.Log("The product is " + result.ToString() + ".");
    }

    void Start()
    {
        //MathDelagate fChanined;
        //fChanined = AddFunction;
        //fChanined += MultiplyFunction;
        //fChanined(3, 5);

        //fChanined -= MultiplyFunction;

        //fChanined(3, 5);
        ////f = AddFunction;
        ////f(3, 5);
        ////f = MultiplyFunction;
        ////f(3,5);
        Diagnostic("", "");
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                m_touchMovement = Vector2.zero;
                Diagnostic("", "");

            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                m_touchMovement += touch.deltaPosition;

                if (m_touchMovement.magnitude > m_minSwipeDistance)
                {
                    OnSwipe();
                    Diagnostic("Swipe detected", m_touchMovement.ToString() + " " + SwipeDiagnostic(m_touchMovement));
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnSwipeEnd();
            }
        }
    }

    public Text m_diagnosticText1;
    public Text m_diagnosticText2;

    public bool m_useDiagnostic = false;

    void Diagnostic(string text1,string text2)
    {
        m_diagnosticText1.gameObject.SetActive(m_useDiagnostic);
        m_diagnosticText2.gameObject.SetActive(m_useDiagnostic);

        if (m_diagnosticText1 && m_diagnosticText2)
        {
            m_diagnosticText1.text = text1;
            m_diagnosticText2.text = text2;


        }
    }

    string SwipeDiagnostic(Vector2 swipeMovement)
    {
        string direction = "";

        if (Mathf.Abs(swipeMovement.x)> Mathf.Abs(swipeMovement.y)) 
        {
            direction = (swipeMovement.x >= 0) ? "right" : "left";
        }
        else
        {
            direction = (swipeMovement.y >= 0) ? "up" : "down";
        }
        return direction;
    }
    
   
}
