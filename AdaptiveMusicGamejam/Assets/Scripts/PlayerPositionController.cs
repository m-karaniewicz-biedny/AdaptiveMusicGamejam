using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionController : MonoBehaviour
{
    private enum Region
    {
        plains,
        desert,
        forest,
        mountains,
    }

    Vector3 mapCenter = new Vector3(125, 100, 125);
    float wrapPositionAltitude = -100;

    Region currentRegion;
    Region lastFrameRegion;

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
        Region reg = GetRegionOnPosition(transform.position);
        if (lastFrameRegion != reg)
        {
            currentRegion = reg;

            switch (currentRegion)
            {
                case Region.plains:
                    {
                        Debug.Log("Entering plains.");
                        break;
                    }
                case Region.desert:
                    {
                        Debug.Log("Entering desert.");
                        break;
                    }
                case Region.forest:
                    {
                        Debug.Log("Entering forest.");
                        break;
                    }
                case Region.mountains:
                    {
                        Debug.Log("Entering mountains.");
                        break;
                    }
            }
        }
        
        lastFrameRegion = currentRegion;
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
