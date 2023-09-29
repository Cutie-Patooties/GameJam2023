/**
 * Author: Hudson
 * Contributors: 
 * Description: Defines the shotgun ranged weapon
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define shotgun class
public class WeaponShotgun : RangedWeapon
{

    protected float m_shotgunSpreadAngleRadians = 0.0f;

    public WeaponShotgun(
        string name,
        Color textColor,
        Sprite icon,
        int tier,
        float shotCooldown,
        int damagePerRound,
        float projectileSpeed,
        float bulletAliveTime,
        GameObject bulletPrefab,
        float shotgunSpreadAngleRadians
    ) : base(name, textColor, icon, tier, shotCooldown, damagePerRound, projectileSpeed, bulletAliveTime, bulletPrefab)
    {
        m_shotgunSpreadAngleRadians = shotgunSpreadAngleRadians;
    }

    public override bool _Shoot(GameObject player, Vector3 spawnPos, Vector2 direction, int damageIncrease, AudioClip sound)
    {
        // Check if we can shoot
        if (CanShoot())
        {

            // Reset cooldown
            ResetCooldown();

            // Spawn Projectiles
            SpawnProjectile(player, spawnPos, direction, damageIncrease, 0.0f);     // Center
            SpawnProjectile(player, spawnPos, direction, damageIncrease, m_shotgunSpreadAngleRadians);     // Left
            SpawnProjectile(player, spawnPos, direction, damageIncrease, -m_shotgunSpreadAngleRadians);     // Right

            // TODO: play shoot sound


            return true;

        }

        return false;
    }

    private void SpawnProjectile(GameObject player, Vector3 spawnPos, Vector2 direction, int damageIncrease, float offsetAngleRadians)
    {
        // Spawn a bullet with the correct attributes
        GameObject projectile = Instantiate(m_bulletPrefab, spawnPos, Quaternion.identity);

        // Calculate angle of projectile
        float directionAngle = directionToRadians(direction) + offsetAngleRadians;

        // Set projectile attributes
        projectile.GetComponent<Rigidbody2D>().velocity = radiansToDirection(directionAngle);
        projectile.GetComponent<Rigidbody2D>().velocity = projectile.GetComponent<Rigidbody2D>().velocity.normalized * m_projectileSpeed;

        projectile.GetComponent<ProjectileAttackScript>().damage = m_damagePerRound + damageIncrease;
        projectile.GetComponent<ProjectileAttackScript>().maxAliveTime = m_bulletAliveTime;
    }

    private float directionToRadians(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x);
    }

    private Vector2 radiansToDirection(float radians)
    {
        // Use unit circle def to convert the angle into coords (good review for trig for me so yay)
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

}
