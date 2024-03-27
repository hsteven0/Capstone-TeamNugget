using UnityEngine;
using UnityEngine.UI;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using System;

public class Slingshot : MonoBehaviour
{
    public Transform clonedSlingItem, clonedFingerObj, clonedElastic; // SlingItem, Finger, Elastic prefabs
    private Vector3 centeredPos; // center of the Slingshot (where the SlingItem is placed)
    private GameObject controlObject; // object that controls the slingshot (both shoot and angle change)
    private Vector2 controlStartPos; // save start position to calculate translation positions
    private GameObject slingItem, fingerObj, elasticObj; // clone of what is being slung, the finger itself, elastic string
    private int activeLayer; // a reference to the "Active" layer
    private const float diameter = 3.75f; // circlular bounds for SlingItem when aiming
    private LineRenderer lineRenderer;

    void Start()
    {
        activeLayer = LayerMask.NameToLayer("Active");
        centeredPos = new Vector3(gameObject.transform.position.x, -8, 0);
        CreateSlingItem();
        CreateLineRenderer();
        DisplayFinger();
    }

    void Update()
    {
        if (slingItem == null) CreateSlingItem();
        ControlAngles();
        DestroyOutOfBounds();
    }

    private void ControlAngles()
    {
        if (LivesSystem.lives <= 0 && controlObject != null) {
            controlObject.SetActive(false);
            return;
        }

        Vector2 currentVelocity = slingItem.GetComponent<Rigidbody2D>().velocity;
        if (slingItem == null || currentVelocity != Vector2.zero) { 
            // funny rotations
            slingItem.transform.Rotate(0f, 0f, 620f * Time.deltaTime, Space.Self);

            // TODO: redo this; make it so that elastic string becomes visible and lineRenderer
            // is disabled when slingItem collides with the elastic string, instead of 
            // being instantly disabled and spawned when slinging. Also, need to limit
            // stretch distance somehow to prevent wonky rendering (most likely 
            // requires an additional script attached to the elastic string. this script
            // will let it collide until some stretch check returns true, and turns off 
            // collision with the slingItem)
            if (lineRenderer.enabled) {
                lineRenderer.enabled = false;
                SpawnElasticString();
            }
            return;
        }
        if (!lineRenderer.enabled) {
            lineRenderer.enabled = true;
            DeleteElasticString();
        }
        DrawSlingLines();
        slingItem.transform.position = SetControlPosition();
    }

    private void Sling()
    {
        var itemPhysics = slingItem.GetComponent<Rigidbody2D>();
        const int strength = 24;

        // calculate and add sling velocity, if there is none
        if (itemPhysics.velocity == Vector2.zero)
        {
            // sling physics 
            Vector3 force = centeredPos - slingItem.transform.position; // direction
            force.Normalize(); // unit vector conversion (for constant speed)
            force *= strength; // strength/speed scalar
            // assigning force
            itemPhysics.velocity = force;
            // toggle collision physics
            slingItem.GetComponent<Collider2D>().enabled = true;

            // someone used the slingshot; make the slingshot active if it isn't
            if (gameObject.layer != activeLayer && force != Vector3.zero) {
                DisableIdleChecker();
                gameObject.layer = activeLayer;
                DeleteFinger();
            }
        }
    }

    private void DestroyOutOfBounds()
    {
        // convert game object transform coords to screen coords
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(slingItem.transform.position);
        if (screenPosition.x < 0 || screenPosition.x > Screen.width ||
            screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            UnregisterGestures();
            Destroy(controlObject);
            Destroy(slingItem);
        }
    }

    private void CreateSlingItem()
    {
        CreateControlObject();
        // set slingItem to be cloned
        slingItem = Instantiate(clonedSlingItem.gameObject);
        // let the Slingshot GameObject be the parent of the slingItem
        slingItem.transform.SetParent(transform);
        slingItem.tag = "SlingItem";
        // set slingItem position
        slingItem.transform.position = SetControlPosition();
        // disable sling item collisions. enable when the SlingItem is in motion
        slingItem.GetComponent<Collider2D>().enabled = false;
    }

    private void CreateControlObject()
    {
        var panelSize = new Texture2D(Screen.width / 3, Screen.height); // 1/3 of the screen width; size of one panel
        controlObject = new GameObject
        {
            name = "ControlObject"
        };

        // add components to this control object (order matters)
        Image image = controlObject.AddComponent<Image>();
        TransformGesture transformGesture = controlObject.AddComponent<TransformGesture>();
        controlObject.AddComponent<Transformer>();
        controlObject.AddComponent<PressGesture>();
        controlObject.AddComponent<ReleaseGesture>();
        RegisterGestures();

        // set transform types to only Translation
        transformGesture.Type = TransformGesture.TransformType.Translation;

        // transform image to proper distinct position (by name) 
        image.rectTransform.anchorMax = Vector2.zero;
        image.rectTransform.anchorMin = Vector2.zero;
        image.rectTransform.pivot = Vector2.zero;
        image.rectTransform.anchoredPosition = new Vector2(GetBoundsBySlingshotName(gameObject.name), 0);
        controlStartPos = image.transform.position;

        // set the size of the image
        image.rectTransform.sizeDelta = new Vector2(panelSize.width, panelSize.height);
        HideImage(image);

        // set the Canvas GameObject as parent
        controlObject.transform.SetParent(GameObject.Find("Canvas").transform);
        image.transform.localScale = Vector2.one;
    }

