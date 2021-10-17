using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public enum Region
{
    plains,
    desert,
    forest,
    mountains,
}

public class PlayerPositionController : MonoBehaviour
{


    Vector3 mapCenter = new Vector3(125, 100, 125);
    float wrapPositionAltitude = -100;

    internal Region currentRegion;
    Region lastFrameRegion;

    private void Start()
    {
        AkSoundEngine.PostEvent("Music", gameObject);

        SwitchToRegion(GetRegionOnPosition(transform.position));
    }

    void Update()
    {
        if (transform.position.y < wrapPositionAltitude)
        {
            Vector3 moveback = (mapCenter - transform.position) * 2;
            Vector3 target = transform.position + moveback;
            target.y = 500;

            transform.position = target;
        }

        CheckCurrentRegion();

    }

    private void CheckCurrentRegion()
    {
        Region r = GetRegionOnPosition(transform.position);
        if (lastFrameRegion != r)
        {
            SwitchToRegion(r);
        }

        lastFrameRegion = currentRegion;
    }

    private void SwitchToRegion(Region region)
    {
        currentRegion = region;

        switch (currentRegion)
        {
            case Region.plains:
                {
                    AkSoundEngine.PostEvent("StateMeadow", gameObject);
                    Debug.Log("Entering plains.");
                    break;
                }
            case Region.desert:
                {
                    AkSoundEngine.PostEvent("StateDesert", gameObject);
                    Debug.Log("Entering desert.");
                    break;
                }
            case Region.forest:
                {
                    AkSoundEngine.PostEvent("StateForest", gameObject);
                    Debug.Log("Entering forest.");
                    break;
                }
            case Region.mountains:
                {
                    AkSoundEngine.PostEvent("StateWinter", gameObject);
                    Debug.Log("Entering mountains.");
                    break;
                }
        }
    }

    private Region GetRegionOnPosition(Vector3 position)
    {
        if (position.x > mapCenter.x)
        {
            if (position.z > mapCenter.z) return Region.plains;
            else return Region.desert;
        }
        else
        {
            if (position.z > mapCenter.z) return Region.mountains;
            else return Region.forest;
        }
    }
}
