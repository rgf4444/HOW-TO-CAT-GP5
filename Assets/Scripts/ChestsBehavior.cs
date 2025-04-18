using UnityEngine;

public class ChestsBehavior : MonoBehaviour
{
    [SerializeField] GameObject Key, lockedSymbol, unlockedSymbol, letter;
    //private Animator Animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Key != null)
            {
                if(lockedSymbol != null)
                {
                    lockedSymbol.SetActive(true);
                }
            } else
            {
                if(unlockedSymbol != null)
                {
                    unlockedSymbol.SetActive(true);
                }
                if(letter != null)
                {
                    letter.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Key != null)
            {
                if (lockedSymbol != null)
                {
                    lockedSymbol.SetActive(false);
                }
            }
            else
            {
                if (unlockedSymbol != null)
                {
                    unlockedSymbol.SetActive(false);
                }
            }
        }
    }
}
