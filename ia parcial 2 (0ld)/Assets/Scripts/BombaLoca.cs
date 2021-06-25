using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BombaLoca : Enemy
{
    private Animator anim;
    public AudioSource _audio;

    [SerializeField] private float distanciaParaCorrer = 5;
    [SerializeField] private float distanciaParaExplotar = 1;
    [SerializeField] private float runSpeed = 3;

    private bool _exploded = false;

    //State Machine
    public FSM<string> _fsmBomba;


      void Start()
      {
        anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        IdleState<string> bombaIdleState = new IdleState<string>(this );
        WalkState<string> bombaWalkState = new WalkState<string>(this, anim);
        AtackState<string> bombaAtackState = new AtackState<string>(this, anim, _audio);

        bombaIdleState.AddTransition("Walk", bombaWalkState);
        bombaWalkState.AddTransition("Idle", bombaIdleState);
        bombaWalkState.AddTransition("Atack", bombaAtackState);
        bombaIdleState.AddTransition("Atack", bombaAtackState);

        _fsmBomba = new FSM<string>(bombaIdleState);
        _fsmBomba.SetState(bombaIdleState);

      }
     void Update()
     {
        _fsmBomba.OnUpdate();
         CheckDistance();
     }


    public void CheckDistance()
    {
        Vector3 lookAtPos = Player.transform.position;

        transform.LookAt(lookAtPos);

        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if (distanceToPlayer <= distanciaParaCorrer && !_exploded)
        {
            _fsmBomba.Transition("Walk");        
        }
        else
        {         
            _fsmBomba.Transition("Idle");       
        }

        if (distanceToPlayer <= distanciaParaExplotar && !_exploded)
        {    
            _fsmBomba.Transition("Atack");
            StartCoroutine(hacerDañoEnSegundos(1.5f));
        }
    }
    public void Walk()
    {
        anim.SetBool("walk", true);
        transform.position += transform.forward * runSpeed * Time.deltaTime;
    }
    public void Idle()
    {
        anim.SetBool("walk", false);
    }
    public void Atack()
    {       
        anim.SetBool("walk", false);
        anim.SetTrigger("attack01");
        _exploded = true;
        _audio.PlayDelayed(0.5f);
        Destroy(anim.gameObject, 1.5f);
    }

}

   
