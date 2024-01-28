using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("GAME OBJECTS")]
    public Menino menino;

    [Header("TICKLE SETTINGS")]
    public int[] tickleLevels = new int[0];
    private int currentTickleLevel = 0;
    private int currentTickleGoal = 0;
    public int tickleIncrementAfterFinishedGoals = 20;

    [Header("OTHER VISUALS")]
    public Animator background;

    private IState currentState = null;
    public IState CurrentState { get { return currentState; } }
    public StateTitleScreen titleScreenState;
    public StateRun runState;
    public StateFall fallState;
    public StateTickle tickleState;
    public StateTickleStop tickleStopState;
    public StateTickleFinished tickleFinishedState;
    public StateLose loseState;
    public StateResults resultsState;

    [Header("LOSE STATE")]
    private float loseStateTimeBeforeAllowSkip = 0.5f;
    private float loseStateTimer = 0f;
    public bool CanSkipLoseScreen { get { return loseStateTimer > loseStateTimeBeforeAllowSkip; } }
    public enum LoseConditions { Runaway, TooManyTickles }
    public LoseConditions loseCondition;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        titleScreenState = new StateTitleScreen();
        runState = new StateRun();
        fallState = new StateFall();
        tickleState = new StateTickle();
        tickleStopState = new StateTickleStop();
        tickleFinishedState = new StateTickleFinished();
        loseState = new StateLose();
        resultsState = new StateResults();

        ResetGame();

        SetState(titleScreenState);
    }

    private void OnEnable()
    {
        menino.OnClickedTitleScreenMenino += GoToRunState;
        menino.OnCompleteHoverBar += GoToFallState;
        menino.OnCompleteFallAnimation += StartTickleState;
        menino.OnStopTickle += GoToTickleStopState;
        menino.OnFinishedStopTickleTimer += GoBackToTickle;
        menino.OnCompleteTickleCount += GoToTickleFinishedState;
        menino.OnFinishedFinishedTickleTimer += GoToRunState;
        menino.OnFinishedTickleTimer += GoToLoseStateRunaway;
        menino.OnTickledDuringStopTime += GoToLoseStateTooManyTickles;
        menino.OnGoToResultsScreen += GoToResultsScreen;
        menino.OnSkipResultsScreen += GoToTitleScreen;

        tickleStopState.OnExit += FadeOutStopBackground;
    }

    private void OnDisable()
    {
        menino.OnClickedTitleScreenMenino -= GoToRunState;
        menino.OnCompleteHoverBar -= GoToFallState;
        menino.OnCompleteFallAnimation -= StartTickleState;
        menino.OnStopTickle -= GoToTickleStopState;
        menino.OnFinishedStopTickleTimer -= GoBackToTickle;
        menino.OnCompleteTickleCount -= GoToTickleFinishedState;
        menino.OnFinishedFinishedTickleTimer -= GoToRunState;
        menino.OnFinishedTickleTimer -= GoToLoseStateRunaway;
        menino.OnTickledDuringStopTime -= GoToLoseStateTooManyTickles;
        menino.OnGoToResultsScreen -= GoToResultsScreen;
        menino.OnSkipResultsScreen -= GoToTitleScreen;

        tickleStopState.OnExit -= FadeOutStopBackground;
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void ResetGame()
    {
        menino.ResetMenino();
        loseStateTimer = 0f;
        SetTickleLevel(0);
    }

    private void SetState(IState targetState)
    {
        if(targetState == currentState) return;

        if (currentState != null)
            currentState.OnExitState();

        currentState = targetState;

        currentState.OnEnterState();
    }

    private void SetTickleLevel(int i)
    {
        currentTickleLevel = i;

        if (currentTickleLevel < tickleLevels.Length)
            currentTickleGoal = tickleLevels[currentTickleLevel];
        else
            currentTickleGoal += tickleIncrementAfterFinishedGoals;
    }

    private void StartTickleState()
    {
        // Set Tickle level
        SetTickleLevel(currentTickleLevel);
        currentTickleLevel++;
        menino.SetTickleTarget(currentTickleGoal);

        // Start Tickel Timer
        menino.StartTickleTimer();
        
        // Set Stop Timer
        menino.SetStopTimer();

        SoundManager.instance.PlaySound("tickle-start");

        // Change State
        SetState(tickleState);
    }

    void FadeOutStopBackground()
    {
        background.Play("bg-red-fadeout");
    }


    // called when going back to TickleState from StopTickle
    private void GoBackToTickle()
    {
        SoundManager.instance.PlaySound("tickle-start");
        FadeOutStopBackground();
        // Change State        
        SetState(tickleState);
    }

    private void GoToTickleStopState()
    {
        background.Play("bg-red-fadein");
        SetState(tickleStopState);
    }

    private void GoToTickleFinishedState()
    {
        menino.StartTickleFinishedSequence();
        SetState(tickleFinishedState);
    }

    private void ResetTickleLevel()
    {
        currentTickleLevel = 0;
    }
    private void GoToRunState()
    {
        SetState(runState);
    }

    private void GoToFallState()
    {
        menino.Fall();
        SetState(fallState);
    }
    private void GoToLoseStateRunaway()
    {
        CameraController.instance.ZoomOut();
        loseCondition = LoseConditions.Runaway;
        loseStateTimer = 0f;
        SetState(loseState);
    }
    private void GoToLoseStateTooManyTickles()
    {
        SoundManager.instance.PlaySound("lose-too-many-tickles");
        loseCondition = LoseConditions.TooManyTickles;
        loseStateTimer = 0f;
        SetState(loseState);
    }

    private void GoToTitleScreen()
    {
        SetState(titleScreenState);
    }

    private void GoToResultsScreen()
    {
        SetState(resultsState);
    }
}