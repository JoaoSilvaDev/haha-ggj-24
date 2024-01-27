using UnityEngine;

public class Menino : MonoBehaviour
{
    [Header("MOVEMENT")]
    public float walkSpeed = 5f;
    public float rotationSpeed = 45f; // Degrees per second
    public float minWalkDuration, maxWalkDuration;
    public Bounds bounds;
    private float currentWalkDuration;
    private float walkTimer;
    private Vector3 moveDirection;
    private Vector3 debugTargetPoint;  

    [Header("MOUSE")]
    private float mouseHoverProgress = 0f;
    public float MouseHoverProgress { get { return mouseHoverProgress; } }
    public float mouseHoverIncreaseSpeed = 1f;
    public float mouseHoverDecreaseSpeed = 1f;
    private bool mouseHover = false;
    public float visualHitFrequency = 0.5f;
    private float visualHitTime = 0f;

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
        }
        else if (GameManager.instance.CurrentState is StateLose)
        {
        }

    }
    
    void OnMouseOver() { mouseHover = true; }
    void OnMouseExit() { mouseHover = false; }
    #endregion

    #region MOVEMENT
    private void OnStartRun()
    {
        SetNewWalkDuration();
    }
    private void Run()
    {
        WalkStraight();

        walkTimer -= Time.deltaTime;

        if (walkTimer <= 0f)
        {
            // 5% chance of staying still
            if (Random.Range(0, 20) > 19)
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
        currentWalkDuration = Random.Range(minWalkDuration, maxWalkDuration);
        walkTimer = currentWalkDuration;
        moveDirection = GetRandomDirectionInsideBounds();
    }

    private Vector2 GetRandomDirectionInsideBounds()
    {
        for(int i = 0; i < 1000; i++)
        {
            Vector2 dir = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) * Vector2.up;
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

    #region MOUSE HOVER
    private void MouseHoverUpdate()
    {
        if (mouseHover)
        {
            // mouse progress
            mouseHoverProgress += mouseHoverIncreaseSpeed * Time.deltaTime;
            if (mouseHoverProgress > 1f)
                mouseHoverProgress = 1f;

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

    private void VisualHit()
    {

    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.DrawWireSphere(debugTargetPoint, 0.5f);
    }
}
