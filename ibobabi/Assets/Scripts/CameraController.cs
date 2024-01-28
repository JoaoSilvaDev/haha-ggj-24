using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom")]
    public float zoomOutCameraSize = 5f;
    public float zoomMidCameraSize = 1f;
    public float zoomInCameraSize = 1f;
    public float zoomInMoreCameraSize = 1f;
    public float zoomLerpSpeedDefault = 25f;
    public float zoomLerpSpeedFaster = 50f;
    public float zoomLerpSpeedSlower = 50f;
    private float zoomTarget;
    private float zoomLerpSpeed = 0f;

    [Header("Follow")]
    public float moveLerpSpeed = 0.5f;
    private Vector3 targetPos;
    
    private Transform originalTransform;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.7f;
    private float dampingSpeed = 1.0f;

    private Camera cam;
    private Vector3 defaultPosition;

    public static CameraController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        defaultPosition = transform.position;
        targetPos = defaultPosition;
        cam = GetComponent<Camera>();

        zoomTarget = zoomOutCameraSize;
        zoomLerpSpeed = zoomLerpSpeedDefault;
    }
    private void Start()
    {
        GameManager.instance.titleScreenState.OnEnter += ZoomOut;
        GameManager.instance.tickleState.OnEnter += ZoomIn;
        GameManager.instance.tickleFinishedState.OnEnter += ZoomMid;
        GameManager.instance.tickleFinishedState.OnExit += ZoomOut;
    }
    private void OnDisable()
    {
        GameManager.instance.titleScreenState.OnEnter -= ZoomOut;
        GameManager.instance.tickleState.OnEnter -= ZoomIn;
        GameManager.instance.tickleFinishedState.OnEnter += ZoomMid;
        GameManager.instance.tickleFinishedState.OnExit -= ZoomOut;
    }

    void Update()
    {
        if (zoomLerpSpeed < 0)
            cam.orthographicSize = zoomTarget;
        else
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, zoomLerpSpeed * Time.deltaTime);

        if (shakeDuration > 0)
        {
            Vector3 rand = Random.insideUnitSphere * shakeMagnitude;
            transform.position = transform.position + rand;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.position = Vector3.Lerp(transform.position, targetPos, moveLerpSpeed * Time.deltaTime);
        }
    }
    public void ZoomOut()
    {
        zoomTarget = zoomOutCameraSize;
        targetPos = defaultPosition;
        zoomLerpSpeed = zoomLerpSpeedDefault;
    }

    public void ZoomIn()
    {
        zoomTarget = zoomInCameraSize;
        targetPos = GameManager.instance.menino.transform.position + new Vector3(0f, 0f, -10f);
        zoomLerpSpeed = zoomLerpSpeedDefault;
    }

    public void ZoomInMore()
    {
        zoomTarget = zoomInMoreCameraSize;
        targetPos = GameManager.instance.menino.transform.position + new Vector3(0f, 0f, -10f);
        zoomLerpSpeed = zoomLerpSpeedFaster;
    }

    public void ZoomInMoreInstant()
    {
        zoomTarget = zoomInMoreCameraSize;
        targetPos = GameManager.instance.menino.transform.position + new Vector3(0f, 0f, -10f);
        zoomLerpSpeed = -1;
    }

    public void ZoomMid()
    {
        zoomTarget = zoomMidCameraSize;
        targetPos = GameManager.instance.menino.transform.position + new Vector3(0f, 0f, -10f);
        zoomLerpSpeed = zoomLerpSpeedSlower;
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
