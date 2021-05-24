using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public Transform player;
    public float distanceToPunch;
    public float timeToShootInicial = 5f;
    public float timeToShoot = 7f;
    public int DamageAttack = 1;
    public float rotationSpeed = 100f;

    public GameObject fireball;
    public Transform fireballOrigin;
    public Animator anim;

    public bool puedeAtacarAPlayer = false;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();

    }

    void Update()
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotationSpeed * Time.deltaTime);
        rotation.z = 0;
        rotation.x = 0;
        this.transform.rotation = rotation;

        timeToShoot -= Time.deltaTime;

        if(timeToShoot<= 0)
        {
            timeToShoot = timeToShootInicial;
            //Invoca el ataque al finalizar la animacion (DevilBossAttack)
            anim.SetTrigger("attack2");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        puedeAtacarAPlayer = true;
        anim.SetTrigger("attack1");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        puedeAtacarAPlayer = true;
        anim.SetTrigger("attack1");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        puedeAtacarAPlayer = false;
    }
    public void Disparar()
    {
        GameObject bulletInstance = Instantiate(fireball);
        bulletInstance.transform.forward = fireballOrigin.right;
        bulletInstance.transform.position = fireballOrigin.position;        
    }
}
