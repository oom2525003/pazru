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

            // コルーチンで停止
            StartCoroutine(Restart());
        }
    }

    public IEnumerator Restart()
    {
        // ○秒停止する
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3.0f);

        // 再開する
        Time.timeScale = 1;

        // クリア画面に移動
        SceneManager.LoadScene("ClearScene");
    }
}

