using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    //private const float INFO_MSG_DURATION_DEFAULT = 2f;
    [SerializeField] private TextMeshProUGUI infoMessageTextBox;

    private Coroutine infoMessageCoroutine;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
        infoMessageTextBox.transform.parent.gameObject.SetActive(false);
    }

    public void InfoMessage(string message, float duration)
    {
        if (infoMessageCoroutine != null) StopCoroutine(infoMessageCoroutine);
        infoMessageCoroutine = StartCoroutine(InfoMessageSequence(message, duration));
    }

    private IEnumerator InfoMessageSequence(string message, float duration)
    {
        ShowInfoMessage(message);

        yield return new WaitForSeconds(duration);

        HideInfoMessage();
    }

    private void ShowInfoMessage(string message)
    {
        infoMessageTextBox.transform.parent.gameObject.SetActive(true);
        infoMessageTextBox.text = message;
    }

    private void HideInfoMessage()
    {
        infoMessageTextBox.transform.parent.gameObject.SetActive(false);
    }



}
