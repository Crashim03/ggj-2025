using Unity.VisualScripting;
using UnityEngine;

public class EndPointScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.DecreaseHealth();
            Destroy(other.gameObject);
        }
    }
}
