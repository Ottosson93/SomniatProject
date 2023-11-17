using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;
    public float force;
    private float yDirectionOffset = 0.7f;
    private string targetTag;
    public int damage = 2;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;

        rb.velocity = new Vector3(direction.x, direction.y + yDirectionOffset, direction.z).normalized * force;
    }

    public void OnTriggerEnter(Collider other)
    {
        targetTag = "Player";
        if (other.gameObject.tag != targetTag)
            return;

        if(targetTag == "Player")
        {
            other.GetComponent<Player>().TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}
