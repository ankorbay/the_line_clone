using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    const string DEFAULT_SCORE_VALUE = "Score";

    [SerializeField] [Range(1,10)] float speed = 2.5f;
    [SerializeField] TMP_Text txt;
    [SerializeField] FinalScore finalScore;

    bool isGameRunning;

    float score = 0;
    float amountPerSecond = 1f;
    int intScore;
    int bestSaved;


    void Start()
    {
        txt.text = DEFAULT_SCORE_VALUE;
    }

    void Update()
    {
        if (isGameRunning)
        {
            score += amountPerSecond * Time.deltaTime * speed;
            intScore = (int)score;
            txt.text = "Score \n" + intScore;
            
        }
    }


    public void RunScore()
    {
        isGameRunning = true;
    }

    public void StopScore()
    {
        if (isGameRunning)
        {
            PlayerPrefs.SetInt("current", intScore);
            finalScore.UpdateScore();
        }
        isGameRunning = false;
    }
}
