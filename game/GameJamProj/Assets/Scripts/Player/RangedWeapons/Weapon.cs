/**
 * Author: Hudson
 * Contributors: 
 * Description: Defines a generic ranged weapon
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define weapon class
public class Weapon : ScriptableObject
{

    /* Bullet prefab */
    protected GameObject m_bulletPrefab = null;

    /* UI Script Component */
    GenericBar m_uiBar = null;

    /* Properties of weapon */
    protected string m_name = "null";
    protected Sprite m_icon = null;
    protected Color m_textColor = Color.white;

    protected float m_projectileSpeed = 1.0f;
    protected float m_bulletAliveTime = 5.0f;
    protected float m_shotCooldown = 1.0f;

    protected int m_damagePerRound = 1;

    private float m_currCooldown = 0.0f;

    public Weapon(
        string name,
        Color textColor,
        Sprite icon,
        float shotCooldown,
        int damagePerRound,
        float projectileSpeed,
        float bulletAliveTime,
        GameObject bulletPrefab
    )
    {
        m_name = name;
        m_textColor = textColor;
        m_icon = icon;
        m_shotCooldown = shotCooldown;
        m_damagePerRound = damagePerRound;
        m_projectileSpeed = projectileSpeed;
        m_bulletAliveTime = bulletAliveTime;
        m_bulletPrefab = bulletPrefab;
    }

    // run to update the cooldown fields and UI bar
    /// <summary>
    /// For use in RangedWeaponManager.cs only.
    /// </summary>
    /// <param name="active">Put if the weapon is currently active</param>
    /// <param name="onlyChargeIfActive">Determines if cooldown is only updated when the weapon is active</param>
    public void _Update(bool active, bool onlyChargeIfActive = false)
    {
        // Update cooldown
        if (m_currCooldown > 0.0f && !onlyChargeIfActive)
            m_currCooldown -= Time.deltaTime;

        // Update UI bar
        if (m_uiBar != null && active)
            m_uiBar.SetBarValue(m_currCooldown, m_shotCooldown);

    }

    /// <summary>
    /// For use in RangedWeaponManager.cs only. 
    /// Attempts to shoot the gun.
    /// </summary>
    /// <param name="spawnPos">player location (transform.position)</param>
    /// <param name="direction">direction to shoot ((playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10.0f)) - transform.position).normalized;)</param>
    /// <returns>Whether or not player is able to shoot</returns>
    public virtual bool _Shoot(GameObject player, Vector3 spawnPos, Vector2 direction, int damageIncrease)
    {

        // Check if we can shoot
        if (m_currCooldown <= 0.0f)
        {

            // Reset cooldown
            ResetCooldown();

            // Spawn a bullet with the correct attributes
            GameObject projectile = Instantiate(m_bulletPrefab, spawnPos, Quaternion.identity);

            // Set projectile attributes
            projectile.GetComponent<Rigidbody2D>().velocity = direction;
            projectile.GetComponent<Rigidbody2D>().velocity = projectile.GetComponent<Rigidbody2D>().velocity.normalized * m_projectileSpeed;

            projectile.GetComponent<ProjectileAttackScript>().damage = m_damagePerRound + damageIncrease;
            projectile.GetComponent<ProjectileAttackScript>().maxAliveTime = m_bulletAliveTime;

            // TODO: play shoot sound

            return true;

        }

        return false;

    }

    /// <summary>
    /// For use in RangedWeaponManager.cs only. 
    /// Links the UI bar so it can update it
    /// </summary>
    /// <param name="uiBarComponent">UI bar component</param>
    public void _LinkUIBar(GenericBar uiBarComponent)
    {
        // Don't waste time if UI bar is already linked
        if (m_uiBar != null) return;
        m_uiBar = uiBarComponent;
    }

    protected bool CanShoot()
    {
        return m_currCooldown <= 0.0f;
    }

    protected void ResetCooldown()
    {
        m_currCooldown = m_shotCooldown;
    }

    /* Getters */

    public string GetWeaponName()
    {
        return m_name;
    }

    public Sprite GetSprite()
    {
        return m_icon;
    }

    public Color GetTextColor()
    {
        return m_textColor;
    }

}
