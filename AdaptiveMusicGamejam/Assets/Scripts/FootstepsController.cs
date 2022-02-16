using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerRegionLocator))]
[RequireComponent(typeof(CharacterController))]
public class FootstepsController : MonoBehaviour
{
    PlayerController con;
    PlayerRegionLocator playerLoc;

    [SerializeField] float footstepDistanceInterval = 3f;
    private float footstepDistanceCounter = 0;

    private Vector3 positionLastFrame;

    private void Awake()
    {
        positionLastFrame = transform.position;
        if (con == null) con = GetComponent<PlayerController>();
        if (playerLoc == null) playerLoc = GetComponent<PlayerRegionLocator>();
    }

    private void Update()
    {
        if (con.IsGrounded)
        {
            footstepDistanceCounter += (transform.position - positionLastFrame).magnitude;

            if (footstepDistanceCounter > footstepDistanceInterval)
            {
                while (footstepDistanceCounter > footstepDistanceInterval) footstepDistanceCounter -= footstepDistanceInterval;

                PlayRegionAwareFootstep();
            }
        }

        positionLastFrame = transform.position;
    }

    private void PlayRegionAwareFootstep()
    {
        switch (playerLoc.currentRegion)
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
            case Region.tower:
                {
                    AkSoundEngine.PostEvent("TowerSteps", gameObject);
                    break;
                }
        }
    }
}
