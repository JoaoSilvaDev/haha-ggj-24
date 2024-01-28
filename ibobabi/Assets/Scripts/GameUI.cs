using DG.Tweening;
using System.Drawing;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("TITLE SCREEN UI")]
    public CanvasGroup titleScreenUI;

    [Header("RUN STATE UI")]
    public CanvasGroup runUI;
    public Image mouseHoverFillBar;
    public TextMeshProUGUI liveTickleCounter;
    public RectTransform mouseHoverBarParent;

    [Header("TICKLE STATE UI")]
    public CanvasGroup tickleUI;
    public TextMeshProUGUI tickleCounter;
    public Image tickleTimerFillbar;

    [Header("TICKLE STOP STATE UI")]
    public CanvasGroup tickleStopUI;

    [Header("LOSE STATE UI")]
    public CanvasGroup loseUI;
    public TextMeshProUGUI loseScreenPressAnyKey;

    [Header("RESULTS SCREEN")]
    public TextMeshProUGUI ticklesText;
    public Image resultsBackground;
    public CanvasGroup resultsUI;

    [Header("DEBUG")]
    public CanvasGroup debugUI;
    public TextMeshProUGUI stateDebugText;
    
    private void Awake()
    {
        titleScreenUI.alpha = 0f;
        runUI.alpha = 0f;
        tickleUI.alpha = 0f;
        tickleStopUI.alpha = 0f;
        loseUI.alpha = 0f;
        debugUI.alpha = 0f;
    }

    private void Start()
    {
        GameManager.instance.titleScreenState.OnEnter += OnEnterTitlescreenState;
        GameManager.instance.titleScreenState.OnExit += OnExitTitlescreenState;

        GameManager.instance.runState.OnEnter += OnEnterRunState;
        GameManager.instance.runState.OnExit += OnExitRunState;

        GameManager.instance.tickleState.OnEnter += OnEnterTickleState;
        GameManager.instance.tickleState.OnExit += OnExitTickleState;

        GameManager.instance.tickleStopState.OnEnter += OnEnterTickleStopState;
        GameManager.instance.tickleStopState.OnExit += OnExitTickleStopState;

        GameManager.instance.loseState.OnEnter += OnEnterLoseState;
        GameManager.instance.loseState.OnExit += OnExitLoseState;

        GameManager.instance.loseState.OnEnter += OnEnterLoseState;
        GameManager.instance.loseState.OnExit += OnExitLoseState;

        GameManager.instance.resultsState.OnEnter += OnEnterResultsState;
        GameManager.instance.resultsState.OnExit += OnExitResultsState;
    }
    private void OnDisable()
    {
        GameManager.instance.titleScreenState.OnEnter -= OnEnterTitlescreenState;
        GameManager.instance.titleScreenState.OnExit -= OnExitTitlescreenState;

        GameManager.instance.runState.OnEnter -= OnEnterRunState;
        GameManager.instance.runState.OnExit -= OnExitRunState;

        GameManager.instance.tickleState.OnEnter -= OnEnterTickleState;
        GameManager.instance.tickleState.OnExit -= OnExitTickleState;

        GameManager.instance.tickleStopState.OnEnter -= OnEnterTickleStopState;
        GameManager.instance.tickleStopState.OnExit -= OnExitTickleStopState;

        GameManager.instance.loseState.OnEnter -= OnEnterLoseState;
        GameManager.instance.loseState.OnExit -= OnExitLoseState;

        GameManager.instance.resultsState.OnEnter -= OnEnterResultsState;
        GameManager.instance.resultsState.OnExit -= OnExitResultsState;
    }

    private void Update()
    {
        if (GameManager.instance.CurrentState is StateRun)
        {
            // GAME UI - mouse hover
            mouseHoverFillBar.fillAmount = 1 - GameManager.instance.menino.MouseHoverProgress;
            mouseHoverBarParent.position = Camera.main.WorldToScreenPoint(GameManager.instance.menino.transform.position);

            if (GameManager.instance.menino.TotalTickes > 0)
                liveTickleCounter.text = GameManager.instance.menino.TotalTickes.ToString();
            else
                liveTickleCounter.text = "";
        }
        else if (GameManager.instance.CurrentState is StateTickle)
        {
            // GAME UI - tickle
            tickleCounter.text = GameManager.instance.menino.TickleCounter.ToString();
            tickleTimerFillbar.fillAmount = GameManager.instance.menino.TickleTimeNormalized;
        }
        else if(GameManager.instance.CurrentState is StateLose)
        {
            loseScreenPressAnyKey.alpha = GameManager.instance.CanSkipLoseScreen ? 1 : 0;
        }

        // DEBUG
        if (Input.GetKeyDown(KeyCode.Tab))
            debugUI.alpha = debugUI.alpha > 0 ? 0 : 1f;

        stateDebugText.text = "state: " + GameManager.instance.CurrentState.GetType().Name;
    }

    private void OnEnterTitlescreenState() { print("OnEnterTitlescreenState"); titleScreenUI.alpha = 1f; GameManager.instance.ResetGame(); }
    private void OnExitTitlescreenState() { titleScreenUI.alpha = 0f; }

    private void OnEnterRunState() { runUI.alpha = 1f; }
    private void OnExitRunState() { runUI.alpha = 0f; }

    private void OnEnterTickleState() {tickleUI.alpha = 1f; }
    private void OnExitTickleState() { tickleUI.alpha = 0f; }

    private void OnEnterTickleStopState() { tickleStopUI.alpha = 1f; }
    private void OnExitTickleStopState() { tickleStopUI.alpha = 0f; }

    private void OnEnterLoseState()
    {
        loseScreenPressAnyKey.text = GameManager.instance.loseCondition.ToString();
        loseUI.alpha = 1f;
    }
    private void OnExitLoseState()
    {
        loseUI.alpha = 0f;
    }

    private void OnEnterResultsState()
    {
        ticklesText.text = "Tickles: " + GameManager.instance.menino.TotalTickes;
        resultsUI.alpha = 1f;

        if (GameManager.instance.loseCondition == GameManager.LoseConditions.TooManyTickles)
            resultsBackground.color = new UnityEngine.Color(0f, 0f, 0f, 1f);

        DOVirtual.Float(-1f, 1f, 1f, (value) =>
        {
            if (value < 0f)
            {
                ticklesText.alpha = 0f;
                if (GameManager.instance.loseCondition == GameManager.LoseConditions.Runaway)
                    resultsBackground.color = new UnityEngine.Color(resultsBackground.color.r, resultsBackground.color.g, resultsBackground.color.b, 0f);
            }
            else
            {
                ticklesText.alpha = value;
                if (GameManager.instance.loseCondition == GameManager.LoseConditions.Runaway)
                    resultsBackground.color = new UnityEngine.Color(resultsBackground.color.r, resultsBackground.color.g, resultsBackground.color.b, value);
            }
        });
    }
    private void OnExitResultsState()
    {
        resultsUI.alpha = 0f;
    }
}
