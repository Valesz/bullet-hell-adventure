using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public float curHp;
    public bool canDamage;
    float damageTimer;
    [SerializeField]float timer;
    
    [Header("Phase1")]
    public GameObject spawnerForPhase1;
    public GameObject projectile;
    public float rotationSpeed;
    bool spawnedPhase1;


    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHp / maxHp > 0.7f)
        {
            Phase1();
        } else if (curHp / maxHp <= 0.7f)
        {
            
        }
        if (damageTimer <= 0)
        {
            canDamage = true;
        } else
        {
            damageTimer -= Time.deltaTime;
        }
        timer += Time.deltaTime;
    }

    public void Phase1()
    {
        if (!spawnedPhase1)
        {
            float distance = 10f;
            Instantiate(spawnerForPhase1, new Vector3(transform.position.x + distance, transform.position.y, 0f), Quaternion.identity, transform);
            Instantiate(spawnerForPhase1, new Vector3(transform.position.x - distance, transform.position.y, 0f), Quaternion.identity, transform);
            Instantiate(spawnerForPhase1, new Vector3(transform.position.x, transform.position.y + distance, 0f), Quaternion.identity, transform);
            Instantiate(spawnerForPhase1, new Vector3(transform.position.x, transform.position.y - distance, 0f), Quaternion.identity, transform);
            spawnedPhase1 = true;
            canDamage = false;
            damageTimer = 20f;
        }
        //if (timer % 2 < 0.1)
        //{
        //    ShootAtPlayer();
        //}
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + rotationSpeed * Time.deltaTime);
    }

    public void ShootAtPlayer()
    {

        Instantiate(projectile, transform.position, Quaternion.LookRotation(Vector3.forward, Player.instance.transform.position));
    }

    public void changeHp(float value)
    {
        if (canDamage)
        {
            curHp = Mathf.Min(maxHp, Mathf.Max(0, curHp + value));
            if (curHp <= 0) 
            {
                Die();
            }
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
