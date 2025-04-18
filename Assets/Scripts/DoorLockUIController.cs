using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorLockUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform buttonContainer;
    public TMP_InputField inputField;
    public TMP_Text feedbackText;

    [Header("Door & Password Settings")]
    public string correctPassword;
    public GameObject doorLockPanel;
    public BoxCollider2D doorCollider;

    private void Start()
    {
        feedbackText.gameObject.SetActive(false);
    }

    public void ShuffleButtons()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in buttonContainer)
        {
            children.Add(child);
        }

        for (int i = 0; i < children.Count; i++)
        {
            Transform temp = children[i];
            int randomIndex = Random.Range(i, children.Count);
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    public void AddLetter(string letter)
    {
        inputField.text += letter;
    }

    public void Backspace()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    public void Confirm()
    {
        if (inputField.text == correctPassword)
        {
            Destroy(doorLockPanel);
            doorCollider.enabled = true;

            var playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovements>();
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

        }
        else
        {
            inputField.text = "";
            StartCoroutine(ShowIncorrectMessage());
        }
    }


    private IEnumerator ShowIncorrectMessage()
    {
        feedbackText.gameObject.SetActive(true);
        feedbackText.text = "Incorrect password";
        yield return new WaitForSeconds(3f);
        feedbackText.gameObject.SetActive(false);
    }
}
