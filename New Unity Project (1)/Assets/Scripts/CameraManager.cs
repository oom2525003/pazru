using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")&&!other.isTrigger)
        {
            // エリア内はカメラ有効
            virtualCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // エリア外はカメラ無効
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCamera.SetActive(false);
        }
    }
}
