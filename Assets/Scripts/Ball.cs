using UnityEngine;

public class Ball : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float distanceToBaller;
    public GameObject theBaller; // Assigned on creation! (In ExperimentEnvironmentSetup)

    private void Update()
    {
        distanceToBaller = Vector2.Distance(transform.position, theBaller.transform.position);
    }

    public void SetBallToRed()
    {
        if (spriteRenderer)
            spriteRenderer.color = Color.red;
    }

    public void SetBallToWhite()
    {
        if (spriteRenderer)
            spriteRenderer.color = Color.white;
    }
}
