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
            // クリア画面に移動
            SceneManager.LoadScene("ClearScene");
        }
    }
}

