using UnityEngine;

// 動かす障害物

public class MovingObject : MonoBehaviour
{
    // グリッドの設定
    private const int GRID_WIDTH = 6;
    private const int GRID_HEIGHT = 4;
    // 
    private const float MAP_SIZE = 2.0f;
    private const float START_X = -5.0f;
    private const float START_Y = 3.0f;
    //
    private int currentX = 0;
    private int currentY = 0;
    // 
    private bool[,] mapOccupied = new bool[GRID_WIDTH, GRID_HEIGHT];
    void Start()
    {
        // 初期位置の設定
        currentX = 0;
        currentY = 0;
        // 全てのマップをあいてる状態で初期化
        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                mapOccupied[x, y] = false;
            }
        }

        UpdateObjectPosition();
    }
    void Update()
    {
        // 
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMove(0, -1);
        }
        // 
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMove(0, 1);
        }
        // 
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMove(-1, 0);
        }
        // 
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMove(1, 0);
        }
    }
    // 
    void TryMove(int deltaX, int deltaY)
    {
        int newX = currentX + deltaX;
        int newY = currentY + deltaY;
        // 
        if (newX < 0 || newX >= GRID_WIDTH || newY < 0 || newY >= GRID_HEIGHT)
        {
            Debug.Log("aaaa");
            return;
        }
        // 
        if (mapOccupied[newX, newY])
        {
            Debug.Log("aaaaa");
            return;
        }
        //
        currentX = newX;
        currentY = newY;
        UpdateObjectPosition();
        Debug.Log("aaaaa: ({currentX}, {currentY})");
    }
    // 
    void UpdateObjectPosition()
    {
        // 
        // 
        Vector3 newPosition = new Vector3(
            START_X + currentX * MAP_SIZE,
            START_Y - currentY * MAP_SIZE, //
            transform.position.z  // 
        );
        transform.position = newPosition;
    }
    // 
    public void SetMapOccupied(int x, int y, bool occupied)
    {
        if (x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT)
        {
            mapOccupied[x, y] = occupied;
        }
    }
    //
    public bool IsMapEmpty(int x, int y)
    {
        if (x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT)
        {
            return !mapOccupied[x, y];
        }
        return false;
    }
}

