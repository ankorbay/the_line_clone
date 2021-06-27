using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Spawner spawner;
    Player playerController;
    PlayerPositionController playerPositionController;
    Score score;
    InfoPlaneText infoPlaneText;

    [Inject]
    public void Init(Spawner spawner, Player playerController, Score score, InfoPlaneText infoPlaneText, PlayerPositionController playerPositionController)
    {
        this.spawner = spawner;
        this.playerController = playerController;
        this.score = score;
        this.infoPlaneText = infoPlaneText;
        this.playerPositionController = playerPositionController;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartTheGame();
        }

        if (!playerController.IsAlive())
        {
            score.StopScore();
            playerPositionController.TurnOffControls();

            if (!playerController.IsAnimationPlaying())
            {
                StopTheGame();
            }
        }

        if (playerController.IsDestroyBlocksMode)
        {
            infoPlaneText.MakeVisible();
            int timeLeft = playerController.GetModeActiveTimeLeft();
            infoPlaneText.SetText("Destroy block mode time left " + timeLeft);
        }
        else if (playerController.IsSizeReducerMode)
        {
            infoPlaneText.MakeVisible();
            int timeLeft = playerController.GetModeActiveTimeLeft();
            infoPlaneText.SetText("Size reducer mode time left " + timeLeft);
        }
        else
        {
            infoPlaneText.Hide();
        }
    }


    void StartTheGame()
    {
        score.RunScore();
        playerPositionController.TurnOnControls();
        spawner.MoveBlocks();
    }

    void StopTheGame()
    {
        SceneManager.LoadScene(1);
    }
}
