using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour
{
    [SerializeField][Range(0, 10)] private float speed = 8.5f;

    private bool isGameRunning = false;
    private bool isBonusSpawned = false;
    private Vector2 blocksMovementStep;
    private new Camera camera;
    private PooledBlock lastBlockSpawned;
    public Queue<int> lastEmptyBlockIndices;
    
    private void Awake()
    {
        camera = Camera.main;
        blocksMovementStep = ScreenData.CalculateBlockStep(7);
        lastEmptyBlockIndices = new Queue<int>();
        lastEmptyBlockIndices.Enqueue(4);
    }

    private void Start()
    {
        BlockPool.Instance.AddBlocks(100);
        SizeReducerPool.Instance.AddSizeReducers(1);
        DestroyerPool.Instance.AddDestroyers(1);

        SpawnStartingSet();
    }

    private void Update()
    {
        if (BlockSpawningCondition())
        {
            SpawnRow(9, false);
        }
    }

    private void SpawnStartingSet()
    {
        for (int i = 1; i < 10; i++)
        {
            SpawnStartRow(i, true);
        }
    }

    private void SpawnDestroyer(Vector2 pos)
    {
        isBonusSpawned = true;

        PooledDestroyer destroyer = DestroyerPool.Instance.Get();
        destroyer.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        destroyer.gameObject.SetActive(true);
        destroyer.DestoyOnTimeOut(12f);
        SetUpObjectMovement(destroyer.gameObject, false);
        SetNextSpawnMinTimeout(12f);
    }

    private void SetNextSpawnMinTimeout(float timeoutSec)
    {
        Sequence myTimeoutSequence = DOTween.Sequence();
        myTimeoutSequence.AppendInterval(timeoutSec);
        myTimeoutSequence.AppendCallback(EnableBonusSpawning);
        void EnableBonusSpawning()
        {
            isBonusSpawned = false;
        }
    }

    private void SpawnSizeReducer(Vector2 pos)
    {
        isBonusSpawned = true;

        PooledSizeReducer sizeReducer = SizeReducerPool.Instance.Get();
        sizeReducer.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        sizeReducer.gameObject.SetActive(true);
        sizeReducer.DestoyOnTimeOut(12f);
        SetUpObjectMovement(sizeReducer.gameObject, false);
        SetNextSpawnMinTimeout(12f);
    }

    private void SpawnBlock(Vector2 pos, bool pauseOnStart)
    {
        PooledBlock block = BlockPool.Instance.Get();
        block.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        block.gameObject.SetActive(true);

        lastBlockSpawned = block;

        SetUpObjectMovement(block.gameObject, pauseOnStart);
    }

    private bool BlockSpawningCondition()
    {
        float pos = lastBlockSpawned.gameObject.transform.position.y;
        bool isWentDownOneBlock = pos <= blocksMovementStep.y * 4.02f;
        bool isLastBlockHigherThanViewport = pos > blocksMovementStep.y * 3.1f;
        return isWentDownOneBlock && isLastBlockHigherThanViewport && isGameRunning;
    }

    private void SpawnRow(int rowNum, bool pauseOnStart)
    {
        float posY = CalculateRowYPos(rowNum);

        Queue<int> newRowBlockIndices = GenerateEmptyBlockColIndices(lastEmptyBlockIndices);
        for (int colNum = 1; colNum < 8; colNum++)
        {
            float posX = CalculateColXPos(colNum);

            if (!newRowBlockIndices.Contains(colNum)) {
                SpawnBlock(new Vector2(posX, posY), pauseOnStart);
            } else
            {
                SpawnBonus(posY, posX);
            }
        }

        lastEmptyBlockIndices.Clear();
        int[] valuesArray = newRowBlockIndices.ToArray();

        foreach (var index in valuesArray)
        {
            lastEmptyBlockIndices.Enqueue(index);
        }
    }

    private void SpawnBonus(float posY, float posX)
    {
        if (!isBonusSpawned)
        {
            if (PassFail(0.3f))
            {
                SpawnDestroyer(new Vector2(posX, posY));
            } else if (PassFail(0.3f))
            {
                SpawnSizeReducer(new Vector2(posX, posY));
            }
        }
    }

    private void SpawnStartRow(int rowNum, bool pauseOnStart)
    {
        float posY = CalculateRowYPos(rowNum);
        for (int colNum = 1; colNum < 8; colNum++)
        {
            float posX = CalculateColXPos(colNum);
            if (StartPositioningCondition(rowNum, colNum)) SpawnBlock(new Vector2(posX, posY), pauseOnStart);
        }
    }

    private Queue<int> GenerateEmptyBlockColIndices(Queue<int> lastEmptyBlockIndices)
    {
        Queue<int> newRowIndices = new Queue<int>();

        int previousRowIndicesCount = lastEmptyBlockIndices.Count;
        int previousRowLastIndex = lastEmptyBlockIndices.Peek();

        if (previousRowIndicesCount != 1)
        {
            newRowIndices.Enqueue(previousRowLastIndex);
        }
        else
        {
            int destinationIndex = RandomNumber(2, 6);

            if (destinationIndex > previousRowLastIndex)
            {
                for (int i = destinationIndex; i >= previousRowLastIndex; i--)
                {
                    newRowIndices.Enqueue(i);
                }
            }
            else if (destinationIndex < previousRowLastIndex)
            {
                for (int i = destinationIndex; i <= previousRowLastIndex; i++)
                {
                    newRowIndices.Enqueue(i);
                }
            }
            else
            {
                newRowIndices.Enqueue(previousRowLastIndex);
            }
        }

        return newRowIndices;
    }

    #region Transformation

    public void MoveBlocks()
    {
        isGameRunning = true;
        DOTween.PlayAll();
    }

    private float CalculateColXPos(int colNum)
    {
        return (colNum - 4) * blocksMovementStep.x;
    }

    private float CalculateRowYPos(int rowNum)
    {
        return (rowNum - 4) * blocksMovementStep.y;
    }

    private void SetUpObjectMovement(GameObject gameObject, bool pauseOnStart)
    {
        gameObject.transform.DOLocalMoveY(gameObject.transform.position.y - blocksMovementStep.y * 10f, speed).SetEase(Ease.Linear);
        if (pauseOnStart) gameObject.transform.DOPause();
    }

    private bool StartPositioningCondition(int row, int col)
    {
        bool conditionLine1 = row == 1 && (col == 1 || col == 7);
        bool conditionLine2 = row == 2 && (col == 1 || col == 7);
        bool conditionLine3 = row == 3 && (col == 1 || col == 2 || col == 6 || col == 7);
        bool conditionLine4 = row == 4 && (col == 1 || col == 2 || col == 6 || col == 7);
        bool conditionLine5 = row == 5 && (col == 1 || col == 2 || col == 6 || col == 7);
        bool conditionLine6 = row == 6 && !(col == 4);
        bool conditionLine7 = row == 7 && !(col == 4);
        bool conditionLine8 = row == 8 && !(col == 4);
        bool conditionLine9 = row == 9 && !(col == 4);

        return conditionLine1 || conditionLine2 || conditionLine3 || conditionLine4 || conditionLine5 || conditionLine6 || conditionLine7 || conditionLine8 || conditionLine9;
    }

    #endregion

    #region Utilities
    private bool PassFail(float chanceOfSuccess)
    {
        return Random.value < chanceOfSuccess;
    }

    private int RandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    #endregion
}
