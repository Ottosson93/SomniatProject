using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLuciditySystem : MonoBehaviour
{
    [SerializeField] private Spell spellToCast;
    [SerializeField] private Transform castPoint;
    [SerializeField] private float timeBetweenCasts = 0.3f;
    private float currentCastTimer;

    // Added player so that lucidity is fetched from this class.
    private Player player;

    private bool castingSpell = false;

    private InputAction spellInput;

    private void Awake()
    {
        spellInput = new InputAction("Spell Cast", binding: "<Keyboard>/q");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }

    private void OnEnable()
    {
        spellInput.Enable();
    }

    private void OnDisable()
    {
        spellInput.Disable();
    }

    private void Update()
    {
        bool hasEnoughLucidity = player.lucidity - spellToCast.SpellToCast.LucidityCost > 0f;
        if(!castingSpell && spellInput.triggered && hasEnoughLucidity)
        {
            castingSpell = true;
            player.lucidity -= spellToCast.SpellToCast.LucidityCost;
            currentCastTimer = 0;
            CastSpell();
        }
        if (castingSpell)
        {
            currentCastTimer += Time.deltaTime;

            if(currentCastTimer > timeBetweenCasts)
            {
                castingSpell = false;
            }
        }
    }

    private void CastSpell()
    {
        Instantiate(spellToCast, castPoint.position, castPoint.rotation);
    }
}
