/**
 * Author: Hudson
 * Contributors: 
 * Description: Manages weapons
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RangedWeaponManager : MonoBehaviour
{

    [SerializeField] private GameObject WeaponIconObject = null;
    [SerializeField] private GameObject projectileObject = null;

    // Weapon Sprites
    [SerializeField] private Sprite defaultWeaponSprite = null;
    [SerializeField] private Sprite shotgunSprite = null;

    [SerializeField] private bool invertScrollWheel = false;

    private PlayerAttack attackScript = null;
    
    private Camera playerCamera = null;

    [System.NonSerialized]
    public List<Weapon> m_weapons = null;

    public int activeWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {

        // Get attack script
        attackScript = GetComponent<PlayerAttack>();

        // Set playerCamera to the main camera
        playerCamera = Camera.main;

        // Instantiate the weapons list
        m_weapons = new List<Weapon>();

        // Ray gun               name       sprite            cooldwn dmg spd  alive  bulletObject
        m_weapons.Add(new Weapon("Ray Gun", defaultWeaponSprite, 0.5f, 1, 5.0f, 5.0f, projectileObject));

        // Shotgun                name       sprite      cooldwn dmg spd  alive  bulletObject      spreadOffset
        m_weapons.Add(new Shotgun("Shotgun", shotgunSprite, 1.5f, 3, 3.0f, 1.0f, projectileObject, Mathf.Deg2Rad * 10.0f));

    }

    // Update is called once per frame
    void Update()
    {

        // Check if player wants to switch weapon
        if (Input.mouseScrollDelta.y > 0)
            activeWeapon += (!invertScrollWheel ? -1 : 1);
        else if (Input.mouseScrollDelta.y < 0)
            activeWeapon += (!invertScrollWheel ? 1 : -1);

        // Ensure active weapon is in bounds of available weapons
        if (activeWeapon >= m_weapons.Count)
            activeWeapon = m_weapons.Count - 1;
        else if (activeWeapon < 0)
            activeWeapon = 0;

        // Update weapon icon
        WeaponIconObject.GetComponent<SpriteRenderer>().sprite = m_weapons[activeWeapon].GetSprite();
        // Update weapon name
        WeaponIconObject.transform.Find("WeaponName").GetComponent<TextMeshProUGUI>().text = m_weapons[activeWeapon].GetWeaponName();

        // Update cooldown on all weapons
        foreach (Weapon weapon in m_weapons)
            weapon.Update();

        // Check if player wants to shoot their gun
        if (Input.GetButton("Shoot"))
        {

            // Direction player wants to shoot
            Vector2 dir = (playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10.0f)) - transform.position).normalized;

            // Attempt to shoot weapon and store if successful
            bool shotFired = m_weapons[activeWeapon].Shoot(transform.gameObject, transform.position, dir, attackScript.damageIncrease);

            if (!shotFired)
            {
                // TODO: play out of ammo sound
            }

        }

    }
}

// Define weapon class
public class Weapon : MonoBehaviour
{

    /* Bullet prefab */
    protected GameObject m_bulletPrefab = null;

    /* Properties of weapon */
    protected string m_name = "null";
    protected Sprite m_icon = null;

    protected float m_projectileSpeed = 1.0f;
    protected float m_bulletAliveTime = 5.0f;
    protected float m_shotCooldown = 1.0f;

    protected int m_damagePerRound = 1;

    private float m_currCooldown = 0.0f;

    public Weapon(string name, Sprite icon, float shotCooldown, int damagePerRound, float projectileSpeed, float bulletAliveTime, GameObject bulletPrefab)
    {
        m_name = name;
        m_icon = icon;
        m_shotCooldown = shotCooldown;
        m_damagePerRound = damagePerRound;
        m_projectileSpeed = projectileSpeed;
        m_bulletAliveTime = bulletAliveTime;
        m_bulletPrefab = bulletPrefab;
    }

    // run to update the cooldown fields
    public void Update()
    {
        if (m_currCooldown > 0.0f)
        {
            m_currCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Attempts to shoot the gun
    /// </summary>
    /// <param name="spawnPos">player location (transform.position)</param>
    /// <param name="direction">direction to shoot ((playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10.0f)) - transform.position).normalized;)</param>
    /// <returns>Whether or not player is able to shoot</returns>
    public virtual bool Shoot(GameObject player, Vector3 spawnPos, Vector2 direction, int damageIncrease)
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

    protected bool CanShoot()
    {
        return m_currCooldown <= 0.0f;
    }

    protected void ResetCooldown()
    {
        m_currCooldown = m_shotCooldown;
    }

    public string GetWeaponName()
    {
        return m_name;
    }

    public Sprite GetSprite()
    {
        return m_icon;
    }

}

// Define shotgun class
public class Shotgun : Weapon
{

    protected float m_shotgunSpreadAngleRadians = 0.0f;

    public Shotgun(
        string name,
        Sprite icon,
        float shotCooldown,
        int damagePerRound,
        float projectileSpeed,
        float bulletAliveTime,
        GameObject bulletPrefab,
        float shotgunSpreadAngleRadians
    ) : base(name, icon, shotCooldown, damagePerRound, projectileSpeed, bulletAliveTime, bulletPrefab)
    {
        m_shotgunSpreadAngleRadians = shotgunSpreadAngleRadians;
    }

    public override bool Shoot(GameObject player, Vector3 spawnPos, Vector2 direction, int damageIncrease)
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
