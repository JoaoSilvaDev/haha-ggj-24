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
    public float visualHitScaleAmount = 0.2f;
    private bool mouseHover = false;
    private float visualHitTime = 0f;

    [Header("FALL")]
    public float fallDuration = 1f;
    private float fallTimer = 0f;

    [Header("TICKLE")]
    public float tickleDuration = 10f;
    private float currentTickleTimer = -1f;
    public float TickleTimeNormalized {  get { return currentTickleTimer/tickleDuration; } }
    private int tickleCounter = 0;
    public int TickleCounter { get { return tickleCounter; } }

    [Header("TICKLE STOP")]
    public float minStopDuration = 2f;
    public float maxStopDuration = 4f;
    private float stopTiming = -1f;
    private float stopDuration = 1f;
    private float stopTimer = 0f;
    private float stopTimerInputBuffer = 0.5f;
    private bool hasStoppedinCurrentTickleSequence = false;

    [Header("TICKLE FINISHED SLIGHTLY ANGRY")]
    public float tickleFinishedDuration = 3f;
    private float tickleFinishedTimer = 0f;

    [Header("RUNAWAY")]
    public float runawayScreenDuration = 2f;
    private float runawayScreenTimer = 0f;

    [Header("RESULTS")]
    public float resultsTimeBeforeSkip = 0.7f;
    private float resultsBeforeSkipTimer = 0f;

    [Header("CURSOR")]
    public Texture2D cursorDefault;
    public Texture2D cursorClicked;
    public Texture2D cursorHoverDefault;
    public Texture2D cursorHoverClicked;

    private int totalTickles = 0;
    public int TotalTickes {  get { return totalTickles; } }

    [Header("CAMERA SHAKE")]
    public float cameraShakeHit = 0.02f;
    public float cameraShakeHitDuration = 0.2f;
    public float cameraShakeTickle = 0.02f;
    public float cameraShakeTickleDuration = 0.02f;

    public Animator anim;
    public meninoVFX vfx;
    public SpriteRenderer rend;

    public Action OnCompleteHoverBar;
    public Action OnCompleteFallAnimation;
    public Action OnStopTickle;
    public Action OnCompleteTickleCount;
    public Action OnClickedTitleScreenMenino;
    public Action OnFinishedTickleTimer;
    public Action OnTickledDuringStopTime;
    public Action OnFinishedStopTickleTimer;
    public Action OnFinishedFinishedTickleTimer;
    public Action OnGoToResultsScreen;
    public Action OnSkipResultsScreen;

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
        else if (GameManager.instance.CurrentState is StateFall)
        {
            SoundManager.instance.StopSound("run-loop-breath");
            FallStateUpdate();
        }
        else if (GameManager.instance.CurrentState is StateTickle)
        {
            TickleInputUpdate();
            TickleTimeUpdate();
        }
        else if (GameManager.instance.CurrentState is StateTickleStop)
        {
            anim.Play("stop-closeup");
            TickleStopTimeUpdate();
            if (stopTimer > stopTimerInputBuffer)
            {
                if (AnyLetterKeyDown())
                {
                    anim.Play("gun");
                    OnTickledDuringStopTime();
                }
            }
        }
        else if (GameManager.instance.CurrentState is StateTickleFinished)
        {
            TickleFinishedTimeUpdate();
        }
        else if(GameManager.instance.CurrentState is StateLose && GameManager.instance.loseCondition == GameManager.LoseConditions.Runaway)
        {
            moveDirection = Vector2.up;
            WalkStraight();
            runawayScreenTimer += Time.deltaTime;
            if (runawayScreenTimer > runawayScreenDuration)
                GoToResultsScreen();
        }
        else if(GameManager.instance.CurrentState is StateResults)
        {
            resultsBeforeSkipTimer += Time.deltaTime;
            if(resultsBeforeSkipTimer >= resultsTimeBeforeSkip)
            {
                if (AnyLetterKeyDown())
                    OnSkipResultsScreen?.Invoke();
            }
        }

        // check click
        if (mouseHover && Input.GetMouseButtonDown(0))
            OnClickedMenino();

        UpdateAnimation();

        if(mouseHover)
        {
            if (Input.GetMouseButton(0))
                Cursor.SetCursor(cursorHoverClicked, Vector2.zero, CursorMode.Auto);
            else
                Cursor.SetCursor(cursorHoverDefault, Vector2.zero, CursorMode.Auto);

        }
        else
        {
            if(Input.GetMouseButton(0))
                Cursor.SetCursor(cursorClicked, Vector2.zero, CursorMode.Auto);
            else
                Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        }

    }
    void OnMouseOver() { mouseHover = true; }
    void OnMouseExit() { mouseHover = false; }
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

    private void UpdateAnimation()
    {
        if(GameManager.instance.CurrentState is StateTitleScreen)
        {
            anim.Play("sleep");
        }
        else if (GameManager.instance.CurrentState is StateRun ||
            (GameManager.instance.CurrentState is StateLose && GameManager.instance.loseCondition == GameManager.LoseConditions.Runaway))
        {
            if (moveDirection.y > 0.1f)
                anim.Play("walk-up");
            else if(moveDirection.y < 0.1f)
                anim.Play("walk-down");
            else
                anim.Play("idle");

            if (moveDirection.x > 0)
                rend.flipX = false;
            else if (moveDirection.x < 0)
                rend.flipX = true;
        }
        else if (GameManager.instance.CurrentState is StateTickle)
        {
            anim.Play("laughs");
        }
    }
    
    public void ResetMenino()
    {
        transform.position = new Vector3(
            UnityEngine.Random.Range(-6f, 6f),
            UnityEngine.Random.Range(-3f, 3f),
            0f);

        walkTimer = 0f;
        totalTickles = 0;
        runawayScreenTimer = 0f;
        resultsBeforeSkipTimer = 0f;
    }

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
            {
                StopMovement(minWalkDuration);
            }
            else
            {
                SetNewWalkDuration();
            }
        }
    }
    private void WalkStraight()
    {
        if(GameManager.instance.CurrentState is not StateLose)
        {
            if(moveDirection.magnitude > 0.01f)
                SoundManager.instance.PlaySound("run-loop-breath", true); 
            else
                SoundManager.instance.StopSound("run-loop-breath");
        }

        transform.position += moveDirection * walkSpeed * Time.deltaTime;
    }
    
    // this is called as an animation event
    public void RunStepSound()
    {
        SoundManager.instance.PlaySound("run-step");
    }

    public void SlightlyAngrySound()
    {
        SoundManager.instance.PlaySound("slightly-angry");
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

    public void Fall()
    {
        fallTimer = 0f;
        anim.Play("fall");
    }

    private void FallStateUpdate()
    {
        fallTimer += Time.deltaTime;
        if (fallTimer > fallDuration)
            OnCompleteFallAnimation?.Invoke();
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
                HoverVisualHit();
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
        {
            OnClickedTitleScreenMenino?.Invoke();
        }
    }

    private void HoverVisualHit()
    {
        vfx.ScaleHit(UnityEngine.Random.Range(visualHitScaleAmount, visualHitScaleAmount), true, false, 0.2f);
        vfx.FlashHit(Color.white, 0.2f);
        SoundManager.instance.PlaySound("slap");
        CameraController.instance.Shake(cameraShakeHitDuration, cameraShakeHit);
    }
    #endregion

    #region TICKLE
    public void SetTickleTarget(int target)
    {
        tickleCounter = target;
    }

    public void StartTickleTimer()
    {
        currentTickleTimer = tickleDuration;
    }

    public void SetStopTimer()
    {
        hasStoppedinCurrentTickleSequence = false;
        stopTiming = tickleDuration * (1 - UnityEngine.Random.Range(0.4f, 0.6f));
        stopDuration = UnityEngine.Random.Range(minStopDuration, maxStopDuration);
    }

    public void TickelStopZoomOut()
    {
        rend.flipX = false;
        CameraController.instance.ZoomIn();
    }

    public void TickleStopZoomInMore()
    {
        rend.flipX = false;
        CameraController.instance.ZoomInMore();
    }

    public void TickleTimeUpdate()
    {
        currentTickleTimer -= Time.deltaTime;

        if (currentTickleTimer <= 0f)
            FinishedTickleTimer();

        if (currentTickleTimer < stopTiming && !hasStoppedinCurrentTickleSequence)
            StopTickle();
    }

    public void FinishedTickleTimer()
    {
        OnFinishedTickleTimer?.Invoke();
    }

    private void TickleInputUpdate()
    {
        if(AnyLetterKeyDown())
            Tickle();
    }

    private void Tickle()
    {
        SoundManager.instance.PlaySound("laugh");
        vfx.ScaleHit(UnityEngine.Random.Range(visualHitScaleAmount, visualHitScaleAmount), true, false, 0.2f);
        vfx.FlashHit(Color.white, 0.1f);
        rend.flipX = (UnityEngine.Random.Range(0f,1f) > 0.5f);

        CameraController.instance.Shake(cameraShakeTickleDuration, cameraShakeTickle);

        anim.Play("laughs", 0, UnityEngine.Random.Range(0f, 1f));

        tickleCounter--;
        totalTickles++;

        if (tickleCounter <= 0)
            OnCompleteTickleCount?.Invoke();
    }

    private void TickleStopTimeUpdate()
    {
        stopTimer += Time.deltaTime;
        if (stopTimer >= stopDuration)
        {
            stopTimer = 0f;
            OnFinishedStopTickleTimer?.Invoke();
        }
    }

    private void StopTickle()
    {
        print("StopTickle");
        SoundManager.instance.StopAllSounds();
        SoundManager.instance.PlaySound("stop-before");
        stopTimer = 0f;
        hasStoppedinCurrentTickleSequence = true;
        rend.flipX = false;
        OnStopTickle?.Invoke();
    }

    public void GunReloadSound()
    {
        SoundManager.instance.PlaySound("gun-reload");
    }

    public void MeninoStayStop()
    {
        SoundManager.instance.PlaySound("say-stop");
    }

    private void TickledDuringStopTime()
    {
        OnTickledDuringStopTime?.Invoke();
    }

    public void StartTickleFinishedSequence()
    {
        anim.Play("slightly-angry");
        tickleFinishedTimer = 0f;
    }

    private void TickleFinishedTimeUpdate()
    {
        tickleFinishedTimer += Time.deltaTime;
        if (tickleFinishedTimer >= tickleFinishedDuration)
        {
            runawayScreenTimer = 0f;
            OnFinishedFinishedTickleTimer?.Invoke();
        }
    }

    public void GoToResultsScreen()
    {
        OnGoToResultsScreen?.Invoke();
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.DrawWireSphere(debugTargetPoint, 0.2f);
    }
}
