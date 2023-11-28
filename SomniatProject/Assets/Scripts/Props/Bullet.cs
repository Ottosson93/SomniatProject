using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Collider collider = this.gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
        else if (other.gameObject.CompareTag("DestructibleObject"))
        {
            other.GetComponent<ExplosiveObject>().TakeDamage(damage);
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }

}
