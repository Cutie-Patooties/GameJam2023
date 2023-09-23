/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script just destroys a particle effect when it's done
**/

using UnityEngine;

public class DisableParticleEffect : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!_particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
