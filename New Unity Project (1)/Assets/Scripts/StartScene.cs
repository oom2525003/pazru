using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScene : MonoBehaviour
{
    public void switchScene()
    {
        // ƒQ[ƒ€‰æ–Ê‚ÉˆÚ“®
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }
}
