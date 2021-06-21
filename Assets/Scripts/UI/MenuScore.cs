using UnityEngine;
using UnityEngine.UI;

public class MenuScore : MonoBehaviour
{
    private Text txt;

    private int currentScore;
    private int savedBest;
    private int newBest;

    void Start()
    {
        txt = GetComponent<Text>();
        currentScore = PlayerPrefs.GetInt("current");
        savedBest = PlayerPrefs.GetInt("bestSaved");
        newBest = PlayerPrefs.GetInt("newBest");

        if (newBest > savedBest) {
            PlayerPrefs.SetInt("bestSaved", newBest);

            txt.text = "New best score\n" + newBest;
        } else
        {
            txt.text = "Score\n" + currentScore + "\nBest\n" + savedBest;
        }
    }

}
