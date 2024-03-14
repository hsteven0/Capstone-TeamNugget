using UnityEngine;
using TMPro;

/*
This class will be used as calculating point multipliers upon contact with enemies.
*/
public class Ball : MonoBehaviour
{
    private int killCount; // a count of enemies defeated with one ball
    private int oldAmount;
    public GameObject pointsText;

    void Start() {
        killCount = 0;
        oldAmount = GameScore.points;
    }

    // gets every object that is destroyed on the scene
    void OnDestroy() {
        // check only for SlingItem tag
        if (!gameObject.CompareTag("SlingItem")) return;
        killCount = 0;
        GameScore.points = oldAmount;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemies")) return;
        UpdatePoints();
        killCount++;
    }

    private void UpdatePoints() {
        // reward player with more points, based on number of enemies defeated with a single ball
        GameScore.points = oldAmount * (int) Mathf.Pow(2, killCount);
        GameScore.score += GameScore.points;
        GameObject point = Instantiate(pointsText, transform.position, Quaternion.identity);
        point.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(GameScore.points.ToString());
    }
}
