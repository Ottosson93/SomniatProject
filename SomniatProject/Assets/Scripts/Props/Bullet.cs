using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bullet : MonoBehaviour
{

    public enum Type
    {
        Player,
        Enemy
    };
    public Type type;
    private string targetTag;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case Type.Player:
                targetTag = "Enemy";
                break;
            case Type.Enemy:
                targetTag = "Player";
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != targetTag)
            return;

        if (targetTag == "Player")
            other.GetComponent<Player>().TakeDamage(damage);
        else
            other.GetComponent<Enemy>().TakeDamage(damage);


        gameObject.SetActive(false);
    }
}

