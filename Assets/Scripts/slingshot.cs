using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class slingshot : MonoBehaviour
{
    public LineRenderer[] lineRenderers ; 
    public Transform[] stripPositions ;
    public Transform anchor ; 
    bool isMouseDown ;
    public Vector3 currentPosition ; 
    public GameObject ballPrefab ; 
    public float ballPositionOffset ; 
    public float spawnDelay = 1.5f ; 

    Rigidbody2D ball ; 
    Collider2D ballCollider ; 

    public float force ; 
    public float maxPullBack ; 

    void Start() {
        lineRenderers[0].positionCount = 2 ; 
        lineRenderers[1].positionCount = 2 ; 
        lineRenderers[0].SetPosition(0, stripPositions[0].position) ; 
        lineRenderers[1].SetPosition(0, stripPositions[1].position) ; 

        CreateBall() ; 
    }

    void Update() {

        if (isMouseDown) {
            Vector3 mousePosition = Input.mousePosition ; 
            mousePosition.z = 10 ; 

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition) ; 
            currentPosition = anchor.position + Vector3.ClampMagnitude(currentPosition - anchor.position, maxPullBack) ; 
            SetStrips(currentPosition) ; 

            if (ballCollider) {
                ballCollider.enabled = true ; 
            }
        }

        else {
            ResetStrips() ; 
        }
    }

    private void OnMouseDown() {
        isMouseDown = true ; 
    }

    private void OnMouseUp () {
        Shoot() ; 
        isMouseDown = false ; 
    }

    void ResetStrips() {
        currentPosition = anchor.position ; 
        SetStrips(currentPosition) ; 
    }

    void SetStrips(Vector3 position) {
        lineRenderers[0].SetPosition(1, position) ; 
        lineRenderers[1].SetPosition(1, position) ; 

        Vector3 dir = position - anchor.position ; 
        ball.transform.position = position + dir.normalized * ballPositionOffset ; 
        return ; 
    }

    void CreateBall() {
        ball = Instantiate(ballPrefab).GetComponent<Rigidbody2D>() ; 
        ballCollider = ball.GetComponent<Collider2D>() ; 
        ballCollider.enabled = false ; 

        ball.isKinematic = true ; 
    }

    void Shoot() {
        ball.isKinematic = false ; 

        Vector3 ballForce = (currentPosition - anchor.position) * force * -1 ; 
        ball.velocity = ballForce ; 

        ball = null ; 
        ballCollider = null ; 

        // Invoke("CreateBall", 1.5f) ; 
        CreateBall() ; 
    }
}

