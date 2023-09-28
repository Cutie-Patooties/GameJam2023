/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script spawns enemy projectiles that the player must avoid
**/

using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    // Variables needed for this script
    public float damage;
    public float speed;

    private Vector3 position;
    private Vector3 direction;

    private GameObject player;
    private GameManager game;

    [SerializeField] private GameObject blastEffect;

    private void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManager>();

        player = GameObject.Find("EntityPlayer");
        position = player.transform.position;
        direction = (position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = rotation;
    }

    private void Update()
    {
        if (!game.hasEnded)
        {
            transform.Translate(speed * Time.deltaTime * direction, Space.World);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().currentHealth -= damage;
            Instantiate(blastEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (other.CompareTag("OutOfBounds"))
        {
            Destroy(gameObject);
        }
    }
}