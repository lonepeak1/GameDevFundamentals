using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowScore : MonoBehaviour
{
    public bool being_destroyed = false;
    public string ScoreTitle = "Points";
    public Sprite num1;
    public Sprite num2;
    public Sprite num3;
    public Sprite num4;
    public Sprite num5;
    public Sprite num6;
    public Sprite num7;
    public Sprite num8;
    public Sprite num9;
    public Sprite num0;

    public SpriteRenderer onesScoreNumberRenderer;
    public SpriteRenderer tensScoreNumberRenderer;


    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameController.controller.onUpdateScore += UpdateScore;
    }

    // Update is called once per frame
    void UpdateScore(int score)
    {
        int tensNum = (int)(score / 10);
        int onesNum = score % 10;

        if (onesScoreNumberRenderer != null)
        {
            if (onesNum == 0)
                onesScoreNumberRenderer.sprite = num0;
            if (onesNum == 1)
                onesScoreNumberRenderer.sprite = num1;
            if (onesNum == 2)
                onesScoreNumberRenderer.sprite = num2;
            if (onesNum == 3)
                onesScoreNumberRenderer.sprite = num3;
            if (onesNum == 4)
                onesScoreNumberRenderer.sprite = num4;
            if (onesNum == 5)
                onesScoreNumberRenderer.sprite = num5;
            if (onesNum == 6)
                onesScoreNumberRenderer.sprite = num6;
            if (onesNum == 7)
                onesScoreNumberRenderer.sprite = num7;
            if (onesNum == 8)
                onesScoreNumberRenderer.sprite = num8;
            if (onesNum == 9)
                onesScoreNumberRenderer.sprite = num9;
        }


        if (tensScoreNumberRenderer != null)
        {
            //Tens renderer
            if (tensNum == 0)
                tensScoreNumberRenderer.sprite = num0;
            if (tensNum == 1)
                tensScoreNumberRenderer.sprite = num1;
            if (tensNum == 2)
                tensScoreNumberRenderer.sprite = num2;
            if (tensNum == 3)
                tensScoreNumberRenderer.sprite = num3;
            if (tensNum == 4)
                tensScoreNumberRenderer.sprite = num4;
            if (tensNum == 5)
                tensScoreNumberRenderer.sprite = num5;
            if (tensNum == 6)
                tensScoreNumberRenderer.sprite = num6;
            if (tensNum == 7)
                tensScoreNumberRenderer.sprite = num7;
            if (tensNum == 8)
                tensScoreNumberRenderer.sprite = num8;
            if (tensNum == 9)
                tensScoreNumberRenderer.sprite = num9;
        }



    }
    
}