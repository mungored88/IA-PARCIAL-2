using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joy : MonoBehaviour, 
                    IDragHandler, //para poder detectar cuando EMPIEZO a hacer drag por encima del objeto
                    IEndDragHandler, //permite detectar cuando termino de dragear el objeto
                    IPointerUpHandler, //permite detectar cuando suelto el boton del mouse por arriba de este objeto
                    IPointerDownHandler //permite detectar cuando aprieto el boton del mouse encima del objeto
                    //IPointerEnterHandler, //permite detectar cuando el mouse COMIENZA a estar POR ENCIMA del objeto
                    //IPointerExitHandler, //permite detectar cuando el mouse DEJA de estar ENCIMA del objeto
                    //IPointerClickHandler //permite detectar cuando hago click sobre el objeto
{
    public float maxDistance;
    public Vector3 stickValue;//Valor que voy a usar para moverme

    public RectTransform stick;

    void Start()
    {
        stick.transform.localPosition = Vector3.zero;
    }

    #region INTERFACES
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Empieza el DRAG");
        stick.position = eventData.position;

        stick.localPosition = Vector3.ClampMagnitude(stick.localPosition, maxDistance);
		stickValue= stick.localPosition / maxDistance;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Se termino el DRAG");
        stick.localPosition = Vector3.zero;
        stickValue = Vector3.zero;
    }

    //Si solo presiono y no muevo el stick
    public void OnPointerDown(PointerEventData eventData)
    {
        stick.position = eventData.position;

        stick.localPosition = Vector3.ClampMagnitude(stick.localPosition, maxDistance);
        stickValue = stick.localPosition / maxDistance;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        stick.localPosition = Vector3.zero;
        stickValue = Vector3.zero;
    }


    #endregion
}
