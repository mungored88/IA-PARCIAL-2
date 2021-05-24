using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaLoca : Enemy
{
    private Animator anim;
    public AudioSource explosion;

    [SerializeField] private float distanciaParaCorrer = 5;
    [SerializeField] private float distanciaParaExplotar = 1;
    [SerializeField] private float runSpeed = 3;

    private bool _exploto = false;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 lookAtPos = Player.transform.position;
        transform.LookAt(lookAtPos);

        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if (distanceToPlayer <= distanciaParaCorrer && !_exploto)
        {
            correrHaciaPlayer();
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }
        
        if(distanceToPlayer <= distanciaParaExplotar && !_exploto)
        {
            anim.SetBool("walk", false);
            _exploto = true;
            anim.SetTrigger("attack01");
                     
            StartCoroutine(hacerDañoEnSegundos(1.5f));
        }

    }

    void correrHaciaPlayer()
    {
        transform.position += transform.forward * runSpeed * Time.deltaTime;
    }
}
