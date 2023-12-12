using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScene : MonoBehaviour
{
    public static LevelScene instance;

    [SerializeField]
    private Transform levelHolderPrefab;
    [SerializeField]
    private Transform levelsContainer;
    [SerializeField]
    private Transform starPrefab;

    public Transform sceneTransition;

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

    void Start()
    {
        PrepareLevels();
    }
    public void PlayChangeScene()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
    }

    private void PrepareLevels()
    {
        bool isBot = true;
        for (int i = 0; i < LevelManager.instance.levelData.GetLevels().Count; i++)
        {
            Transform holder = Instantiate(levelHolderPrefab, levelsContainer);
            holder.name = i.ToString();
            if (isBot)
            {
                holder.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
            }

            Level level = LevelManager.instance.levelData.GetLevelAt(i);
            if (LevelManager.instance.levelData.GetLevelAt(i).isPlayable)
            {
                holder.GetComponent<LevelHolder>().EnableHolder();
            }
            else
            {
                holder.GetComponent<LevelHolder>().DisableHolder();
            }

            SetAchivement(holder, level);
            isBot = !isBot;
        }
    }

    private void SetAchivement(Transform holder, Level levelData)
    {
        Transform achivementContainer = holder.GetChild(0).GetChild(0);

        if (!levelData.isPlayable)
        {
            achivementContainer.gameObject.SetActive(false);
        }

        for(int i = 0; i < levelData.achivement; i++)
        {
            Transform disableFilter = achivementContainer.GetChild(i).GetChild(0);
            disableFilter.gameObject.SetActive(false);
        }
    }



}
