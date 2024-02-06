using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript ; 
using TouchScript.Gestures;


public class CannonControllerTouchScript : MonoBehaviour
{
    private TapGesture tapGesture;


    // Cannon Controller (rotate)
    private Vector2 CannonPosition;
    private Vector2 TouchPosition;
    private Vector2 direction;

    // CannonBall Controller
    public GameObject CannonBall;
    public float firepower;
    public Transform FirePoint;
    private bool isAiming = false ; 

    private void OnEnable() 
    {
        tapGesture.Tapped += tappedHandler;
    }
    private void OnDisable()
    {
        tapGesture.Tapped -= tappedHandler;
    }
    private void tappedHandler(object sender, System.EventArgs e)
    {
        direction = tapGesture.ScreenPosition;
        Fire() ; 
    }


    // Start is called before the first frame update
    void Start()
    {
        // Get the PressGesture and TransformGesture components
        // pressGesture = GetComponent<PressGesture>();
        // releaseGesture = GetComponent<ReleaseGesture>() ; 

        // // Subscribe to the gesture events
        // pressGesture.Pressed += OnPress;
        // releaseGesture.Released += OnRelease ; 

        tapGesture = GetComponent<TapGesture>();
    }

    // Update is called once per frame
    void Update()
    {
        CannonRotate() ; 
        // if (isAiming) 
        // {
        //     CannonRotate();
        // }
    }

    public void CannonRotate()
    {
        CannonPosition = transform.position;
        if (isAiming)
        {
            TouchPosition = Camera.main.ScreenToWorldPoint(tapGesture.ScreenPosition);
            direction = TouchPosition - CannonPosition;
            transform.right = direction;
        }
    }

    // public void OnPress(object sender, System.EventArgs e)
    // {
    //     isAiming = true ; 
    //     Debug.Log("AIMING") ; 
    // }

    // public void OnRelease(object sender, System.EventArgs e)
    // {
    //     if (isAiming)
    //     {
    //         Fire() ; 
    //         Debug.Log("FIRED") ; 
    //         isAiming = false ; 
    //     }
    // }

    public void Fire()
    {
        GameObject newCannonBall = Instantiate(CannonBall, FirePoint.position, FirePoint.rotation);
        newCannonBall.GetComponent<Rigidbody2D>().velocity = transform.right * firepower;
    }

    // private void OnDestroy()
    // {
    //     // Unsubscribe from the events to prevent memory leaks
    //     pressGesture.Pressed -= OnPress;
    //     releaseGesture.Released -= OnRelease ; 
    // }
}
