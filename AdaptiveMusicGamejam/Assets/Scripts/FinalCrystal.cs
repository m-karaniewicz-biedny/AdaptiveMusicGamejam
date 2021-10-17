using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
public class FinalCrystal : Interactable
{
    [SerializeField] ParticleSystem DestroyParticles;

    public override void Interact()
    {
        int crystals = GameManager.Instance.collectedCrystalCount;

        if (crystals < GameManager.CRYSTALS_NEEDED_TO_LOWER_THE_TOWER) StartBadEnding();
        else if (crystals >= GameManager.CRYSTALS_NEEDED_FOR_TRUE_ENDING) StartTrueEnding();
        else StartRegularEnding();

        gameObject.SetActive(false);
    }

    private void DestroyCrystal()
    {
        AkSoundEngine.PostEvent("FinalCrystalExplosion", gameObject);
        Instantiate(DestroyParticles.gameObject, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void StartTrueEnding()
    {
        GameManager.Instance.TrueEnding();
        DestroyCrystal();
    }

    private void StartRegularEnding()
    {
        GameManager.Instance.RegularEnding();
        DestroyCrystal();
    }

    private void StartBadEnding()
    {
        GameManager.Instance.BadEnding();
        DestroyCrystal();
    }

}
