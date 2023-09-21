using UnityEngine;

public class AttackScript : MonoBehaviour
{
    // Variables needed for this script
    [SerializeField] private ParticleSystem killEffect;

    // This will kill an enemy upon hitting them
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            killEffect.Play();
            Camera.main.GetComponent<CameraShake>().Shake(0.1f, 0.2f);
        }
    }
}