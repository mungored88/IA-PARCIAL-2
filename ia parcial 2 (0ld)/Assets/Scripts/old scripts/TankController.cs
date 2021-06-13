using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour, IPotionGrabber
{


    [SerializeField] private float m_moveSpeed = 6;
    [SerializeField] private float m_turnSpeed = 200;

    [SerializeField] private Rigidbody m_rigidBody = null;

    [SerializeField] private ControlMode controlMode;
    // Start is called before the first frame update


    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private Vector3 m_currentDirection = Vector3.zero;

    private bool m_jumpInput = false;

    private List<Collider> m_collisions = new List<Collider>();

    public CheckPoint checkpoint;
    public int vidas = 5;

    public bool esMobile = true;
    public JoystickController joystick;


    public AudioSource getDaniooSound;
    public int danioDeTanke = 1;




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
        if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }

        joystick = gameObject.GetComponent<JoystickController>();
        esMobile = (controlMode == ControlMode.ANDROID);
    }
    private void Start()
    {
        checkpoint = this.GetComponent<CheckPoint>();

        Sounds sound = this.GetComponent<Sounds>();
        this.GetComponent<Sounds>().SoundPlay(sound.clips[0]);


    }

    internal void recibirDaño()
    {
        //loadCheckPoint();
        this.vidas -= 1;
        getDaniooSound.PlayOneShot(getDaniooSound.clip);
        this.GetComponent<ContadorDeVidas>().Life = this.vidas;
    }

    private void Update()
    {
        if (!m_jumpInput && Input.GetKey(KeyCode.Space))
        {
            m_jumpInput = true;
        }
        if (esMobile && joystick.jumpPressed)
        {
            m_jumpInput = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) || joystick.jumpPressed)
        {
            this.GetComponent<ShootWithTank>().Disparar();
        }

    }


    private void FixedUpdate()
    {

        correrUpdate();

       // m_wasGrounded = m_isGrounded;
        m_jumpInput = false;
    }


    private void correrUpdate()
    {
        float v = 0;
        float h = 0;
        if (!esMobile)
        {
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

    public void GetHp(int hp)
    {
        this.vidas += hp;
        if (vidas > 5) vidas = 5;
        this.GetComponent<ContadorDeVidas>().Life = this.vidas;

    }

    public void GetShield(int shield)
    {
        return;
    }

    public void GetPwr(int pwr)
    {
        danioDeTanke += pwr;
    }
}
