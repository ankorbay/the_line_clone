using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    const string DEFAULT_SCORE_VALUE = "Score";

    [SerializeField] [Range(1,10)] float speed = 2.5f;
    [SerializeField] TMP_Text txt;

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
            intScore = (int)score;
            txt.text = "Score \n" + intScore;
            score += amountPerSecond * Time.deltaTime * speed;
        }
    }


    public void RunScore()
    {
        isGameRunning = true;
    }

    public void StopScore()
    {
        bestSaved = PlayerPrefs.GetInt("bestSaved");
        isGameRunning = false;
        if(score > bestSaved)
        {
            PlayerPrefs.SetInt("newBest", intScore);
            PlayerPrefs.SetInt("current", intScore);
        } else
        {
            PlayerPrefs.SetInt("current", intScore);
        }
    }
}
