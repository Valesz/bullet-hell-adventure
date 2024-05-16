using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public float curHp;
    public bool canDamage;
    public Image hpIndicator;
    public ParticleSystem damagedParticle;
    public Color colorCanDamage;
    public Color colorCantDamage;
    float damageTimer;
    float timer;

    [Header("Phase1")]
    public float phase1percentage;
    public GameObject spawnerForPhase1;
    private GameObject spawnedSpawnerPhase1;
    private BulletSpawner spawnerScript1;
    public GameObject projectile1;
    public float rotationSpeed1;
    bool spawnedPhase1;
    
    [Header("Phase2")]
    public float phase2percentage;
    public GameObject spawnerForPhase2;
    private List<GameObject> spawnedSpawnersPhase2;
    public GameObject projectile2;
    public float rotationSpeed2;
    bool spawnedPhase2;

    [Header("Phase3")]
    public float phase3percentage;
    public GameObject spawnerForPhase3;
    public GameObject projectile3;
    public float rotationSpeed3;
    bool spawnedPhase3;


    // Start is called before the first frame update
    void Start()
    {
        curHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHp / maxHp > phase1percentage / 100)
        {
            Phase1();
        }
        else if (curHp / maxHp > phase2percentage / 100)
        {
            Phase2();
        }
        else if (curHp / maxHp > phase3percentage / 100)
        {
            Phase3();
        } else
        {
            Die();
        }

        if (damageTimer <= 0)
        {
            canDamage = true;
            UpdateHpIndicator();
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
            canDamage = false;
            damageTimer = 3;
            UpdateHpIndicator();
            spawnedSpawnerPhase1 = Instantiate(spawnerForPhase1, transform.position, Quaternion.identity);
            spawnerScript1 = spawnedSpawnerPhase1.GetComponent<BulletSpawner>();
            spawnedPhase1 = true;
        }

        if (timer % 2 < 0.001)
        {
            ShootAtPlayer(projectile1);
        }
    }

    public void Phase2()
    {
        if (!spawnedPhase2)
        {
            timer = 0;
            float distance = 10f;
            canDamage = false;
            damageTimer = 10;
            UpdateHpIndicator();
            spawnedSpawnersPhase2 = new List<GameObject>();
            spawnedSpawnersPhase2.Add(Instantiate(spawnerForPhase2, new Vector3(transform.position.x + distance, transform.position.y, 0f), Quaternion.identity, transform));
            spawnedSpawnersPhase2.Add(Instantiate(spawnerForPhase2, new Vector3(transform.position.x - distance, transform.position.y, 0f), Quaternion.identity, transform));
            spawnedSpawnersPhase2.Add(Instantiate(spawnerForPhase2, new Vector3(transform.position.x, transform.position.y + distance, 0f), Quaternion.identity, transform));
            spawnedSpawnersPhase2.Add(Instantiate(spawnerForPhase2, new Vector3(transform.position.x, transform.position.y - distance, 0f), Quaternion.identity, transform));
            spawnedPhase2 = true;
        }

        if (timer % 2 < 0.1)
        {
            ShootAtPlayer(projectile2);
            timer = 0.11f;
        }
    }

    public void Phase3()
    {
        if (!spawnedPhase3)
        {
            timer = 0;
            int i = 0;
            foreach (GameObject item in spawnedSpawnersPhase2)
            {
                BulletSpawner tmpspawnerScript = item.GetComponent<BulletSpawner>();
                tmpspawnerScript.numberOfWays = 2;
                int directionAdd = 0;
                switch (i)
                {
                    case 0: 
                        directionAdd = 180 + 90 - 30;
                        break;
                    case 1:
                        directionAdd = 0 + 90 - 30;
                        break;
                    case 2:
                        directionAdd = 270 + 90 - 30;
                        break;
                    case 3:
                        directionAdd = 90 + 90 - 30;
                        break;
                }
                    
                item.transform.localEulerAngles = new Vector3 (0f, 0f, item.transform.localEulerAngles.z + directionAdd);
                i++;
            }

            spawnerScript1.fireRate = 0.15f;
            spawnerScript1.rotationSpeed = 7f;
            spawnedPhase3 = true;
            canDamage = false;
            damageTimer = 10f;
            UpdateHpIndicator();
        }

        if (timer % 5 < 0.1)
        {
            spawnerScript1.rotationSpeed *= -1;
            timer = 0.11f;
        }
        if (timer % 2 < 0.1)
        {
            ShootAtPlayer(projectile3);
        }
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + rotationSpeed3 * Time.deltaTime);
    }

    public void ShootAtPlayer(GameObject projectile)
    {
        Vector3 tmpPos = transform.position;
        Quaternion tmpRot = Quaternion.identity;
        transform.LookAt(Player.instance.transform.position);
        Projectile projScript = projectile.GetComponent<Projectile>();
        GameObject spawnedBullet = GameManager.spawnBullet(transform.position, lookAtPlayer(), projScript.damage, projScript.speed, projScript.lifeTime);
        spawnedBullet.transform.localScale = projectile.transform.localScale;
        transform.position = tmpPos;
        transform.rotation = tmpRot;
    }

    public Quaternion lookAtPlayer()
    {
        var dir = Player.instance.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void changeHp(float value)
    {
        if (canDamage)
        {
            curHp = Mathf.Min(maxHp, Mathf.Max(0, curHp + value));
            UpdateHpIndicator();
            if (value < 0)
            {
                damagedParticle.Emit(5);
            }
            if (curHp <= 0) 
            {
                Die();
            }
        }
    }

    public void UpdateHpIndicator()
    {
        hpIndicator.fillAmount = curHp / maxHp;
        hpIndicator.color = canDamage ? colorCanDamage : colorCantDamage;
    }

    public void Die()
    {
        GameManager.OpenGameOverPanel();
        spawnedSpawnerPhase1.SetActive(false);
        gameObject.SetActive(false);
    }
}
