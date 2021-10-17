using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
public class Crystal : Interactable
{
    [SerializeField] ParticleSystem DestroyParticlePrefab;
    [SerializeField] GameObject DestructionSoundPrefab;
    public override void Interact()
    {
        Activate();
    }
    
    private void Activate()
    {
        GameManager.Instance.AwardCrystal(1);
        DestroyCrystal();
    }
    
    private void DestroyCrystal()
    {
        GameObject sound = Instantiate(DestructionSoundPrefab,transform.position,Quaternion.identity);
        AkSoundEngine.PostEvent("CrystalExplosionSFX",sound);
        
        Instantiate(DestroyParticlePrefab.gameObject,transform.position,Quaternion.identity);
        
        AkSoundEngine.StopAll(gameObject);
        
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

}
