using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    public Joy myStick;
    public BotonSaltar myButton;

    public Vector3 stickValue;
    public bool jumpPressed;

    void Update()
    {
        stickValue = myStick.stickValue;
        jumpPressed = myButton.jumpPressed;
    }

    public float getXValue()
    {
        return stickValue.x;
    } 
    public float getYValue()
    {
        return stickValue.y;
    }

    public void SetTankButton()
    {
        Image[] joys = myButton.GetComponentsInChildren<Image>();
        joys[0].color = new Color(255, 255, 255, 255);
        joys[1].color = new Color(0,0,0,0);
        
    }

}
