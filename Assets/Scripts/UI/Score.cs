using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] [Range(1,10)] float speed = 2.5f;

    Text txt;

    bool isGameRunning;

    float score = 0;
    float amountPerSecond = 1f;
    int intScore;
    int bestSaved;


    void Start()
    {
        txt = GetComponent<Text>();
        txt.text = "Score";
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
