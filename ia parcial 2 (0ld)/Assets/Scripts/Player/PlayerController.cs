using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPotionGrabber
{

    [SerializeField] private float m_moveSpeed = 6;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;

    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    [SerializeField] private ControlMode controlMode;

    //public SaveLoad save_load;

    

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    private bool m_jumpInput = false;

    private bool m_isGrounded;

    private List<Collider> m_collisions = new List<Collider>();

    public CheckPoint checkpoint;
    public int vidas = 5;
    public GameObject defender;
    public bool isDefender;

    public bool esMobile = false;
    public JoystickController joystick;
    public enum ControlMode
    {
        ANDROID,
        PC
    }

    public ControlMode GetKeyType()
    {
        return controlMode;
    }



    private void Awake()
    {
        if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }

        joystick = gameObject.GetComponent<JoystickController>();
        esMobile = (controlMode == ControlMode.ANDROID);

    }
    private void Start()
    {
        checkpoint = this.GetComponent<CheckPoint>();
        this.guardarCheckPoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    internal void recibirDaño()
    {
        if(isDefender)
        {
            isDefender = false;
            defender.gameObject.SetActive(false);
            return;
        }
        loadCheckPoint();
        this.vidas -= 1;
        this.GetComponent<ContadorDeVidas>().Life = this.vidas;
        //this.GetComponent<DamageTester>().daño();

        //ActualizarCorazones... en caso de tener 0 Proceder a video o Perder
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }


    private void Update()
    {
        if (!m_jumpInput && Input.GetKey(KeyCode.Space))
        {
            m_jumpInput = true;
        }
        if ( esMobile && joystick.jumpPressed)
       {
            m_jumpInput = true;
        }

    }


    public void guardarCheckPoint()
    {
        checkpoint.setCheckPoint(this.transform);
    }

    public void loadCheckPoint()
    {
        CheckPointOBJ check = checkpoint.getLastCheckPoint();
        this.transform.position = check.lastCheckPos;
        this.transform.rotation = check.lastCheckRot;
    }

    private void FixedUpdate()
    {
        m_animator.SetBool("Grounded", m_isGrounded);
        
       correrUpdate();

        m_wasGrounded = m_isGrounded;
        m_jumpInput = false;
    }

    private void correrUpdate()
    {
        float v = 0;
        float h = 0;
        if (!esMobile) {
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");
        }
        if (esMobile)
        {
             v = joystick.getYValue();
             h = joystick.getXValue();
        }
        bool walk = Input.GetKey(KeyCode.LeftShift);

        if (v < 0)
        {
            if (walk) { v *= m_backwardsWalkScale; }
            else { v *= m_backwardRunScale; }
        }
        else if (walk)
        {
            v *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

       

        m_animator.SetFloat("MoveSpeed", m_currentV);

        JumpingAndLanding();
    }

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && m_jumpInput)
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }

    
    public void GetHp(int hp)
    {
        this.vidas += hp;
        if (vidas > 5) vidas = 5;       
        this.GetComponent<ContadorDeVidas>().Life = this.vidas;

    }
    
    public void GetShield(int shield)
    {
        isDefender = true;
        defender.gameObject.SetActive(true);
    }

    public void GetPwr(int pwr)
    {
        return;
    }
}
