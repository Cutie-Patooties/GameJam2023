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

    [Header("Required Game Objects")]
    [SerializeField] private GameObject WeaponIconObject = null;
    [SerializeField] private GameObject projectileObject = null;
    [SerializeField] private GameObject weaponCooldownBar = null;

    // Weapon Sprites
    [Header("Default Weapon Sprites")]
    [SerializeField] private Sprite noWeaponSprite = null;
    [SerializeField] private Sprite defaultWeaponSprite = null;
    [SerializeField] private Sprite shotgunSprite = null;

    [Header("Controls")]
    [SerializeField] private bool invertScrollWheel = false;

    private PlayerAttack attackScript = null;
    
    private Camera playerCamera = null;

    //[System.NonSerialized]
    private List<Weapon> m_weapons = null;

    private int activeWeapon = 0;

    private void Awake()
    {
        // Instantiate the weapons list
        m_weapons = new List<Weapon>();
    }

    // Start is called before the first frame update
    void Start()
    {

        // Get attack script
        attackScript = GetComponent<PlayerAttack>();

        // Set playerCamera to the main camera
        playerCamera = Camera.main;

        // Ray gun                 name       text color   sprite            cooldwn dmg spd  alive  bulletObject
        //m_weapons.Add(new Weapon("Ray Gun", Color.white, defaultWeaponSprite, 0.5f, 1, 5.0f, 5.0f, projectileObject));
        // Shotgun                  name       text color sprite      cooldwn dmg spd  alive  bulletObject      spreadOffset
        //m_weapons.Add(new Shotgun("Shotgun", Color.red, shotgunSprite, 1.5f, 3, 3.0f, 1.0f, projectileObject, Mathf.Deg2Rad * 10.0f));

        /* Add weapons player should start with here */
        AddWeapon(new Weapon("Ray Gun", Color.white, defaultWeaponSprite, 0.5f, 1, 5.0f, 5.0f, projectileObject));
        AddWeapon(new Shotgun("Shotgun", Color.green, shotgunSprite, 1.5f, 3, 3.0f, 1.0f, projectileObject, Mathf.Deg2Rad * 10.0f));

    }

    // Update is called once per frame
    void Update()
    {

        // Guard in case player has no weapon
        if(m_weapons.Count == 0)
        {

            // technically unnessesary but useful if player receives multiple weapons at once when having none
            activeWeapon = 0;
            // Update weapon icon
            WeaponIconObject.GetComponent<SpriteRenderer>().sprite = noWeaponSprite;
            // Update weapon name text and text color
            WeaponIconObject.transform.Find("WeaponName").GetComponent<TextMeshProUGUI>().text = "Unarmed";
            WeaponIconObject.transform.Find("WeaponName").GetComponent<TextMeshProUGUI>().color = Color.white;

            return;

        }

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
        // Update weapon name text and text color
        WeaponIconObject.transform.Find("WeaponName").GetComponent<TextMeshProUGUI>().text = m_weapons[activeWeapon].GetWeaponName();
        WeaponIconObject.transform.Find("WeaponName").GetComponent<TextMeshProUGUI>().color = m_weapons[activeWeapon].GetTextColor();

        // Update all weapons
        for(int i = 0; i < m_weapons.Count; ++i)
            m_weapons[i]._Update(i == activeWeapon);

        // Check if player wants to shoot their gun
        if (Input.GetButton("Shoot"))
        {

            // Direction player wants to shoot
            Vector2 dir = (playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10.0f)) - transform.position).normalized;

            // Attempt to shoot weapon and store if successful
            bool shotFired = m_weapons[activeWeapon]._Shoot(transform.gameObject, transform.position, dir, attackScript.damageIncrease);

            if (!shotFired)
            {
                // TODO: play out of ammo sound
            }

        }

    }

    /// <summary>
    /// Adds a weapon to the player's useable weapons. Checking for existing weapons is done via the weapon's name. The check for existing names is case-insensitive
    /// </summary>
    /// <param name="weapon">The weapon to add</param>
    /// <param name="overrideIfAlreadyExists">True - If the weapon already exists in the list, override it\nFalse: If the weapon already exists in the list, do nothing</param>
    /// <returns>true if added new weapon, false if weapon already existed in list (will still return false even if overrideIfAlreadyExists is true)</returns>
    public bool AddWeapon(Weapon weapon, bool overrideIfAlreadyExists = true)
    {

        // Link UI bar to weapon
        weapon._LinkUIBar(weaponCooldownBar.GetComponent<GenericBar>());

        // Loop thru list and make sure that we are not adding a duplicate weapon
        //foreach(Weapon w in m_weapons)
        for(int i = 0; i < m_weapons.Count; ++i)
        {
            // If the weapon with the same name (case-insensitive) is found either override it or do nothing depending on preferences
            if (m_weapons[i].GetWeaponName().ToLower().Equals(weapon.GetWeaponName().ToLower())) {

                if(overrideIfAlreadyExists)
                    m_weapons[i] = weapon;

                return false;

            }
        }

        // If we make it here, the weapon was not found in the player's inventory so add it
        m_weapons.Add(weapon);

        return true;

    }

    /// <summary>
    /// Attempts to remove the weapon with specified name (case-insensitive)
    /// </summary>
    /// <param name="weapon">Name of weapon you wish to remove</param>
    /// <returns>If we found a weapon with the given name to remove</returns>
    public bool RemoveWeapon(string weaponName)
    {

        // Loop thru all weapons
        for(int i = 0; i < m_weapons.Count; ++i)
        {
            // If we find a weapon with the same name (case-insensitive) delete it
            if (m_weapons[i].GetWeaponName().ToLower().Equals(weaponName))
            {
                m_weapons.RemoveAt(i);
                return true;
            }
        }

        return false;

    }

}

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
        if(m_uiBar != null && active)
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

// Define shotgun class
public class Shotgun : Weapon
{

    protected float m_shotgunSpreadAngleRadians = 0.0f;

    public Shotgun(
        string name,
        Color textColor,
        Sprite icon,
        float shotCooldown,
        int damagePerRound,
        float projectileSpeed,
        float bulletAliveTime,
        GameObject bulletPrefab,
        float shotgunSpreadAngleRadians
    ) : base(name, textColor, icon, shotCooldown, damagePerRound, projectileSpeed, bulletAliveTime, bulletPrefab)
    {
        m_shotgunSpreadAngleRadians = shotgunSpreadAngleRadians;
    }

    public override bool _Shoot(GameObject player, Vector3 spawnPos, Vector2 direction, int damageIncrease)
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
