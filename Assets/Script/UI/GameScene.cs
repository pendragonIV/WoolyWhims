using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Transform pausePanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button levelButton;
    [SerializeField]
    private Slider time;

    public void ShowPausePanel()
    {
        overlayPanel.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), pausePanel.GetComponent<RectTransform>());
        pauseButton.interactable = false;
        replayButton.interactable = false;
        levelButton.interactable = false;
        Time.timeScale = 0;
    }

    public void HidePausePanel()
    {
        StartCoroutine(FadeOut(overlayPanel.GetComponent<CanvasGroup>(), pausePanel.GetComponent<RectTransform>()));
        pauseButton.interactable = true;
        replayButton.interactable = true;
        levelButton.interactable = true;
        Time.timeScale = 1;
    }

    public void ShowWinPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        pauseButton.interactable = false;
        replayButton.interactable = false;
        levelButton.interactable = false;

        Transform achivementContainer = winPanel.GetChild(0);
        StartCoroutine(SetAchive(achivementContainer));
    }

    private IEnumerator SetAchive(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            if(i < GameManager.instance.achivement)
            {
                container.GetChild(i).GetChild(0).DOScale(0, .3f).SetUpdate(true);
                yield return new WaitForSecondsRealtime(.3f);
            }
        }
    }

    public void ShowLosePanel()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        pauseButton.interactable = false;
        replayButton.interactable = false;
        levelButton.interactable = false;
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 500), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);

        pausePanel.gameObject.SetActive(false);
        overlayPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void UpdateTime(float timeLeft, float timeLimit)
    {
        time.value = timeLeft/ timeLimit;
    }
}
