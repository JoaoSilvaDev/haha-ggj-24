using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("RUN STATE UI")]
    public CanvasGroup runUI;
    public Image mouseHoverFillBar;
    public RectTransform mouseHoverBarParent;

    [Header("TICKLE STATE UI")]
    public CanvasGroup tickleUI;
    public TextMeshProUGUI tickleCounter;

    [Header("TICKLE STOP STATE UI")]
    public CanvasGroup tickleStopUI;

    [Header("LOSE STATE UI")]
    public CanvasGroup loseUI;

    [Header("DEBUG")]
    public CanvasGroup debugUI;
    public TextMeshProUGUI stateDebugText;
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
    }

    private void Update()
    {
        // GAME UI - mouse hover
        mouseHoverFillBar.fillAmount = GameManager.instance.menino.MouseHoverProgress;
        mouseHoverBarParent.position = Camera.main.WorldToScreenPoint(GameManager.instance.menino.transform.position);

        // GAME UI - tickle
        tickleCounter.text = GameManager.instance.menino.TickleCounter.ToString();

        // DEBUG
        if (Input.GetKeyDown(KeyCode.Tab))
            debugUI.alpha = debugUI.alpha == 0 ? 1 : 0;

        stateDebugText.text = "state: " + GameManager.instance.CurrentState.GetType().Name;
    }

    private void OnEnterTitlescreenState()
    {
        runUI.alpha = 0f;
        tickleUI.alpha = 0f;
    }
    private void OnExitTitlescreenState()
    {
    }

    private void OnEnterRunState()
    {
        runUI.alpha = 1f;
    }
    private void OnExitRunState()
    {
        runUI.alpha = 0f;
    }

    private void OnEnterTickleState()
    {
        tickleUI.alpha = 1f;
    }
    private void OnExitTickleState()
    {
        tickleUI.alpha = 0f;
    }

    private void OnEnterTickleStopState()
    {
        tickleStopUI.alpha = 1f;
    }
    private void OnExitTickleStopState()
    {
        tickleStopUI.alpha = 0f;
    }

    private void OnEnterLoseState()
    {
        loseUI.alpha = 1f;
    }
    private void OnExitLoseState()
    {
        loseUI.alpha = 0f;
    }
}
