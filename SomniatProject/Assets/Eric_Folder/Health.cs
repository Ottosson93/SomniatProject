using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;

    public abstract void TakeDamage(int damage);

}
