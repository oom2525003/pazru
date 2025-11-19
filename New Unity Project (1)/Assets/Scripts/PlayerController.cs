using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // グリッドの設定
    private const int GRID_WIDTH = 6;
    private const int GRID_HEIGHT = 4;

    // マップのサイズと開始位置
    private const float MAP_SIZE = 2.0f;
    private const float START_X = -5.0f;
    private const float START_Y = 3.0f;

    // 現在のグリッド位置
    private int currentX = 0;
    private int currentY = 0;

    // マップの占有状態を管理する配列（true = 占有されている、false = 空いている）
    private bool[,] mapOccupied = new bool[GRID_WIDTH, GRID_HEIGHT];

    void Start()
    {
        // 初期位置の設定（左上を0,0とする）
        currentX = 0;
        currentY = 0;

        // すべてのマップを空いている状態で初期化
        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                mapOccupied[x, y] = false;
            }
        }

        // プレイヤーの初期位置を設定
        UpdatePlayerPosition();

        Debug.Log($"Player初期化: グリッド({currentX}, {currentY}), ワールド座標{transform.position}");
    }

    void Update()
    {
        // W キー - 上に移動
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(0, -1); // Yは上に行くほど減少
        }
        // S キー - 下に移動
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TryMove(0, 1);
        }
        // A キー - 左に移動
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(-1, 0);
        }
        // D キー - 右に移動
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    // 指定方向への移動を試みる
    void TryMove(int deltaX, int deltaY)
    {
        int newX = currentX + deltaX;
        int newY = currentY + deltaY;

        // グリッドの範囲内かチェック
        if (newX < 0 || newX >= GRID_WIDTH || newY < 0 || newY >= GRID_HEIGHT)
        {
            Debug.Log("Playerはグリッドの範囲外に移動できません");
            return;
        }

        // 移動先の座標を計算
        Vector3 targetPosition = new Vector3(
            START_X + newX * MAP_SIZE,
            START_Y - newY * MAP_SIZE,
            transform.position.z
        );

        // 移動先のマップが空いているかチェック
        if (mapOccupied[newX, newY])
        {
            Debug.Log("Playerはマップが占有されているため移動できません");
            return;
        }

        // 移動先にMovingObjectがあるかチェック
        if (IsMovingObjectAtPosition(targetPosition))
        {
            Debug.Log("PlayerはMovingObjectがあるため移動できません");
            return;
        }

        // 移動を実行
        currentX = newX;
        currentY = newY;
        UpdatePlayerPosition();

        Debug.Log($"Playerが移動しました: グリッド({currentX}, {currentY})");
    }

    // プレイヤーの実際の位置を更新
    void UpdatePlayerPosition()
    {
        Vector3 newPosition = new Vector3(
            START_X + currentX * MAP_SIZE,
            START_Y - currentY * MAP_SIZE,
            transform.position.z
        );

        transform.position = newPosition;
    }

    // 指定位置にMovingObjectがあるかチェック
    bool IsMovingObjectAtPosition(Vector3 position)
    {
        MovingObject[] movingObjects = FindObjectsOfType<MovingObject>();

        foreach (MovingObject obj in movingObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, position);

            if (distance < MAP_SIZE)
            {
                Debug.Log($"移動先にMovingObject '{obj.name}' がいます (距離: {distance:F2})");
                return true;
            }
        }

        return false;
    }

    // マップの占有状態を設定するメソッド（外部から呼び出し可能）
    public void SetMapOccupied(int x, int y, bool occupied)
    {
        if (x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT)
        {
            mapOccupied[x, y] = occupied;
            Debug.Log($"マップ({x}, {y})の占有状態を{occupied}に設定");
        }
    }

    // 特定のマップが空いているか確認するメソッド
    public bool IsMapEmpty(int x, int y)
    {
        if (x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT)
        {
            return !mapOccupied[x, y];
        }
        return false;
    }

    // 現在のグリッド位置を取得
    public Vector2Int GetCurrentGridPosition()
    {
        return new Vector2Int(currentX, currentY);
    }
}