using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
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

