using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool spawnedByPlayer;
    public float damage;
    public float speed;
    public float lifeTime;
    float _curLifeTime;

    private void OnEnable()
    {
        _curLifeTime = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_curLifeTime >= lifeTime)
        {
            gameObject.SetActive(false);
        }
        _curLifeTime += Time.deltaTime;
        Move();
    }

    void Move()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spawnedByPlayer)
        {
            if (collision.CompareTag("Player"))
            {
                Player.instance.ChangeHp(-damage);
                gameObject.SetActive(false);
            }
        } else
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy>().changeHp(-damage);
                gameObject.SetActive(false);
            }
        }
    }
}
