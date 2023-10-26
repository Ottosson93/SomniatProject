using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
public class SpellAttackSystem : MonoBehaviour
{
    [SerializeField] private Spell currentSpell;
    [SerializeField] private Transform castPoint;
    [SerializeField] private float timeBetweenCasts = 0.3f;
    private float currentCastTimer;

    private ThirdPersonController controller;

    // Added player so that lucidity is fetched from this class.
    private Player player;

    private bool castingSpell = false;

    private InputAction spellInput;


    public void UpdateSpell(Spell spell)
    {
        currentSpell = spell;
    }

    private void Awake()
    {
        controller = GetComponent<ThirdPersonController>();
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
        bool hasEnoughLucidity = player.lucidity - currentSpell.SpellToCast.LucidityCost > 0f;
        if(!castingSpell && spellInput.triggered && hasEnoughLucidity)
        {
            Debug.Log("Casted Spell");
            castingSpell = true;
            player.lucidity -= currentSpell.SpellToCast.LucidityCost;
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

    public void PickUpNewSpell(Spell newSpell)
    {
        currentSpell = newSpell;
    }

    private void CastSpell()
    {
        var (success, position) = controller.GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - castPoint.position;

            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            castPoint.forward = direction;
            Debug.Log("Updated forward direction");
        }
        Instantiate(currentSpell, castPoint.position, castPoint.rotation);
        Debug.Log(castPoint.rotation);
    }

    public void AICastSpell(Spell spell, Transform castPoint, Transform PlayerPos)
    {
        Vector3 direction = ( PlayerPos.position - castPoint.position).normalized;
        direction.y = 0;
        castPoint.forward = direction;

        Instantiate(spell, castPoint.position, castPoint.rotation);

    }
}
