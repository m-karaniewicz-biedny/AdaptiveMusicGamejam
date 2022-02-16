using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StayOnTerrain : MonoBehaviour
{
    [SerializeField] float offsetY = 0;

    private void OnValidate() => UpdatePosition();

    private void OnEnable() => UpdatePosition();

    private void UpdatePosition()
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain == null) return;
        transform.position = new Vector3(
            transform.position.x,terrain.
            SampleHeight(transform.position)+offsetY,
            transform.position.z
            );
    }
}
