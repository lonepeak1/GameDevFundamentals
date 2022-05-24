using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController controller;

    public delegate void IncreaseScoreBy(int x);
    public delegate void DecreaseScoreBy(int x);
    public delegate void UpdateScore(int x);
    public event IncreaseScoreBy onIncreaseScore;
    public event DecreaseScoreBy onDecreaseScore;
    public event UpdateScore onUpdateScore;
    int score = 0;

    public delegate void IncreaseHealthBy(int x);
    public delegate void DecreaseHealthBy(int x);
    public delegate void UpdateHealth(int x);
    public event IncreaseHealthBy onIncreaseHealth;
    public event DecreaseHealthBy onDecreaseHealth;
    public event UpdateHealth onUpdateHealth;
    int health = 100;

    public delegate void PlayerDied();
    public static event PlayerDied onPlayerDied;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    public void DecreaseScore(int x)
    {
        score += x;
        if(onIncreaseScore != null)
            onIncreaseScore(x);

        if(onUpdateScore != null)
            onUpdateScore(score);
    }

    public void IncreaseScore(int x)
    {
        score -= x;
        if (onDecreaseScore != null)
            onDecreaseScore(x);

        if (onUpdateScore != null)
            onUpdateScore(score);
    }

    public void DecreaseHealth(int x)
    {
        health -= x;
        if (onIncreaseHealth != null)
            onIncreaseHealth(x);

        if (onUpdateHealth != null)
            onUpdateHealth(health);
    }

    public void IncreaseHealth(int x)
    {
        health += x;
        if (onDecreaseHealth != null)
            onDecreaseHealth(x);

        if (onUpdateHealth != null)
            onUpdateHealth(health);
    }


    // Start is called before the first frame update
    void Start()
    {
        if(controller== null)
            controller = this;
    }

    public void SignalPlayerDead()
    {
        if (onPlayerDied != null)
            onPlayerDied();
    }
}

