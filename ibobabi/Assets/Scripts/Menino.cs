using System;
using UnityEngine;

public class Menino : MonoBehaviour
{
    [Header("MOVEMENT")]
    public float walkSpeed = 5f;
    public float minWalkDuration, maxWalkDuration;
    public Bounds bounds;
    private float currentWalkDuration;
    private float walkTimer;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 debugTargetPoint;  

    [Header("MOUSE HOVER")]
    public float mouseHoverIncreaseSpeed = 1f;
    public float mouseHoverDecreaseSpeed = 1f;
    private float mouseHoverProgress = 0f;
    public float MouseHoverProgress { get { return mouseHoverProgress; } }

    [Header("MOUSE HOVER VISUALS")]
    public float visualHitFrequency = 0.5f;
    private bool mouseHover = false;
    private float visualHitTime = 0f;

    [Header("TICKLE")]
    public float minStopWaitDuration, maxStopWaitDuration;
    public float minStopDuration, maxStopDuration;
    private float stopDuration, stopWaitDuration;
    private int tickleCounter = 0;
    public int TickleCounter { get {  return tickleCounter; } }

    public Action OnCompleteHoverBar;
    public Action OnCompleteTickleCount;
    public Action OnClickedTitleScreenMenino;

    #region UNITY FUNCTIONS
    private void Start()
    {
        GameManager.instance.runState.OnEnter += OnStartRun;
    }
    private void OnDisable()
    {
        GameManager.instance.runState.OnEnter -= OnStartRun;
    }

    private void Update()
    {
        if (GameManager.instance.CurrentState is StateRun)
        {
            Run();
            MouseHoverUpdate();
        }
        else if (GameManager.instance.CurrentState is StateTickle)
        {
            TickleInputUpdate();
        }

        // check click
        if (mouseHover && Input.GetMouseButtonDown(0))
            OnClickedMenino();

    }
    
    void OnMouseOver() { mouseHover = true; }
    void OnMouseExit() { mouseHover = false; }
    #endregion

    #region MOVEMENT
    private void OnStartRun()
    {
        SetNewWalkDuration();
        ResetHoverProgress();
    }
    private void Run()
    {
        WalkStraight();

        walkTimer -= Time.deltaTime;

        if (walkTimer <= 0f)
        {
            // 5% chance of staying still
            if (UnityEngine.Random.Range(0, 20) > 19)
                StopMovement(minWalkDuration);
            else
                SetNewWalkDuration();
        }
    }
    private void WalkStraight()
    {
        transform.position += moveDirection * walkSpeed * Time.deltaTime;
    }

    private void StopMovement(float duration)
    {
        currentWalkDuration = duration;
        walkTimer = currentWalkDuration;
        moveDirection = Vector2.zero;
    }

    private void SetNewWalkDuration()
    {
        currentWalkDuration = UnityEngine.Random.Range(minWalkDuration, maxWalkDuration);
        walkTimer = currentWalkDuration;
        moveDirection = GetRandomDirectionInsideBounds();
    }

    private Vector2 GetRandomDirectionInsideBounds()
    {
        for(int i = 0; i < 1000; i++)
        {
            Vector2 dir = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)) * Vector2.up;
            debugTargetPoint = transform.position + new Vector3(dir.x, dir.y, 0f) * walkSpeed;
            if (IsPositionInsideBounds(debugTargetPoint, bounds))
                return dir.normalized;
        }

        Debug.LogWarning("Did not find an appropriate direction in 1000 iterations??? - Setting walk direction towards center (0,0)");

        return -transform.position.normalized;
    }

    private bool IsPositionInsideBounds(Vector2 position, Bounds bounds)
    {
        return position.x > bounds.min.x && position.x < bounds.max.x &&
                position.y > bounds.min.y && position.y < bounds.max.y;
    }

    #endregion

    #region MOUSE
    private void MouseHoverUpdate()
    {
        if (mouseHover)
        {
            // mouse progress
            mouseHoverProgress += mouseHoverIncreaseSpeed * Time.deltaTime;
            if (mouseHoverProgress > 1f)
            {
                mouseHoverProgress = 1f;
                OnCompleteHoverBar?.Invoke();
            }

            // visual stuff
            visualHitTime += Time.deltaTime;
            if(visualHitTime > visualHitFrequency)
            {
                VisualHit();
                visualHitTime = 0f;
            }
        }
        else
        {
            // mouse progress
            mouseHoverProgress -= mouseHoverDecreaseSpeed * Time.deltaTime;
            if(mouseHoverProgress < 0f)
                mouseHoverProgress = 0f;
            
            // visual stuff
            visualHitTime = 0f;
        }
    }

    private void ResetHoverProgress()
    {
        mouseHoverProgress = 0f;
    }

    private void OnClickedMenino()
    {
        if (GameManager.instance.CurrentState is StateTitleScreen)
            OnClickedTitleScreenMenino?.Invoke();
    }

    private void VisualHit()
    {

    }
    #endregion

    #region TICKLE
    public void SetTickleTarget(int target)
    {
        tickleCounter = target;
        SetStopTimer();
    }

    public void SetStopTimer()
    {
        stopWaitDuration = UnityEngine.Random.Range(minStopWaitDuration, maxStopWaitDuration);
        stopDuration = UnityEngine.Random.Range(minStopDuration, maxStopDuration);
    }

    private void TickleInputUpdate()
    {
        if(AnyLetterKeyDown())
            Tickle();
    }

    private void Tickle()
    {
        tickleCounter--;

        // call tickle visuals from here

        if (tickleCounter <= 0)
            OnCompleteTickleCount?.Invoke();
    }

    private bool AnyLetterKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.A)) return true;
        else if (Input.GetKeyDown(KeyCode.B)) return true;
        else if (Input.GetKeyDown(KeyCode.C)) return true;
        else if (Input.GetKeyDown(KeyCode.D)) return true;
        else if (Input.GetKeyDown(KeyCode.E)) return true;
        else if (Input.GetKeyDown(KeyCode.F)) return true;
        else if (Input.GetKeyDown(KeyCode.G)) return true;
        else if (Input.GetKeyDown(KeyCode.H)) return true;
        else if (Input.GetKeyDown(KeyCode.I)) return true;
        else if (Input.GetKeyDown(KeyCode.J)) return true;
        else if (Input.GetKeyDown(KeyCode.K)) return true;
        else if (Input.GetKeyDown(KeyCode.L)) return true;
        else if (Input.GetKeyDown(KeyCode.M)) return true;
        else if (Input.GetKeyDown(KeyCode.N)) return true;
        else if (Input.GetKeyDown(KeyCode.O)) return true;
        else if (Input.GetKeyDown(KeyCode.P)) return true;
        else if (Input.GetKeyDown(KeyCode.Q)) return true;
        else if (Input.GetKeyDown(KeyCode.R)) return true;
        else if (Input.GetKeyDown(KeyCode.S)) return true;
        else if (Input.GetKeyDown(KeyCode.T)) return true;
        else if (Input.GetKeyDown(KeyCode.U)) return true;
        else if (Input.GetKeyDown(KeyCode.V)) return true;
        else if (Input.GetKeyDown(KeyCode.W)) return true;
        else if (Input.GetKeyDown(KeyCode.X)) return true;
        else if (Input.GetKeyDown(KeyCode.Y)) return true;
        else if (Input.GetKeyDown(KeyCode.Z)) return true;
        return false;
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.DrawWireSphere(debugTargetPoint, 0.5f);
    }
}
