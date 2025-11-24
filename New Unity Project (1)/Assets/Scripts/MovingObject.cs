using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // マップのサイズと範囲設定
    private const float MAP_SIZE = 2.0f;
    private const float START_X = -5.0f;
    private const float START_Y = 3.0f;
    private const float END_X = 5.0f;
    private const float END_Y = -3.0f;

    // 選択されているかどうか
    private bool isSelected = false;

    // 既に移動したかどうか
    private bool hasMoved = false;

    // 視覚効果用
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // SpriteRendererを取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Collider2Dの確認
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError($"{gameObject.name}にCollider2Dがありません！BoxCollider2Dを追加してください。");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Collider2D検出 OK (enabled: {collider.enabled}, isTrigger: {collider.isTrigger})");

            if (collider.isTrigger)
            {
                Debug.LogWarning($"{gameObject.name}のCollider2DのisTriggerがONです。OFFにしてください！");
            }
        }

        // Z座標を手前に調整
        Vector3 pos = transform.position;
        if (pos.z >= 0)
        {
            transform.position = new Vector3(pos.x, pos.y, -1);
            Debug.Log($"{gameObject.name}のZ座標を-1に調整しました");
        }

        Debug.Log($"{gameObject.name}初期化完了: 位置{transform.position}");
    }

    void Update()
    {
        // 選択されていて、かつ未移動の場合のみ十字キーで移動
        if (isSelected && !hasMoved)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                TryMove(0, MAP_SIZE);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                TryMove(0, -MAP_SIZE);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                TryMove(-MAP_SIZE, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                TryMove(MAP_SIZE, 0);
            }
        }
    }

    // オブジェクトをクリックしたときの処理
    void OnMouseDown()
    {
        Debug.Log($"{gameObject.name}がクリックされました (移動済み: {hasMoved})");

        // 既に移動済みの場合は選択できない
        if (hasMoved)
        {
            Debug.Log($"{gameObject.name}は既に移動済みのため選択できません");
            return;
        }

        // 他のすべてのMovingObjectの選択を解除
        MovingObject[] allMovingObjects = FindObjectsOfType<MovingObject>();
        foreach (MovingObject obj in allMovingObjects)
        {
            if (obj != this)
            {
                obj.Deselect();
            }
        }

        // このオブジェクトを選択
        Select();
    }

    // オブジェクトを選択状態にする
    void Select()
    {
        isSelected = true;

        // 選択状態を視覚的に表示（黄色に）
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
        }

        Debug.Log($"{gameObject.name}を選択しました（移動可能回数: 残り1回）");
    }

    // 選択を解除する
    void Deselect()
    {
        isSelected = false;

        // 移動済みなら灰色、未移動なら元の色に戻す
        if (spriteRenderer != null)
        {
            if (hasMoved)
            {
                spriteRenderer.color = Color.gray;
            }
            else
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    // 指定方向に移動を試みる
    void TryMove(float deltaX, float deltaY)
    {
        Vector3 newPosition = transform.position + new Vector3(deltaX, deltaY, 0);

        Debug.Log($"{gameObject.name}の移動を試みています: {transform.position} → {newPosition}");

        // 1. マップの範囲外に出ないかチェック
        if (newPosition.x < START_X || newPosition.x > END_X ||
            newPosition.y > START_Y || newPosition.y < END_Y)
        {
            Debug.Log($"{gameObject.name}はマップの範囲外に移動できません");
            return;
        }

        // 2. 移動先にmapオブジェクトがあるかチェック
        if (!IsMapAtPosition(newPosition))
        {
            Debug.Log($"{gameObject.name}は移動先にmapがないため移動できません");
            return;
        }

        // 3. 移動先にPlayerがいないかチェック
        if (IsPlayerAtPosition(newPosition))
        {
            Debug.Log($"{gameObject.name}はPlayerがいるため移動できません");
            return;
        }

        // 4. 移動先にゴールがないかチェック
        if (IsGoalAtPosition(newPosition))
        {
            Debug.Log($"{gameObject.name}はGoalがあるため移動できません");
            return;
        }

        // 5. 移動先に他のMovingObjectがいないかチェック
        if (IsOtherMovingObjectAtPosition(newPosition))
        {
            Debug.Log($"{gameObject.name}は他のMovingObjectがいるため移動できません");
            return;
        }

        // すべてのチェックをクリア - 移動を実行
        ExecuteMove(newPosition);
    }

    // 移動を実行
    void ExecuteMove(Vector3 newPosition)
    {
        transform.position = newPosition;
        hasMoved = true;
        isSelected = false;

        // 移動済みは灰色に
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;
        }

        Debug.Log($"{gameObject.name}が移動しました: {newPosition} （このオブジェクトはもう動かせません）");
    }

    // 指定位置にmapオブジェクトがあるかチェック
    bool IsMapAtPosition(Vector3 position)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.ToLower().Contains("map"))
            {
                float distance = Vector3.Distance(obj.transform.position, position);

                if (distance < 1.5f)
                {
                    Debug.Log($"→ map '{obj.name}' が移動先にあります (距離: {distance:F2})");
                    return true;
                }
            }
        }

        Debug.Log("→ 移動先にmapが見つかりません");
        return false;
    }

    // 指定位置にPlayerがいるかチェック
    bool IsPlayerAtPosition(Vector3 position)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, position);

            if (distance < MAP_SIZE)
            {
                Debug.Log($"→ Player '{player.name}' が移動先にいます (距離: {distance:F2})");
                return true;
            }
        }
        else
        {
            Debug.LogWarning("Playerタグを持つオブジェクトが見つかりません！");
        }

        return false;
    }

    // 指定位置に他のMovingObjectがいるかチェック
    bool IsOtherMovingObjectAtPosition(Vector3 position)
    {
        MovingObject[] movingObjects = FindObjectsOfType<MovingObject>();

        foreach (MovingObject obj in movingObjects)
        {
            // 自分自身は除外
            if (obj == this)
            {
                continue;
            }

            float distance = Vector3.Distance(obj.transform.position, position);

            if (distance < 1.0f)
            {
                Debug.Log($"→ MovingObject '{obj.name}' が移動先にいます (距離: {distance:F2})");
                return true;
            }
        }

        return false;
    }

    bool IsGoalAtPosition(Vector3 position)
    {
        GameObject goal = GameObject.FindGameObjectWithTag("Goal");

        if (goal != null)
        {
            float distance = Vector3.Distance(goal.transform.position, position);

            if (distance < MAP_SIZE)
            {
                Debug.Log($"→ Goal '{goal.name}' が移動先にいます (距離: {distance:F2})");
                return true;
            }
        }
        else
        {
            Debug.LogWarning("Goalタグを持つオブジェクトが見つかりません！");
        }

        return false;
    }

    // 移動状態をリセットするメソッド（外部から呼び出し可能）
    public void ResetMovement()
    {
        hasMoved = false;
        isSelected = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        Debug.Log($"{gameObject.name}の移動状態をリセットしました");
    }

    // 移動済みかどうかを確認するメソッド
    public bool HasMoved()
    {
        return hasMoved;
    }
}