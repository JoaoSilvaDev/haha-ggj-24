using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header("GAME OBJECTS")]
    public Menino menino;

    [Header("TICKLE SETTINGS")]
    public int[] tickleLevels = new int[0];
    private int currentTickleLevel = 0;
    private int currentTickleGoal = 0;
    public int tickleIncrementAfterFinishedGoals = 20;

    private IState currentState = null;
    public IState CurrentState { get { return currentState; } }
    public StateTitleScreen titleScreenState;
    public StateRun runState;
    public StateTickle tickleState;
    public StateTickleStop tickleStopState;
    public StateLose loseState;

    [Header("LOSE STATE")]
    private float loseStateTimeBeforeAllowSkip = 2f;
    private float loseStateTimer = 0f;
    public bool CanSkipLoseScreen { get { return loseStateTimer > loseStateTimeBeforeAllowSkip; } }

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        titleScreenState = new StateTitleScreen();
        runState = new StateRun();
        tickleState = new StateTickle();
        tickleStopState = new StateTickleStop();
        loseState = new StateLose();

        ResetGame();

        SetState(titleScreenState);
    }

    private void OnEnable()
    {
        menino.OnClickedTitleScreenMenino += GoToRunState;
        menino.OnCompleteHoverBar += GoToTickleState;
        menino.OnCompleteTickleCount += GoToRunState;
        menino.OnFinishedTickleTimer += GoToLoseState;
    }

    void Update()
    {
        currentState.UpdateState();

        if(currentState is StateLose)
        {
            loseStateTimer += Time.deltaTime;
            if(CanSkipLoseScreen && Input.anyKeyDown)
                    GoToTitleScreen();
            
        }
    }

    public void ResetGame()
    {
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

    private void GoToTickleState()
    {
        // Set Tickle Goal
        SetTickleLevel(currentTickleLevel);
        currentTickleLevel++;
        menino.SetTickleTarget(currentTickleGoal);

        // Start Tickel Timer
        menino.StartTickleTimer();

        // Change Statex
        SetState(tickleState);
    }

    private void ResetTickleLevel()
    {
        currentTickleLevel = 0;
    }
    private void GoToRunState()
    {
        SetState(runState);
    }
    private void GoToLoseState()
    {
        loseStateTimer = 0f;
        SetState(loseState);
    }

    private void GoToTitleScreen()
    {
        SetState(titleScreenState);
    }
}