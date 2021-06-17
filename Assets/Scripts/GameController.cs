﻿using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Spawner _spawner;
    private Player _playerController;
    private Score _score;
    private InfoPlane _infoPlane;

    [Inject]
    public void Construct(Spawner spawner, Player playerController, Score score, InfoPlane infoPlane)
    {
        _spawner = spawner;
        _playerController = playerController;
        _score = score;
        _infoPlane = infoPlane;
    }

    void Start()
    {
        _spawner.SpawnStartingSet();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartTheGame();
        }

        if (!_playerController.IsAlive())
        {
            _score.StopScore();
        }

        if (_playerController.isDestroyBlocksMode)
        {
            _infoPlane.MakeVisible();
            _infoPlane.SetText("Destroy block mode");
        }
        else if (_playerController.isSizeReducerMode)
        {
            _infoPlane.MakeVisible();
            _infoPlane.SetText("Size reducer mode");
        }
        else
        {
            _infoPlane.Hide();
        }
        
        if (!_playerController.IsAnimationPlaying())
        {
            StopTheGame();
        }
    }

    private void StartTheGame()
    {
        _score.RunScore();
        _playerController.TurnOnControls();
        _spawner.MoveBlocks();
    }

    private void StopTheGame()
    {
        SceneManager.LoadScene(1);
    }
}
