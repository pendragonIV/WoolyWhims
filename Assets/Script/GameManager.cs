using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;

    #region Game status
    [SerializeField]
    private bool isGameWin = false;
    [SerializeField]
    private bool isLose = false;

    [SerializeField]
    public int achivement = 0;
    private const int MAX_ACHIVE = 3;

    private Level currentLevelData;
    private float timeLeft;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        GameObject map =  Instantiate(currentLevelData.map);
        GridCellManager.instance.SetTileMap(map.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>());
        GridCellManager.instance.GetMoveableCells();
        timeLeft = currentLevelData.timeLimit;
        Time.timeScale = 1;
    }

    public void Win()
    {
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false, 0);
            }
        }
        SetAchivement();
        if (achivement > LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).achivement)
        {
            LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex, true, true, achivement);
        }

        isGameWin = true;

        gameScene.ShowWinPanel();
        Time.timeScale = 0;
        LevelManager.instance.levelData.SaveDataJSON();
    }

    private void Update()
    {
        if (isGameWin || isLose)
        {
            return;
        }
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Lose();
        }
    }

    private void FixedUpdate()
    {
        gameScene.UpdateTime(timeLeft, currentLevelData.timeLimit);
    }

    private void SetAchivement()
    {
        float totalTime = currentLevelData.timeLimit;
        achivement = (int)((timeLeft / totalTime) * (MAX_ACHIVE + 1));
    }

    public void Lose()
    {
        isLose = true;
        gameScene.ShowLosePanel();
        Time.timeScale = 0;
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool isGameLose()
    {
        return isLose;
    }
}

