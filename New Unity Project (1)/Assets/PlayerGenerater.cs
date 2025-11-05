using UnityEngine;

public class PlayerGenerater : MonoBehaviour
{
    // 移動回数カウント
    private int moveCount = 0;

    // 最後にカウントを更新したCollider
    private Collider2D lastCollider = null;

    void Start()
    {
        // 初期状態では最後のColliderはnull
        lastCollider = null;
    }

    // 物理的に別のColliderに接触したときにMoveCountを増加
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.name); // どのColliderと接触したか確認

        // 最初の接触時、または異なるColliderに接触した場合にカウントを増加
        if (lastCollider == null || lastCollider != other)
        {
            moveCount++;
            lastCollider = other; // 接触したColliderを保存
            Debug.Log("Move Count: " + moveCount);
        }
    }

    // 物理衝突時にも対応（`Is Trigger` が無効な場合）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 最初の接触時、または異なるColliderに接触した場合にカウントを増加
        if (lastCollider == null || lastCollider != collision.collider)
        {
            moveCount++;
            lastCollider = collision.collider; // 接触したColliderを保存
            Debug.Log("Move Count (Collision): " + moveCount);
        }
    }
}
