using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCrystal : Interactable
{
    public override void Interact()
    {
        int crystals = GameManager.Instance.collectedCrystalCount;

        if (crystals < GameManager.CRYSTALS_NEEDED_TO_LOWER_THE_TOWER) BadEnding();
        else if (crystals >= GameManager.CRYSTALS_NEEDED_FOR_TRUE_ENDING) TrueEnding();
        else RegularEnding();

        gameObject.SetActive(false);
    }

    public void RegularEnding()
    {
        UIController.Instance.InfoMessage("You win! (Regular ending)", 5);
    }

    public void TrueEnding()
    {
        UIController.Instance.InfoMessage("You win! (True ending)", 5);
    }

    public void BadEnding()
    {
        UIController.Instance.InfoMessage("You win! (Bad ending)", 5);
    }
}
