using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;


public class Slingshot : MonoBehaviour
{
    // Strips (slingshot sling)
    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform anchor;
    public Vector3 currentPosition;
    public float force; // slingshot collider force: strips with ball
    public float maxPullBack; // limits distance user can pull back slingshot
    // -----------------------
    bool isMouseDown; // checks mouse press -> convert to TouchScript Inputs
    // Ball Spawn
    public GameObject ballPrefab;
    public float ballPositionOffset;
    private float spawnDelay = 1.5f; // delay for ball to spawn
    private bool ballExists = false;

    Rigidbody2D ball;
    Collider2D ballCollider;
    // --------------------------

    void Start() {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        CreateBall();
    }

    void Update() {
        if (isMouseDown) {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = anchor.position + Vector3.ClampMagnitude(currentPosition - anchor.position, maxPullBack);
            SetStrips(currentPosition);

            if (ballCollider) {
                ballCollider.enabled = true;
            }
        }

        else {
            ResetStrips();
        }
    }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedhandler;
        GetComponent<ReleaseGesture>().Released += releasedHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedhandler;
        GetComponent<ReleaseGesture>().Released -= releasedHandler;
    }

    void pressedhandler(object sender, System.EventArgs e)
    {
        isMouseDown = true;
    }

    void releasedHandler(object sender, System.EventArgs e)
    {
        Shoot();
        isMouseDown = false;
    }

    void ResetStrips() {
        currentPosition = anchor.position;
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position) {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (ballExists) { 
            Vector3 dir = position - anchor.position;
            ball.transform.position = position + dir.normalized * ballPositionOffset;
        }
    }

    void CreateBall() {
        ball = Instantiate(ballPrefab).GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<Collider2D>();
        ballCollider.enabled = false;

        ball.isKinematic = true;

        ballExists = true;
    }

    void Shoot() {
        if (ballExists) {
            ball.isKinematic = false; 

            Vector3 ballForce = (currentPosition - anchor.position) * force * -1;
            ball.velocity = ballForce;

            ball = null;
            ballCollider = null;
            ballExists = false;

            Invoke("CreateBall", spawnDelay);
        }
    }
}

