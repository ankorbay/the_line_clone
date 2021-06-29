using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour
{
    const int NUMBER_OF_COLS_PER_ROW = 7;
    const int LAST_STARTING_EMPTY_BLOCK_INDEX = 4;
    const float BONUS_SPAWN_CHANCE = 0.3f;

    [SerializeField][Range(0, 10)] float speed = 8.5f;

    bool isGameRunning = false;
    bool isBonusSpawned = false;

    Vector2 blocksMovementStep;

    PooledBlock lastBlockSpawned;
    Queue<int> lastEmptyBlockIndices;


    void Awake()
    {
        blocksMovementStep = ScreenData.CalculateBlockStep(NUMBER_OF_COLS_PER_ROW);
        lastEmptyBlockIndices = new Queue<int>();
        lastEmptyBlockIndices.Enqueue(LAST_STARTING_EMPTY_BLOCK_INDEX);
    }

    void Start()
    {
        BlockPool.Instance.AddBlocks(100);
        SizeReducerPool.Instance.AddSizeReducers(1);
        DestroyerPool.Instance.AddDestroyers(1);

        SpawnStartingSet();
    }

    void Update()
    {
        if (BlockSpawningCondition())
        {
            SpawnRow(9, false);
        }
    }


    #region Transformation

    public void MoveBlocks()
    {
        isGameRunning = true;
        DOTween.PlayAll();
    }

    float CalculateColXPos(int colNum)
    {
        return (colNum - 4) * blocksMovementStep.x;
    }

    float CalculateRowYPos(int rowNum)
    {
        return (rowNum - 4) * blocksMovementStep.y;
    }

    void SetUpObjectMovement(GameObject gameObject, bool pauseOnStart)
    {
        gameObject.transform.DOLocalMoveY(gameObject.transform.position.y - blocksMovementStep.y * 10f, speed).SetEase(Ease.Linear);
        if (pauseOnStart) gameObject.transform.DOPause();
    }

    bool StartPositioningCondition(int row, int col)
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


    #region Spawning
    void SpawnStartingSet()
    {
        for (int i = 1; i < 10; i++)
        {
            SpawnStartRow(i, true);
        }
    }

    void SpawnDestroyer(Vector2 pos)
    {
        isBonusSpawned = true;

        PooledDestroyer destroyer = DestroyerPool.Instance.Get();
        destroyer.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        destroyer.gameObject.SetActive(true);
        destroyer.DestoyOnTimeOut(12f);
        SetUpObjectMovement(destroyer.gameObject, false);
        SetNextSpawnMinTimeout(12f);
    }

    void SetNextSpawnMinTimeout(float timeoutSec)
    {
        Sequence myTimeoutSequence = DOTween.Sequence();
        myTimeoutSequence.AppendInterval(timeoutSec);
        myTimeoutSequence.AppendCallback(EnableBonusSpawning);
        void EnableBonusSpawning()
        {
            isBonusSpawned = false;
        }
    }

    void SpawnSizeReducer(Vector2 pos)
    {
        isBonusSpawned = true;

        PooledSizeReducer sizeReducer = SizeReducerPool.Instance.Get();
        sizeReducer.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        sizeReducer.gameObject.SetActive(true);
        sizeReducer.DestoyOnTimeOut(12f);
        SetUpObjectMovement(sizeReducer.gameObject, false);
        SetNextSpawnMinTimeout(12f);
    }

    void SpawnBlock(Vector2 pos, bool pauseOnStart)
    {
        PooledBlock block = BlockPool.Instance.Get();
        block.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        block.gameObject.SetActive(true);

        lastBlockSpawned = block;

        SetUpObjectMovement(block.gameObject, pauseOnStart);
    }

    bool BlockSpawningCondition()
    {
        float lastBlockYpos = lastBlockSpawned.gameObject.transform.position.y;
        bool isWentDownOneBlock = lastBlockYpos <= blocksMovementStep.y * 4.04f;
        bool isLastBlockHigherThanViewport = lastBlockYpos > blocksMovementStep.y * 3.1f;
        return isWentDownOneBlock && isLastBlockHigherThanViewport && isGameRunning;
    }

    void SpawnRow(int rowNum, bool pauseOnStart)
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

    void SpawnBonus(float posY, float posX)
    {
        if (!isBonusSpawned)
        {
            if (PassFail(BONUS_SPAWN_CHANCE))
            {
                SpawnDestroyer(new Vector2(posX, posY));
            } else if (PassFail(BONUS_SPAWN_CHANCE))
            {
                SpawnSizeReducer(new Vector2(posX, posY));
            }
        }
    }

    void SpawnStartRow(int rowNum, bool pauseOnStart)
    {
        float posY = CalculateRowYPos(rowNum);
        for (int colNum = 1; colNum < 8; colNum++)
        {
            float posX = CalculateColXPos(colNum);
            if (StartPositioningCondition(rowNum, colNum)) SpawnBlock(new Vector2(posX, posY), pauseOnStart);
        }
    }

    Queue<int> GenerateEmptyBlockColIndices(Queue<int> lastEmptyBlockIndices)
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
    #endregion


    #region Utilities
    bool PassFail(float chanceOfSuccess)
    {
        return Random.value < chanceOfSuccess;
    }

    int RandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    #endregion
}