    private void CreateLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 3;      // total count of lines
        lineRenderer.numCornerVertices = 10; // rounded edges. more string-like appearance.
        lineRenderer.numCapVertices = 10;    // rounded edges. more string-like appearance.
        var col = new Color(1, 1, 1, 1);
        lineRenderer.startColor = col;
        lineRenderer.endColor = col;
    }

    private void DrawSlingLines()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        float dist = Vector3.Distance(slingItem.transform.position, centeredPos);
        // Vector3 positions are subject to change upon new Slingshot asset design
        lineRenderer.SetPosition(0, new Vector3(centeredPos.x - 0.95f, -8.1f, 0.0f));
        lineRenderer.SetPosition(1, (dist <= diameter + 0.1f) ? slingItem.transform.position : centeredPos);
        lineRenderer.SetPosition(2, new Vector3(centeredPos.x + 0.95f, -8.1f, 0.0f));
    }

    private void SpawnElasticString() {
        elasticObj = Instantiate(clonedElastic.gameObject);
        elasticObj.transform.SetParent(transform);
        elasticObj.transform.position = new Vector3(centeredPos.x - 0.95f, -8.1f, 0.0f);
    }

    private void DeleteElasticString() {
        Destroy(elasticObj);
    }

    private void DisplayFinger() {
        fingerObj = Instantiate(clonedFingerObj.gameObject);
        fingerObj.transform.SetParent(transform);
    }

    private void DeleteFinger() {
        Destroy(fingerObj);
    }

    private Vector3 SetControlPosition()
    {
        // angle change speed
        const float controlSpeed = 2.6f;
        // get control object pos and pass them to the sling item
        Vector3 controlPos = new Vector3((controlObject.transform.position.x - controlStartPos.x) * controlSpeed,
                                         controlObject.transform.position.y * controlSpeed,
                                         controlObject.transform.position.z * controlSpeed);
        // limit sling item translation bounds to a circle with a 3.75f diameter
        controlPos = centeredPos + Vector3.ClampMagnitude(controlPos, diameter);
        return controlPos;
    }

    private float GetBoundsBySlingshotName(String name)
    {
        switch (name)
        {
            case "Slingshot Left":
                return 0.0f;
            case "Slingshot Right":
                return 2 * Screen.width / 3; // 2160.0f
        }
        return Screen.width / 3; // middle = 1080.0f
    }

    private void HideImage(Image image)
    {
        // this function hides an image from view, by setting its opacity to 0
        var col = image.color;
        // if (gameObject.name == "Slingshot Left") {
        //     col.a = 0.33f;
        // } else if (gameObject.name == "Slingshot Right") {
        //     col.a = 0.6f;
        // } else {
        //     col.a = 0.2f;
        // }
        col.a = 0f;
        image.color = col;
    }

    private void DisableIdleChecker() {
        // set global active check to true if it isn't
        if (!IdleChecker.slingshotActive)
            IdleChecker.slingshotActive = true;
    }

    private void CancelGestures()
    {
        // NOTE: for future implementation, may not be needed at all
        //       - there is a chance that gestures will be cancelled for us if
        //       - dragging finger over the little gap between panels stops taking input
        // If the above statement is not true, here is a template to cancel gestures.
        // Logic is still needed to determine when gestures should be cancelled (finger not in panel bounds).

        // cancel press, release and transform gestures when 
        // player's touch/finger is out of panel
        var transformGesture = controlObject.GetComponent<TransformGesture>();
        var pressGesture = GetComponent<PressGesture>();

        if (transformGesture != null) transformGesture.Cancel();
        if (pressGesture != null) pressGesture.Cancel();
    }

    private void RegisterGestures()
    {
        controlObject.GetComponent<PressGesture>().Pressed += pressedHandler;
        controlObject.GetComponent<ReleaseGesture>().Released += releasedHandler;
    }

    private void UnregisterGestures()
    {
        controlObject.GetComponent<PressGesture>().Pressed -= pressedHandler;
        controlObject.GetComponent<ReleaseGesture>().Released -= releasedHandler;
    }

    private void pressedHandler(object sender, System.EventArgs e)
    {
        SoundManager.soundManager.PlayEffect("ElasticPulling");
    }

    private void releasedHandler(object sender, System.EventArgs e)
    {
        SoundManager.soundManager.pullingEffectsSource.Stop();
        SoundManager.soundManager.PlayEffect("ElasticFiring");
        Sling();
    }
}

/* FOR DEV:
// Coords to center the Slingshots accurately in the center of each panel
//  - Assuming each panel is 1080p (3240 / 3 = 1080), then
//    1080 / 2 = 540 where the slingshot should be placed, right in the center of that panel
//  - This is to convert pixel coordinates to game/world coordinates.
// Debug.Log(Camera.main.WorldToScreenPoint(slingItem.transform.position));
// Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(540, 0, 0)));   // x:-14.63
// Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(1620, 0, 0)));  // x:0
// Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(2700, 0, 0)));  // x:14.63
*/
