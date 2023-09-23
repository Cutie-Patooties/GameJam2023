/**
 * Author: Alan
 * Contributors: N/A
 * Description: This script makes enemies chase down the player
**/

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Variables needed for this script
    public float movementSpeed;
    private Rigidbody2D enemyrb;
    private GameObject player;

    private void Start()
    {
        enemyrb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("EntityPlayer");
    }

    private void Update()
    {
        // Have enemy go towards player's position
        Vector2 direction = (player.transform.position - transform.position).normalized;
        enemyrb.velocity = direction * movementSpeed;
    }
}
