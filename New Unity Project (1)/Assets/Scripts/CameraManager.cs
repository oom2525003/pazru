using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject player;

    // 前フレームでのプレイヤーの座標
    Vector3 prePlayerPos;


    // カメラの範囲
    private const float cameraWidth = 2.5f, cameraHeight = 2.0f;

    private float edgeRight, edgeLeft, edgeUp, edgeDown;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        edgeLeft = transform.position.x + cameraWidth / 2;
        edgeRight = transform.position.x - cameraWidth / 2;
        edgeDown = transform.position.y - cameraWidth;
        edgeUp = transform.position.y + cameraWidth;

        if (player.transform.position != prePlayerPos)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            prePlayerPos = player.transform.position;
        }

        //if (player.transform.position.x>edgeLeft&& player.transform.position.x < edgeRight)
        //{
        //    transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0)d;
        //}

    }
}
