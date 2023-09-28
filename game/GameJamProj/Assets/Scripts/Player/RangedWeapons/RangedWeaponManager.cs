/**
 * Author: Hudson
 * Contributors: 
 * Description: Manages all ranged weapons
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
