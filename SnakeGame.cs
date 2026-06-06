using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnakeGame : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform gameArea;
    public RectTransform snakePartPrefab;
    public RectTransform food;

    public TMP_Text scoreText;

    [Header("Grid Settings")]
    public int gridWidth = 20;
    public int gridHeight = 20;
    public float cellSize = 20f;

    [Header("Game Settings")]
    public float moveRate = 0.2f;

    [SerializeField] private float timer;
    private Vector2Int direction = Vector2Int.right;
    private Vector2Int foodPos;

    private List<RectTransform> snakeParts = new List<RectTransform>();
    private List<Vector2Int> snakePositions = new List<Vector2Int>();

    private int score = 0;
    private bool isGameOver = false;

    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        if (isGameOver) return;

        HandleInput();

        timer += Time.unscaledDeltaTime; // chạy dù Time.timeScale = 0

        Debug.Log(Time.unscaledDeltaTime);

        if (timer >= moveRate)
        {
            timer = 0;
            MoveSnake();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2Int.down)
            direction = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) && direction != Vector2Int.up)
            direction = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) && direction != Vector2Int.right)
            direction = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) && direction != Vector2Int.left)
            direction = Vector2Int.right;
    }

    void MoveSnake()
    {
        Vector2Int newHeadPos = snakePositions[0] + direction;

        // check wall collision
        if (newHeadPos.x < 0 || newHeadPos.x >= gridWidth || newHeadPos.y < 0 || newHeadPos.y >= gridHeight)
        {
            GameOver();
            return;
        }

        // check self collision
        if (snakePositions.Contains(newHeadPos))
        {
            GameOver();
            return;
        }

        // move body
        snakePositions.Insert(0, newHeadPos);

        if (newHeadPos == foodPos)
        {
            score++;
            scoreText.text = "Score: " + score;
            SpawnFood();
            CreateNewPart();
        }
        else
        {
            snakePositions.RemoveAt(snakePositions.Count - 1);
        }

        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        for (int i = 0; i < snakeParts.Count; i++)
        {
            snakeParts[i].anchoredPosition = GridToUIPos(snakePositions[i]);
        }
    }

    void SpawnFood()
    {
        do
        {
            foodPos = new Vector2Int(Random.Range(0, gridWidth), Random.Range(0, gridHeight));
        }
        while (snakePositions.Contains(foodPos));

        food.anchoredPosition = GridToUIPos(foodPos);
    }

    void CreateNewPart()
    {
        RectTransform part = Instantiate(snakePartPrefab, gameArea);
        snakeParts.Add(part);
    }

    Vector2 GridToUIPos(Vector2Int gridPos)
    {
        float x = gridPos.x * cellSize;
        float y = gridPos.y * cellSize;
        return new Vector2(x, y);
    }

    void ResetGame()
    {
        isGameOver = false;
        score = 0;
        scoreText.text = "Score: 0";

        // destroy old snake parts
        foreach (var part in snakeParts)
        {
            Destroy(part.gameObject);
        }

        snakeParts.Clear();
        snakePositions.Clear();

        direction = Vector2Int.right;

        // create starting snake length 3
        snakePositions.Add(new Vector2Int(5, 10));
        snakePositions.Add(new Vector2Int(4, 10));
        snakePositions.Add(new Vector2Int(3, 10));

        for (int i = 0; i < snakePositions.Count; i++)
        {
            RectTransform part = Instantiate(snakePartPrefab, gameArea);
            snakeParts.Add(part);
        }

        SpawnFood();
        UpdateVisuals();
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER!");
    }

    public void Restart()
    {
        ResetGame();
    }
}