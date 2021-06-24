using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Spawner spawner;
    Player playerController;
    PlayerPositionController playerPositionController;
    Score score;
    InfoPlane infoPlane;

    [Inject]
    public void Init(Spawner spawner, Player playerController, Score score, InfoPlane infoPlane, PlayerPositionController playerPositionController)
    {
        this.spawner = spawner;
        this.playerController = playerController;
        this.score = score;
        this.infoPlane = infoPlane;
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
            infoPlane.MakeVisible();
            int timeLeft = playerController.GetModeActiveTimeLeft();
            infoPlane.SetText("Destroy block mode time left " + timeLeft);
        }
        else if (playerController.IsSizeReducerMode)
        {
            infoPlane.MakeVisible();
            int timeLeft = playerController.GetModeActiveTimeLeft();
            infoPlane.SetText("Size reducer mode time left " + timeLeft);
        }
        else
        {
            infoPlane.Hide();
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
