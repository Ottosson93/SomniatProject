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

        if (other.gameObject.CompareTag("Mask"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other);
            Debug.Log("Lol");
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);

            Vector3 boxSize = new Vector3(1f, 1f, 1f);

            // Check for overlapping colliders within the box
            Collider[] overlappingColliders = Physics.OverlapBox(other.bounds.center, boxSize / 2f);

            // Iterate through overlapping colliders
            foreach (Collider overlappingCollider in overlappingColliders)
            {
                // Check if the overlapping collider is not the original collider
                if (overlappingCollider != other)
                {
                    // Do something with the overlapping collider
                    Debug.Log("Colliding with: " + overlappingCollider.gameObject.name);
                }
            }

        }
        else if (other.gameObject.CompareTag("Boss"))
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
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }

}
