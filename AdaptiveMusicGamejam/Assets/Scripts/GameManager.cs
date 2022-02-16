using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int crystalsThresholdLowerTheTower = 12;
    public int crystalsThresholdTrueEnding = 16;
    
    [Header("References")]
    [SerializeField] TowerController tower;

    [Header("Game Over Prefab Rain")]
    [SerializeField] GameObject trueEndingRainPrefab;
    [SerializeField] GameObject regularEndingRainPrefab;
    [SerializeField] GameObject badEndingRainPrefab;
    
    internal bool gameOver = false;

    private bool towerLowered;
    internal int collectedCrystalCount;


    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        collectedCrystalCount = 0;

        towerLowered = false;
    }

    private void Start()
    {
        AwardCrystal(0);
        UIController.Instance.InfoMessage($"Activate {crystalsThresholdLowerTheTower} crystals to access the Tower.", 4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void AwardCrystal(int count)
    {
        collectedCrystalCount += count;
        UIController.Instance.UpdateCrystalCount(collectedCrystalCount, crystalsThresholdLowerTheTower);
        if (collectedCrystalCount >= crystalsThresholdLowerTheTower && !towerLowered)
        {
            tower.LowerTower();
            towerLowered = true;
        }

    }

    public void TrueEnding() => StartCoroutine(TrueEndingSequence());
    public void RegularEnding() => StartCoroutine(RegularEndingSequence());
    public void BadEnding() => StartCoroutine(BadEndingSequence());

    private void ResetMusic()
    {
        FindObjectOfType<PlayerRegionLocator>().ForceUpdateRegion();
    }

    public IEnumerator TrueEndingSequence()
    {
        yield return new WaitForSeconds(5);

        SpawnEndingPrefabRain(trueEndingRainPrefab);
        gameOver = true;
        ResetMusic();
        UIController.Instance.InfoMessage("You win!", 3);
        yield return new WaitForSeconds(3);

        UIController.Instance.InfoMessage("True Ending", 3);
        yield return new WaitForSeconds(3);


        yield return new WaitForSeconds(10);
        UIController.Instance.InfoMessage("Escape to exit", 5);
    }

    public IEnumerator RegularEndingSequence()
    {
        yield return new WaitForSeconds(5);

        SpawnEndingPrefabRain(regularEndingRainPrefab);
        gameOver = true;
        ResetMusic();
        UIController.Instance.InfoMessage("You win!", 3);
        yield return new WaitForSeconds(3);

        UIController.Instance.InfoMessage("Normal Ending", 3);
        yield return new WaitForSeconds(3);


        yield return new WaitForSeconds(10);
        UIController.Instance.InfoMessage("Escape to exit", 5);
    }

    public IEnumerator BadEndingSequence()
    {
        yield return new WaitForSeconds(5);

        SpawnEndingPrefabRain(badEndingRainPrefab);
        gameOver = true;
        ResetMusic();
        UIController.Instance.InfoMessage("You win!", 3);
        yield return new WaitForSeconds(3);

        UIController.Instance.InfoMessage("Bad Ending", 3);
        yield return new WaitForSeconds(3);


        yield return new WaitForSeconds(10);
        UIController.Instance.InfoMessage("Escape to exit", 5);
    }



    private void SpawnEndingPrefabRain(GameObject prefab)
    {
        int prefabAmount = 20;
        Vector3[] pos = RandomRadialPositions(new Vector3(125, 300, 125), 20, 100, prefabAmount);

        for (int i = 0; i < prefabAmount; i++)
        {
            Instantiate(prefab, pos[i], Random.rotation);
        }
    }

    public static Vector3[] RandomRadialPositions(Vector3 center, float minDistance, float maxDistance, int pointCount)
    {
        Vector3[] points = new Vector3[pointCount];

        float angleOffset = Random.Range(0, 360);
        for (int i = 0; i < pointCount; i++)
        {
            Vector3 positionOffset = new Vector3(0, 0, Random.Range(minDistance, maxDistance));

            float angle = (i * 360 / pointCount + angleOffset) % 360;

            Vector3 pos = center + Quaternion.AngleAxis(angle, Vector3.up) * positionOffset;

            points[i] = pos;
        }

        return points;
    }

}
