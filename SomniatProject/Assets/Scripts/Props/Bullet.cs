using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public enum Type
    {
        Player,
        Enemy,
        DestructibleObject,
        Obstacle
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
            case Type.DestructibleObject:
                targetTag = "DestructibleObject";
                break;
            case Type.Obstacle:
                targetTag = "Obstacle";
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != targetTag)
            return;

        if (targetTag == "Player")
            other.GetComponent<Player>().TakeDamage(damage);
        else if (targetTag == "Enemy")
            other.GetComponent<Enemy>().TakeDamage(damage);

        if (other.gameObject.layer == LayerMask.NameToLayer("DestructibleObject"))
            other.GetComponent<ExplosiveObject>().TakeDamage(damage);
        else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            Destroy(gameObject);


        gameObject.SetActive(false);
    }

}

