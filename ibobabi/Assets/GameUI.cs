using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("MOUSE HOVER BAR")]
    public Image mouseHoverFillBar;
    public RectTransform mouseHoverBarParent;

    [Header("DEBUG")]
    public GameObject debugUI;
    public TextMeshProUGUI stateDebugText;
    private void Start()
    {
        GameManager.instance.runState.OnEnter += OnEnterRunState;
        GameManager.instance.runState.OnExit += OnExitRunState;
        GameManager.instance.titleScreenState.OnEnter += OnEnterTitlescreenState;
        GameManager.instance.titleScreenState.OnExit += OnExitTitlescreenState;
        GameManager.instance.tickleState.OnEnter += OnEnterTickleState;
        GameManager.instance.tickleState.OnExit += OnExitTickleState;
    }
    private void OnDisable()
    {
        GameManager.instance.titleScreenState.OnEnter -= OnEnterTitlescreenState;
        GameManager.instance.titleScreenState.OnExit -= OnExitTitlescreenState;
        GameManager.instance.runState.OnEnter -= OnEnterRunState;
        GameManager.instance.runState.OnExit -= OnExitRunState;
        GameManager.instance.tickleState.OnEnter -= OnEnterTickleState;
        GameManager.instance.tickleState.OnExit -= OnExitTickleState;
    }

    private void Update()
    {
        // GAME UI
        mouseHoverFillBar.fillAmount = GameManager.instance.menino.MouseHoverProgress;
        mouseHoverBarParent.position = Camera.main.WorldToScreenPoint(GameManager.instance.menino.transform.position);


        // DEBUG
        if (Input.GetKeyDown(KeyCode.Tab))
            debugUI.SetActive(!debugUI.activeInHierarchy);

        stateDebugText.text = "state: " + GameManager.instance.CurrentState.GetType().Name;
    }

    private void OnEnterTitlescreenState()
    {
    }

    private void OnExitTitlescreenState()
    {
    }

    private void OnEnterRunState()
    {
        mouseHoverBarParent.gameObject.SetActive(true);
    }

    private void OnExitRunState()
    {
        mouseHoverBarParent.gameObject.SetActive(false);
    }

    private void OnEnterTickleState()
    {
    }

    private void OnExitTickleState()
    {
    }
}
