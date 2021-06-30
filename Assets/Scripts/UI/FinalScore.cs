using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    [SerializeField] TMP_Text txt;

    int currentScore;
    int savedBest;


    public void UpdateScore()
    {
        currentScore = PlayerPrefs.GetInt("current");
        savedBest = PlayerPrefs.GetInt("saved");

        if (currentScore > savedBest) {
            txt.text = "New best score\n" + currentScore;
            PlayerPrefs.SetInt("saved", currentScore);
        } else
        {
            txt.text = "Score\n" + currentScore + "\nBest\n" + savedBest;
        }
    }

}
