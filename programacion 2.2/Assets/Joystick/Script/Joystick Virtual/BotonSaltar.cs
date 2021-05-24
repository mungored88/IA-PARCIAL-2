using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BotonSaltar : MonoBehaviour,
                    //IDragHandler, //para poder detectar cuando EMPIEZO a hacer drag por encima del objeto
                    //IEndDragHandler, //permite detectar cuando termino de dragear el objeto
                    IPointerUpHandler, //permite detectar cuando suelto el boton del mouse por arriba de este objeto
                    IPointerDownHandler //permite detectar cuando aprieto el boton del mouse encima del objeto
                                         //IPointerEnterHandler, //permite detectar cuando el mouse COMIENZA a estar POR ENCIMA del objeto
                                         //IPointerExitHandler, //permite detectar cuando el mouse DEJA de estar ENCIMA del objeto
                   // IPointerClickHandler //permite detectar cuando hago click sobre el objeto
{
    public bool jumpPressed;
    public RectTransform jumpButton;
    public RectTransform fireButton;

    void Start()
    {
        jumpButton.transform.localPosition = Vector3.zero;
    }

    #region INTERFACES



    //Si solo presiono y no muevo el stick
    public void OnPointerDown(PointerEventData eventData)
    {
        jumpButton.position = eventData.position;
        fireButton.position = eventData.position;
        jumpPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        jumpButton.localPosition = Vector3.zero;
        fireButton.localPosition = Vector3.zero;
        jumpPressed = false;
    }

    #endregion
}
