using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] GameObject stairs;
    float loweringDuration;
    Vector3 towerPositionDefault;
    Vector3 towerPositionLowered;
    Vector3 stairsPositionDefault;
    Vector3 stairsPositionLowered;

    private void Awake()
    {
        loweringDuration = 3f;

        stairs.gameObject.SetActive(false);

        stairsPositionDefault = new Vector3(stairs.transform.localPosition.x, stairs.transform.localPosition.y + 100, stairs.transform.localPosition.z);
        stairsPositionLowered = stairs.transform.localPosition;

        towerPositionDefault = transform.position;
        towerPositionLowered = new Vector3(
            transform.position.x,
            65.7f,
            transform.position.z);
            
    }

    public void LowerTower()
    {
        StartCoroutine(ChangePositionSmooth(transform, towerPositionDefault, towerPositionLowered, loweringDuration));

        stairs.gameObject.SetActive(true);

        StartCoroutine(ChangePositionSmooth(stairs.transform, stairsPositionDefault, stairsPositionLowered, loweringDuration, true));
    }

    private static IEnumerator ChangePositionSmooth(Transform tr, Vector3 startPos, Vector3 endPos, float duration, bool local = false)
    {
        float timer = 0;
        while (timer < duration)
        {
            if (local) tr.localPosition = startPos.SmoothStepTo(endPos, timer / duration);
            else tr.position = startPos.SmoothStepTo(endPos, timer / duration);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (local) tr.localPosition = endPos;
        else tr.position = endPos;
    }

}
