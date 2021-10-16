using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] TowerController tower;
    private bool towerLowered;
    internal int collectedCrystalCount;
    public const int CRYSTALS_NEEDED_TO_LOWER_THE_TOWER = 1;
    public const int CRYSTALS_NEEDED_FOR_TRUE_ENDING = 16;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        collectedCrystalCount = 0;

        towerLowered = false;
    }

    private void Start()
    {
        UIController.Instance.InfoMessage("Activate 12 crystals to access the Tower.", 4);
    }

    public void AwardCrystal(int count)
    {
        collectedCrystalCount += count;
        if (collectedCrystalCount >= CRYSTALS_NEEDED_TO_LOWER_THE_TOWER && !towerLowered)
        {
            tower.LowerTower();
            towerLowered = true;
        }

    }

}
