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
    tower,
}

public class PlayerPositionController : MonoBehaviour
{


    Vector3 mapCenter = new Vector3(125, 100, 125);
    float wrapPositionAltitude = 500;

    internal Region currentRegion;
    Region lastFrameRegion;

    private void Start()
    {
        AkSoundEngine.PostEvent("Music", gameObject);

        ForceUpdateRegion();
    }

    void Update()
    {
        if (transform.position.y < -wrapPositionAltitude)
        {
            Vector3 target = GameManager.RandomRadialPositions(
                new Vector3(125f, wrapPositionAltitude, 125f), 0, 100, 1)[0];

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
    
    public void ForceUpdateRegion()
    {
        SwitchToRegion(GetRegionOnPosition(transform.position));
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
            case Region.tower:
                {
                    if(!GameManager.Instance.gameOver)
                    {
                        AkSoundEngine.PostEvent("StateNone", gameObject);
                    }
                    else
                    {
                        AkSoundEngine.PostEvent("StateFinal", gameObject);
                    }
                    
                    Debug.Log("Entering unspecified region.");
                    break;
                }
        }
    }

    private Region GetRegionOnPosition(Vector3 position)
    {
        float towerRadius = 10f;
        if (Vector3.Distance(new Vector3(mapCenter.x, position.y, mapCenter.z), position) < towerRadius)
        {
            return Region.tower;
        }

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
