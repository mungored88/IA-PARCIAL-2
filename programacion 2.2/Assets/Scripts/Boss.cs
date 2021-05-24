using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public Transform[] positions;
    public int positionActual = 0;
    public float timeToMove = 15f;
    public float speed = 5;
    public int vidas = 10;
    public Rigidbody rb;


    public Animator anim;

    public NavMeshAgent myAgent;

    public AudioSource danioSound;

    public HealthBar healthBar;
   
 

    // Start is called before the first frame update
    void Start()
    {
        myAgent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();

        healthBar.SetMaxHealth(vidas);
    }

    private void Update()
    {
        timeToMove -= Time.deltaTime;
        if(timeToMove <= 0)
        {
            moveToNextPosition();
            timeToMove = 15f;
        }
    }





    public void moveToNextPosition()
    {
            Vector3 posToGo = positions[Random.Range(0, positions.Length)].position;
            myAgent.SetDestination(posToGo);

    }

    internal void recibirDaño(int danioARecibir)
    {
        if (vidas <= 0) return;
        anim.SetTrigger("getdamage");
        danioSound.Play(0);
        this.vidas -= danioARecibir;
        healthBar.SetHealth(vidas);
        if(vidas <= 0) { anim.SetTrigger("dead"); myAgent.speed = 0; StartCoroutine(cargarVictoryLevel()); return; }
        moveToNextPosition();
    }

    IEnumerator cargarVictoryLevel()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("03 win");
    }
}
