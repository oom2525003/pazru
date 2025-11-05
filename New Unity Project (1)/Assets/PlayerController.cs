using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // プレイヤーの移動速度
    public float moveSpeed = 5f;

    private Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        // 入力を取得 (WASDまたは矢印キー)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 斜め移動を防ぐため、横方向と縦方向の入力を1方向だけに制限
        if (horizontal != 0 && vertical != 0)
        {
            // 横方向と縦方向が両方押された場合、優先するのはどちらか一方のみ
            // 横方向優先 (例えば、WとAが押されたら、Wだけで移動する)
            horizontal = 0;
        }


        // 移動方向を設定
        movement = new Vector2(horizontal, vertical).normalized;

        // プレイヤーを移動
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
