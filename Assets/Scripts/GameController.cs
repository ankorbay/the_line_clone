using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private Player playerController;
    [SerializeField] private Score score;
    [SerializeField] private InfoPlane infoPlane;

    void Start()
    {
        spawner.SpawnStartingSet();
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
        }

        if (playerController.isDestroyBlocksMode)
        {
            infoPlane.MakeVisible();
            infoPlane.SetText("Destroy block mode");
        }
        else if (playerController.isSizeReducerMode)
        {
            infoPlane.MakeVisible();
            infoPlane.SetText("Size reducer mode");
        }
        else
        {
            infoPlane.Hide();
        }
        
        if (!playerController.IsAnimationPlaying())
        {
            StopTheGame();
        }
    }

    private void StartTheGame()
    {
        score.RunScore();
        playerController.TurnOnControls();
        spawner.MoveBlocks();
    }

    private void StopTheGame()
    {
        SceneManager.LoadScene(1);
    }
}
