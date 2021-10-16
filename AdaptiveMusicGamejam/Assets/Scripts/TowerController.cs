using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    float loweringDuration;
    Vector3 towerPositionDefault;
    Vector3 towerPositionLowered;

    private void Awake()
    {
        loweringDuration = 3f;
        towerPositionDefault = transform.position;
        towerPositionLowered = new Vector3(
            transform.position.x,
            65.7f,
            transform.position.z);
    }

    public void LowerTower(bool reverseLowering = false)
    {
        if (!reverseLowering) StartCoroutine(ChangePositionSmooth(
            transform,
            towerPositionDefault,
            towerPositionLowered, loweringDuration
            ));
        else StartCoroutine(ChangePositionSmooth(
            transform,
            towerPositionLowered,
            towerPositionDefault, loweringDuration
            ));
    }

    private static IEnumerator ChangePositionSmooth(Transform tr, Vector3 startPos, Vector3 endPos, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            tr.position = startPos.SmoothStepTo(endPos, timer / duration);
            
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        tr.position = endPos;
    }

}
