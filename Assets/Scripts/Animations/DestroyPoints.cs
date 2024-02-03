using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPoints : MonoBehaviour
{
    public void DestroyPointsParent() {
        GameObject pointsParent = gameObject.transform.parent.gameObject;
        Destroy(pointsParent);
    }
}
