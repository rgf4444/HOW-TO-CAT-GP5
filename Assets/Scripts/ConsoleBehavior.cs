using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsoleBehavior : MonoBehaviour
{
    [SerializeField] private GameObject consoleUI;
    [SerializeField] private List<GameObject> requiredObjects;
    [SerializeField] private GameObject warningText;
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private PlayerMovements playerMovements;

    private bool playerNearby = false;
    private bool isLocked = false; 

    private void Start()
    {
        warningText.SetActive(false);
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isLocked && !playerMovements.IsMeowing)
        {
            if (AllObjectsDestroyed())
            {
                ToggleConsole();
            }
            else
            {
                StartCoroutine(DenyAccess());
            }
        }
    }

    private bool AllObjectsDestroyed()
    {
        foreach (GameObject obj in requiredObjects)
        {
            if (obj != null)
                return false;
        }
        return true;
    }

    public void ToggleConsole()
    {
        bool isActive = consoleUI.activeSelf;
        consoleUI.SetActive(!isActive);
        playerMovementScript.enabled = isActive;

        // If we just opened the console, meow
        if (!isActive && playerMovements != null)
        {
            playerMovements.Meow();
        }
    }



    private Coroutine denyAccessRoutine;

    private IEnumerator DenyAccess()
    {
        if (denyAccessRoutine != null)
            yield break;

        denyAccessRoutine = StartCoroutine(DenyAccessCoroutine());
    }

    private IEnumerator DenyAccessCoroutine()
    {
        isLocked = true;
        warningText.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningText.SetActive(false);
        yield return new WaitForSeconds(1f);
        isLocked = false;
        denyAccessRoutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
