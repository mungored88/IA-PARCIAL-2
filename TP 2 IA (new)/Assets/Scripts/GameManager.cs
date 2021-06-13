using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IObserver, IObservable
{
    public PlayerModel player;
    public GameObject endScreen;
    public TextMeshProUGUI timer;
    public float time;
    public GameObject winText;
    public GameObject loseText;
    public GameObject reachEndText;
    public TextMeshProUGUI fruitsActual;
    public TextMeshProUGUI fruitsEndNum;
    public TextMeshProUGUI timeNum;
    public GameObject nextLevel;
    public GameObject restartLevel;
    private CamTest _cam;
    bool _levelFinished;
    public delegate void Execute();
    public Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();
    private List<IObserver> _observers = new List<IObserver>();
    public int fruits;
    public int maxFruits;
    // Start is called before the first frame update
    void Start()
    {
        _cam = FindObjectOfType<CamTest>();
        player = FindObjectOfType<PlayerModel>();
        timer.text = "" + (int)time;
        endScreen.SetActive(false);
        winText.SetActive(false);
        loseText.SetActive(false);
        player.Subscribe(this);
        _actionsDic.Add("Lose", Lose);
        _actionsDic.Add("Fruit", AddFruit);
        _actionsDic.Add("PlayerIn", PlayerInEnd);
    }

    // Update is called once per frame
    void Update()
    {
        if (_levelFinished == false)
        {
            time -= Time.deltaTime;
            timer.text = "" + (int)time;
        }

        if (time <= 0)
        {
            Lose();
        }
            
        fruitsActual.text = "" + fruits + " / " + maxFruits;
    } 

    public void Win()
    {
        _levelFinished = true;
        NotifyObserver("End");
        endScreen.SetActive(true);
        _cam.canMove = false;
        winText.SetActive(true);
        if (nextLevel != null)
            nextLevel.SetActive(true);
        fruitsEndNum.text = "" + fruits;
        timeNum.text = "" + (int)time;
    }

    public void Lose()
    {
        _levelFinished = true;
        NotifyObserver("End");
        restartLevel.SetActive(true);
        endScreen.SetActive(true);
        _cam.canMove = false;
        loseText.SetActive(true);
        fruitsEndNum.text = "" + fruits;
        timeNum.text = "" + (int)time;
    }

    void AddFruit()
    {
        fruits++;
        if (fruits >= maxFruits)
            reachEndText.SetActive(true);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnAction(string action)
    {
        if (_actionsDic.ContainsKey(action))
            _actionsDic[action]();
    }

    public void Subscribe(IObserver observer)
    {
        if (_observers.Contains(observer) == false)
            _observers.Add(observer);
    }

    public void Unsuscribe(IObserver observer)
    {
    }

    public void NotifyObserver(string action)
    {
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            _observers[i].OnAction(action);
        }
    }

    public void SecondLevel()
    {
        SceneManager.LoadScene("LVL 3");
    }

    public void ThirdLevel()
    {
        SceneManager.LoadScene("LVL 2 (3) 1");
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void PlayerInEnd()
    {
        Debug.Log("InEnd");
        if (fruits >= maxFruits)
            Win();
    }
}
