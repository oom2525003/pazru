using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    // マップのサイズと範囲設定
    private const float MAP_SIZE = 2.0f;
    private const float START_X = -5.0f;
    private const float START_Y = 3.0f;
    private const float END_X = 5.0f;
    private const float END_Y = -3.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーがゴール
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("ゴール");
            // クリア画面に移動
            SceneManager.LoadScene("ClearScene");
        }
    }
}

