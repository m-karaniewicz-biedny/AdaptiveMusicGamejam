using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
public class FinalCrystal : Interactable
{
    [SerializeField] ParticleSystem DestroyParticles;
    [SerializeField] GameObject DestructionSoundPrefab;

    public override void Interact()
    {
        int crystals = GameManager.Instance.collectedCrystalCount;

        if (crystals < GameManager.Instance.crystalsThresholdLowerTheTower) StartBadEnding();
        else if (crystals >= GameManager.Instance.crystalsThresholdTrueEnding) StartTrueEnding();
        else StartRegularEnding();

        gameObject.SetActive(false);
    }

    private void DestroyCrystal()
    {
        GameObject sound = Instantiate(DestructionSoundPrefab,transform.position,Quaternion.identity);
        AkSoundEngine.PostEvent("FinalCrystalExplosion",sound);

        Instantiate(DestroyParticles.gameObject, transform.position, Quaternion.identity);

        AkSoundEngine.StopAll(gameObject);

        //gameObject.SetActive(false);
        Destroy(gameObject);
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
