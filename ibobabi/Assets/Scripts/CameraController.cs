using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom")]
    public float zoomInCameraSize = 1f;
    public float zoomOutCameraSize = 5f;
    public float zoomLerpSpeed = 0.5f;
    private float zoomTarget;

    [Header("Follow")]
    public float moveLerpSpeed = 0.5f;
    private Vector3 targetPos;

    private Camera cam;
    private Vector3 defaultPosition;

    private void Awake()
    {
        defaultPosition = transform.position;
        targetPos = defaultPosition;
        cam = GetComponent<Camera>();

        zoomTarget = zoomOutCameraSize;
    }
    private void Start()
    {
        GameManager.instance.titleScreenState.OnEnter += ZoomOut;
        GameManager.instance.tickleState.OnEnter += ZoomIn;
        GameManager.instance.tickleState.OnExit += ZoomOut;
        GameManager.instance.tickleStopState.OnEnter += ZoomIn;
    }
    private void OnDisable()
    {
        GameManager.instance.titleScreenState.OnEnter -= ZoomOut;
        GameManager.instance.tickleState.OnEnter -= ZoomIn;
        GameManager.instance.tickleState.OnExit -= ZoomOut;
        GameManager.instance.tickleStopState.OnEnter -= ZoomIn;
    }

    void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, zoomLerpSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveLerpSpeed * Time.deltaTime);
    }

    public void ZoomIn()
    {
        zoomTarget = zoomInCameraSize;
        targetPos = GameManager.instance.menino.transform.position + new Vector3(0f, 0f, -10f);
    }
    public void ZoomOut()
    {
        zoomTarget = zoomOutCameraSize;
        targetPos = defaultPosition;
    }
}
