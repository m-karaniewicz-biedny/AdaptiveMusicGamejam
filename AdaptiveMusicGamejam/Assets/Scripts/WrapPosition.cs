using System.Collections;
using UnityEngine;

public class WrapPosition : MonoBehaviour
{
    float wrapPositionAltitude = 500;

    void LateUpdate()
    {
        if (transform.position.y < -wrapPositionAltitude)
        {
            Debug.Log("wrapping");
            Vector3 target = GameManager.RandomRadialPositions(
                new Vector3(125f, wrapPositionAltitude, 125f), 0, 100, 1)[0];

            transform.position = target;
        }
    }
}