/**
 * Author: Hudson
 * Contributors: Alan
 * Description: Manages all ranged weapons
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(AudioSource))]
public class RangedWeaponManager : MonoBehaviour
{

    [Header("Required Game Objects")]
    [SerializeField] private GameObject weaponIconObject = null;
    [SerializeField] public GameObject projectileObject = null;
    [SerializeField] private GameObject weaponCooldownBar = null;

    // Weapon Sprites
    [Header("Default Weapon Sprites")]
    [SerializeField] public Sprite noWeaponSprite = null;
    [SerializeField] public Sprite defaultWeaponSprite = null;
    [SerializeField] public Sprite shotgunSprite = null;

    [Header("Default Weapon Sound Effects")]
    [SerializeField] public AudioClip defaultWeaponSound = null;
    [SerializeField] public AudioClip shotgunSound = null;

    [Header("Controls")]
    [SerializeField] private bool invertScrollWheel = false;

    private PlayerAttack attackScript = null;
    
    private Camera playerCamera = null;

    //[System.NonSerialized]
    private List<RangedWeapon> m_weapons = null;

    private int activeWeapon = 0;

    private void Awake()
    {
        // Instantiate the weapons list
        m_weapons = new List<RangedWeapon>();
    }

    // Start is called before the first frame update
    void Start()
    {

        // Get attack script
        attackScript = GetComponent<PlayerAttack>();

        // Set playerCamera to the main camera
        playerCamera = Camera.main;

        // Do not add weapons this way, deprecated
        // Ray gun                 name       text color   sprite            cooldwn dmg spd  alive  bulletObject
        //m_weapons.Add(new Weapon("Ray Gun", Color.white, defaultWeaponSprite, 0.5f, 1, 5.0f, 5.0f, projectileObject));
        // Shotgun                  name       text color sprite      cooldwn dmg spd  alive  bulletObject      spreadOffset
        //m_weapons.Add(new Shotgun("Shotgun", Color.red, shotgunSprite, 1.5f, 3, 3.0f, 1.0f, projectileObject, Mathf.Deg2Rad * 10.0f));

        /* Add weapons player should start with here (ignore warnings) */
        // UPDATE: Just call ResetWeapons() instead
        //AddWeapon(new RangedWeapon("Rapid Fire", Color.white, defaultWeaponSprite, defaultWeaponSound, 1, 0.125f, 1, 50.0f, 3.0f, projectileObject));
        //AddWeapon(new WeaponShotgun("Burst Shot", Color.white, shotgunSprite, shotgunSound, 1, 0.75f, 5, 25.0f, 0.75f, projectileObject, Mathf.Deg2Rad * 10.0f));
        ResetWeapons();

    }

    // Update is called once per frame
    void Update()
    {

        // Guard in case player has no weapon
        if(m_weapons.Count == 0)
        {

            // technically unnessesary but useful if player receives multiple weapons at once when having none
            activeWeapon = 0;

            // Update UI
            UpdateWeaponUI(noWeaponSprite, "Unarmed", -1, Color.white);

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

        // Update UI
        UpdateWeaponUI(m_weapons[activeWeapon].GetSprite(), m_weapons[activeWeapon].GetWeaponName(), m_weapons[activeWeapon].GetTier(), m_weapons[activeWeapon].GetTextColor());

        // Update all weapons
        for(int i = 0; i < m_weapons.Count; ++i)
            m_weapons[i]._Update(i == activeWeapon);

        // Check if player wants to shoot their gun
        if (Input.GetButton("Shoot") && attackScript.canAttack)
        {

            // Direction player wants to shoot
            Vector2 dir = (playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10.0f)) - transform.position).normalized;

            // Attempt to shoot weapon and store if successful
            bool shotFired = m_weapons[activeWeapon]._Shoot(transform.gameObject, transform.position, dir, attackScript.damageIncrease);

            if (shotFired)
            {
                attackScript.canAttack = false;
                StartCoroutine(EnableProjectile());
            }
            else
            {
                // TODO: play out of ammo sound
            }

        }

    }

    /// <summary>
    /// Adds a weapon to the player's useable weapons. Checking for existing weapons is done via the weapon's name. The check for existing names is case-insensitive
    /// </summary>
    /// <param name="weapon">The weapon to add</param>
    /// <param name="overrideIfLowerTier">True - If the weapon already exists in the list, override it\nFalse: If the weapon already exists in the list, do nothing</param>
    /// <returns>true if added new weapon, false if weapon already existed in list (will still return false even if overrideIfAlreadyExists is true)</returns>
    public bool AddWeapon(RangedWeapon weapon, bool overrideIfLowerTier = true)
    {

        // Link UI bar to weapon
        weapon._LinkUIBar(weaponCooldownBar.GetComponent<GenericBar>());

        // Loop thru list and make sure that we are not adding a duplicate weapon
        //foreach(Weapon w in m_weapons)
        for(int i = 0; i < m_weapons.Count; ++i)
        {
            // If the weapon with the same name (case-insensitive) is found either override it or do nothing depending on preferences
            if (m_weapons[i].GetWeaponName().ToLower().Equals(weapon.GetWeaponName().ToLower())) {

                // Check if we should override and if our new weapon's tier is greater than the current weapon tier
                if(overrideIfLowerTier && m_weapons[i].GetTier() < weapon.GetTier())
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
            if (m_weapons[i].GetWeaponName().ToLower().Equals(weaponName.ToLower()))
            {
                m_weapons.RemoveAt(i);
                return true;
            }
        }

        return false;

    }

    public void ResetWeapons()
    {
        RemoveWeapon("Rapid Fire");
        RemoveWeapon("Burst Shot");

        AddWeapon(new RangedWeapon("Rapid Fire", Color.green, defaultWeaponSprite, defaultWeaponSound, 1, 0.125f, 1, 50.0f, 3.0f, projectileObject));
        AddWeapon(new WeaponShotgun("Burst Shot", Color.green, shotgunSprite, shotgunSound, 1, 0.75f, 5, 25.0f, 0.75f, projectileObject, Mathf.Deg2Rad * 10.0f));
    }

    private void UpdateWeaponUI(Sprite icon, string weaponName, int tier, Color tierColor)
    {
        // Update weapon icon
        weaponIconObject.GetComponent<SpriteRenderer>().sprite = icon;
        // Update weapon name text and text color
        weaponIconObject.transform.Find("WeaponName").GetComponent<TextMeshProUGUI>().text = weaponName;
        if (tier == -1)
            weaponIconObject.transform.Find("WeaponTier").GetComponent<TextMeshProUGUI>().text = "";
        else
            weaponIconObject.transform.Find("WeaponTier").GetComponent<TextMeshProUGUI>().text = "Tier " + tier;
        weaponIconObject.transform.Find("WeaponTier").GetComponent<TextMeshProUGUI>().color = tierColor;
    }

    IEnumerator EnableProjectile()
    {
        yield return new WaitForSeconds(weaponCooldownBar.GetComponent<GenericBar>().GetMaxBarValue());
        attackScript.canAttack = true;
    }

}