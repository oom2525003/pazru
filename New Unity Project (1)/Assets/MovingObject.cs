using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // マップのサイズ
    private const float MAP_SIZE = 2.0f;

    // マップの範囲設定
    private const float START_X = -5.0f;
    private const float START_Y = 3.0f;
    private const float END_X = 5.0f;
    private const float END_Y = -3.0f;

    // 選択されているかどうか
    private bool isSelected = false;

    // 既に移動したかどうか
    private bool hasMoved = false;

    // 選択時のエフェクト用（オプション）
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // SpriteRendererがあれば取得（色を変えて選択状態を表示するため）
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Collider2Dの確認とZ座標の調整
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning($"{gameObject.name}にCollider2Dがありません！BoxCollider2Dを追加してください。");
        }

        // Z座標を手前に調整（クリック判定を優先）
        Vector3 pos = transform.position;
        if (pos.z >= 0)
        {
            transform.position = new Vector3(pos.x, pos.y, -1);
        }
    }

    void Update()
    {
        // 選択されている場合のみ十字キーで移動
        if (isSelected)
        {
            // 上キー
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(0, MAP_SIZE);
            }
            // 下キー
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(0, -MAP_SIZE);
            }
            // 左キー
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(-MAP_SIZE, 0);
            }
            // 右キー
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(MAP_SIZE, 0);
            }
        }
    }

    // オブジェクトをクリックしたときの処理
    void OnMouseDown()
    {
        Debug.Log($"{gameObject.name}がクリックされました");

        // 他のすべてのMovingObjectの選択を解除
        MovingObject[] allMovingObjects = FindObjectsOfType<MovingObject>();
        foreach (MovingObject obj in allMovingObjects)
        {
            obj.Deselect();
        }

        // このオブジェクトを選択
        Select();
    }

    // オブジェクトを選択状態にする
    void Select()
    {
        isSelected = true;

        // 選択状態を視覚的に表示（色を変える）
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
        }

        Debug.Log($"{gameObject.name}を選択しました");
    }

    // 選択を解除する
    void Deselect()
    {
        isSelected = false;

        // 元の色に戻す
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    // 指定方向に移動する
    void Move(float deltaX, float deltaY)
    {
        Vector3 newPosition = transform.position + new Vector3(deltaX, deltaY, 0);

        // マップの範囲外に出ないかチェック
        if (newPosition.x < START_X || newPosition.x > END_X ||
            newPosition.y > START_Y || newPosition.y < END_Y)
        {
            Debug.Log($"{gameObject.name}はマップの範囲外に移動できません");
            return;
        }

        // 移動先にmapオブジェクトがあるかチェック
        if (!IsMapAtPosition(newPosition))
        {
            Debug.Log($"{gameObject.name}は移動先にmapがないため移動できません");
            return;
        }

        // 移動先にPlayerがいないかチェック
        if (IsPlayerAtPosition(newPosition))
        {
            Debug.Log($"{gameObject.name}はPlayerがいるため移動できません");
            return;
        }

        // 移動先に他のMovingObjectがいないかチェック
        if (IsOtherMovingObjectAtPosition(newPosition))
        {
            Debug.Log($"{gameObject.name}は他のMovingObjectがいるため移動できません");
            return;
        }

        // 移動を実行
        transform.position = newPosition;

        hasMoved = true;
        isSelected = false;

        // 元の色に戻す
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        Debug.Log($"{gameObject.name}が移動しました: {newPosition}");
    }

    // 指定位置にmapオブジェクトがあるかチェック
    bool IsMapAtPosition(Vector3 position)
    {
        // "map"という名前を含むすべてのオブジェクトを検索
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        int mapCount = 0;

        foreach (GameObject obj in allObjects)
        {
            // オブジェクト名に"map"が含まれているかチェック（大文字小文字を区別しない）
            if (obj.name.ToLower().Contains("map"))
            {
                mapCount++;

                // 位置が一致するかチェック（小数点の誤差を考慮）
                float distance = Vector3.Distance(obj.transform.position, position);

                if (distance < 1.5f)
                {
                    Debug.Log($"->{obj.name}が移動先にあります");
                    return true;
                }
            }
        }
        Debug.Log($"map総数:{mapCount}個検出、移動先にmapなし");
        return false;
    }

    // 指定位置にPlayerがいるかチェック
    bool IsPlayerAtPosition(Vector3 position)
    {
        // Playerタグを持つオブジェクトを検索
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 位置が一致するかチェック（小数点の誤差を考慮）
            float distance = Vector3.Distance(player.transform.position, position);
            Debug.Log($"Player距離チェック: {distance} (位置: Player={player.transform.position}, 移動先={position})");

            if (distance < 1.0f) // 判定距離を1.0fに拡大
            {
                return true;
            }
        }
        else
        {
            Debug.LogWarning("Playerタグを持つオブジェクトが見つかりません！Playerオブジェクトに'Player'タグを設定してください。");
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

    // 指定位置に他のMovingObjectがいるかチェック
    bool IsOtherMovingObjectAtPosition(Vector3 position)
    {
        // シーン内のすべてのMovingObjectを取得
        MovingObject[] movingObjects = FindObjectsOfType<MovingObject>();

        foreach (MovingObject obj in movingObjects)
        {
            // 自分自身は除外
            if (obj == this)
            {
                continue;
            }

            // 位置が一致するかチェック（小数点の誤差を考慮）
            float distance = Vector3.Distance(obj.transform.position, position);

            if (distance < 1.0f) // 判定距離を1.0fに設定
            {
                return true;
            }
        }

        return false;
    }
}