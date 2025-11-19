using UnityEngine;
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
        // 初期位置の設定（左下を0,0とする）
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
    }
    void Update()
    {
        // W キー - 上に移動
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(0, -1);
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
    }
    // 指定方向への移動を試みる
    void TryMove(int deltaX, int deltaY)
    {
        int newX = currentX + deltaX;
        int newY = currentY + deltaY;
        // グリッドの範囲内かチェック
        if (newX < 0 || newX >= GRID_WIDTH || newY < 0 || newY >= GRID_HEIGHT)
        {
            Debug.Log("グリッドの範囲外です");
            return;
        }
        // 移動先のマップが空いているかチェック
        if (mapOccupied[newX, newY])
        {
            Debug.Log("マップが占有されています");
            return;
        }
        // 移動を実行
        currentX = newX;
        currentY = newY;
        UpdatePlayerPosition();
        Debug.Log($"移動しました: ({currentX}, {currentY})");
    }
    // プレイヤーの実際の位置を更新（2D版）
    void UpdatePlayerPosition()
    {
        // グリッド位置から実際のワールド座標を計算
        // 左上が(0,0)、右下が(5,3)のグリッド
        Vector3 newPosition = new Vector3(
            START_X + currentX * MAP_SIZE,
            START_Y - currentY * MAP_SIZE, // Yは上から下に減少
            transform.position.z  // Z座標は維持
        );
        transform.position = newPosition;
    }
    // マップの占有状態を設定するメソッド（外部から呼び出し可能）
    public void SetMapOccupied(int x, int y, bool occupied)
    {
        if (x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT)
        {
            mapOccupied[x, y] = occupied;
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
}