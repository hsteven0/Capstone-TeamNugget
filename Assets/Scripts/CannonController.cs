using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    // Cannon Controller (rotate)
    private Vector2 CannonPosition ;
    private Vector2 MousePosition ; 
    private Vector2 direction ; 

    // CannonBall Controller
    public GameObject CannonBall ; 
    public float firepower ; 
    public Transform FirePoint ; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetMouseButtonDown(0)) {
            Fire() ; 
        }
        CannonRotate() ; 
    }

    public void CannonRotate()
    {
        CannonPosition = transform.position ; 
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) ; 
        direction = MousePosition - CannonPosition ; 
        transform.right = direction ; 
    }

    public void Fire() 
    {
        GameObject new_CannonBall = Instantiate(CannonBall, FirePoint.position, FirePoint.rotation) ;
        new_CannonBall.GetComponent<Rigidbody2D>().velocity = transform.right * firepower ; 
    }
}
