using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    public bool rotate;
    public float numberOfWays;
    public float rotationSpeed;
    public float fireRate;
    float curFireRateTimer;
    public float bullettSpeed;
    public float bullettLifeTime;
    public Vector2 bullettSize;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (curFireRateTimer >= fireRate)
        {
            Fire();
            curFireRateTimer = 0;
        }
        curFireRateTimer += Time.deltaTime;
    }

    void Fire()
    {
        if (rotate)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotationSpeed);
        }
        for (int i = 0; i < numberOfWays; i++)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 360 / numberOfWays);
            GameObject tmp = GameManager.spawnBullet(transform.position, transform.rotation, damage, bullettSpeed, bullettLifeTime);
            tmp.transform.localScale = new Vector3(bullettSize.x, bullettSize.y, 0);
        }
    }
}
