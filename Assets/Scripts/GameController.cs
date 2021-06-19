using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Spawner spawner;
    private Player playerController;
    private PlayerPositionController playerPositionController;
    private Score score;
    private InfoPlane infoPlane;

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

        if (playerController.isDestroyBlocksMode)
        {
            infoPlane.MakeVisible();
            int timeLeft = playerController.GetModeActiveTimeLeft();
            infoPlane.SetText("Destroy block mode time left " + timeLeft);
        }
        else if (playerController.isSizeReducerMode)
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

    private void StartTheGame()
    {
        score.RunScore();
        playerPositionController.TurnOnControls();
        spawner.MoveBlocks();
    }

    private void StopTheGame()
    {
        SceneManager.LoadScene(1);
    }
}
