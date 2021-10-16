using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected string tooltip;

    public string GetTooltip()
    {
        if (tooltip != null) return tooltip;
        else
        {
            Debug.LogError("Tooltip missing");
            return "Tooltip missing";
        }
    }
    public abstract void Interact();
}
