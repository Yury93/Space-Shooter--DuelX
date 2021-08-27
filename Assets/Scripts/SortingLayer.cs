using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rend.sortingOrder = 1;
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        rend.sortingOrder = -1;
    }


}
