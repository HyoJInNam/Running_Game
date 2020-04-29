using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager: MonoBehaviour
{
    public int Score;
    private int timeScore;
    public Text curScore;
    public Text ResultScore;

    public bool IsDrawScoreCanvas;
    private GameObject score_canvas;

    public GameObject[] star = new GameObject[3];
    public GameObject[] starEffect = new GameObject[3];
    public void Setting()
    {
        score_canvas = gameObject.transform.Find("score").gameObject;
        Transform cv = score_canvas.transform.Find("canvas_event").transform.Find("score").transform.Find("stars");
        Transform cvEff = score_canvas.transform.Find("canvas_effects").transform.Find("score");
        for (int i = 0; i < 3; i++)
        {
            star[i] = cv.GetChild(i).gameObject;
            starEffect[i] = cvEff.GetChild(i).gameObject;
        }
    }

    public void Initialize()
    {
        IsDrawScoreCanvas = false;
        timeScore = 0;
    }

    public void SetCurentScoreText(PlayerControler get)
    {
        int level = get.player.level + 1;
        Score = (level * timeScore) / 60 * 10
                + (level * 770 * get.starCount)
                + ((get.isGoal) ? 1000 : 0);
        curScore.text = Score.ToString();
        timeScore++;
    }
    public void SetResultScoreText(PlayerControler get)
    {
        if (IsDrawScoreCanvas) return;
        DrawScoreCanvas(true);
        for (int i = 0; i < get.starCount; i++)
        {
            star[i].SetActive(true);
            starEffect[i].SetActive(true);
        }

        //int level = get.player.level + 1;
        //Score = (level * timeScore) / 60 * 10
        //        + (level * 770 * get.starCount)
        //        + (level * 500 * (3 - get.damagedCount))
        //        + ((get.isGoal) ? 1000 : 0);
        ResultScore.text = Score.ToString();
    }
    public void DrawScoreCanvas(bool isDraw)
    {
        IsDrawScoreCanvas = isDraw;
        if(IsDrawScoreCanvas) score_canvas.SetActive(IsDrawScoreCanvas);
    }
}
