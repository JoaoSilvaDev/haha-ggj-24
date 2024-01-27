using System.Linq;
using UnityEditor;
using UnityEngine;

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

        //if (Input.GetKeyDown(KeyCode.Alpha1)) SetState(titleScreenState);
        //if (Input.GetKeyDown(KeyCode.Alpha2)) SetState(runState);
        //if (Input.GetKeyDown(KeyCode.Alpha3)) SetState(tickleState);
        //if (Input.GetKeyDown(KeyCode.Alpha4)) SetState(tickleStopState);
        //if (Input.GetKeyDown(KeyCode.Alpha5)) SetState(loseState);
    }

    private void SetState(IState targetState)
    {
        if(targetState == currentState) return;

        if (currentState != null)
            currentState.OnExitState();

        currentState = targetState;

        currentState.OnEnterState();
    }

    private void GoToTickleState()
    {
        // Set Tickle Goal
        currentTickleLevel++;
        if (currentTickleLevel < tickleLevels.Length)
            currentTickleGoal = tickleLevels[currentTickleLevel];
        else
            currentTickleGoal += tickleIncrementAfterFinishedGoals;
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
        SetState(loseState);
    }
}