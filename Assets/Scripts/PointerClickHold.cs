using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerClickHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler
{
    private bool m_Hold;//Удерживаем ли кнопку
    public bool IsHold => m_Hold;

    private bool m_Click;//Нажали та ли клавиша
    public bool IsClick => m_Click;


    public void OnPointerDown(PointerEventData eventData)
    {
        m_Hold = true;
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_Hold = false;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_Click = true;
        StartCoroutine(OnClick());
    }

    IEnumerator OnClick()
    {
        yield return new WaitForSeconds(0.3f);
        m_Click = false;
    }
}
