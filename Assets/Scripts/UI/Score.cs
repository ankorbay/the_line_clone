using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] [Range(1,10)] private float speed = 2.5f;

    private Text txt;
    private bool isGameRunning;
    private float score;
    private int intScore;
    private float amountPerSecond;
    private int bestSaved;

    void Start()
    {
        txt = GetComponent<Text>();
        txt.text = "Score";

        score = 0;
        amountPerSecond = 1;
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
