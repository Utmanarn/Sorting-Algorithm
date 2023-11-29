using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO: Make the sorting run a few times with different ball positions then return the average. After that we add x amount of balls to the scene and repeat the test until it reaches a critical point (takes too long) where we stop the algorithm.

public class ExperimentEnvironmentSetup : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] public int ballSpawnAmount = 100;
    private int _ballSpawnCurrent = 0;
    private GameObject _theBaller;

    public static List<Ball> Balls;

    private void Start()
    {
        _theBaller = GameObject.Find("TheBaller");
        Balls = new List<Ball>();
    }

    private void FixedUpdate()
    {
        while (_ballSpawnCurrent < ballSpawnAmount)
        {
            SpawnBall();
            _ballSpawnCurrent++;
        }
    }

    // Some balls spawn on top of others.
    private void SpawnBall()
    {
        float x, y, yPos, xPos;

        do
        {
            x = Random.Range(-7f, 7f);
        } while (x is < 1f and > -1f);

        do
        {
            y = Random.Range(-7f, 7f);
        } while (y is < 1f and > -1f);

        yPos = Random.Range(-4f, 4f);
        xPos = Random.Range(-4f, 4f);
        
        var localBall = Instantiate(ball, new Vector3(transform.position.x + xPos, transform.position.y + yPos, 0), Quaternion.identity).GetComponent<Ball>();
        Rigidbody2D ballRb = localBall.GetComponent<Rigidbody2D>();

        ballRb.velocity = new Vector2(x, y);

        localBall.theBaller = _theBaller; // Assign the baller to the ball so it knows what to compare distance to.
        localBall.spriteRenderer = localBall.GetComponent<SpriteRenderer>();

        Balls.Add(localBall);
    }

    public void ResetEnvironment()
    {
        foreach (var ball in Balls)
        {
            Destroy(ball.gameObject);
        }
        Balls.Clear(); // Test, might not delete the already spawned balls?
        _ballSpawnCurrent = 0;
    }

    public void FullEnvironmentReset()
    {
        foreach (var ball in Balls)
        {
            Destroy(ball.gameObject);
        }
        Balls.Clear(); // Test, might not delete the already spawned balls?
        _ballSpawnCurrent = 0;
        ballSpawnAmount = 100; // Probably should be changed to match the starting input.
    }
}
