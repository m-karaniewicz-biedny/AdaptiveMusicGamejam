using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsController : MonoBehaviour
{
    CharacterController charCon;
    PlayerPositionController playerPos;

    [SerializeField] float footstepDistanceInterval = 3f;
    private float footstepDistanceCounter = 0;

    private Vector3 positionLastFrame;

    private void Awake()
    {
        positionLastFrame = transform.position;
        if (charCon == null) charCon = GetComponent<CharacterController>();
        if (playerPos == null) playerPos = GetComponent<PlayerPositionController>();
    }

    private void Update()
    {
        //bool groundBelow = Physics.SphereCast(transform.position, charCon.radius, Vector3.down, out RaycastHit hit, charCon.bounds.extents.y);

        if (charCon.isGrounded)
        {
            footstepDistanceCounter += (transform.position - positionLastFrame).magnitude;

            if (footstepDistanceCounter > footstepDistanceInterval)
            {
                while (footstepDistanceCounter > footstepDistanceInterval) footstepDistanceCounter -= footstepDistanceInterval;

                PlayRegionAwareFootstep();
                //if (hit.collider.tag == terrainTag) PlayTerrainAwareFootstep();
                //else PlayDefaultFootstep();
            }
        }

        positionLastFrame = transform.position;
    }

    private void PlayRegionAwareFootstep()
    {
        switch (playerPos.currentRegion)
        {
            case Region.plains:
                {
                    AkSoundEngine.PostEvent("MeadowSteps", gameObject);
                    break;
                }
            case Region.desert:
                {
                    AkSoundEngine.PostEvent("DesertSteps", gameObject);
                    break;
                }
            case Region.forest:
                {
                    AkSoundEngine.PostEvent("ForestSteps", gameObject);
                    break;
                }
            case Region.mountains:
                {
                    AkSoundEngine.PostEvent("SnowSteps", gameObject);
                    break;
                }
        }
    }

    private void PlayDefaultFootstep()
    {
        AkSoundEngine.PostEvent("MeadowSteps", gameObject);
    }
}
