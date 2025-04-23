using UnityEngine;

public class Lever : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject objectToEnable;
    public bool isLeverOn = false;
    public bool IsLeverOn => isLeverOn;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(isLeverOn);
        }
    }

    public void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        isLeverOn = !isLeverOn;

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(isLeverOn);
        }
    }
}
