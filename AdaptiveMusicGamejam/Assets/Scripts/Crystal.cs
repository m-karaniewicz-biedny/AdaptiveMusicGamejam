using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
public class Crystal : Interactable
{
    [SerializeField] ParticleSystem DestroyParticlePrefab;
    
    public override void Interact()
    {
        Activate();
    }
    
    private void Activate()
    {
        AkSoundEngine.PostEvent("CrystalExplosionSFX",gameObject);
        GameManager.Instance.AwardCrystal(1);
        Instantiate(DestroyParticlePrefab.gameObject,transform.position,Quaternion.identity);
        gameObject.SetActive(false);
    }

}
