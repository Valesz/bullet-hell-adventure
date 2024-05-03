using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject baseProjectile;
    public int projectileBufferAmount;
    static GameObject[] projectileBuffer;
    public Color initPlayerProjectileColor;
    public Color initEnemyProjectileColor;
    public static Color playerProjectileColor;
    public static Color enemyProjectileColor;

    // Start is called before the first frame update
    void Start()
    {
        projectileBuffer = new GameObject[projectileBufferAmount];
        for (int i = 0; i < projectileBuffer.Length; i++)
        {
            GameObject _tmpProjectile = Instantiate(baseProjectile, transform);
            _tmpProjectile.SetActive(false);
            projectileBuffer[i] = _tmpProjectile;
        }
        playerProjectileColor = initPlayerProjectileColor;
        enemyProjectileColor = initEnemyProjectileColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            resstart();
        }
    }

    public void resstart()
    {
        SceneManager.LoadScene(1);
    }

    public static int findNextAvailable()
    {
        for (int i = 0;i < projectileBuffer.Length;i++)
        {
            if (!projectileBuffer[i].activeInHierarchy)
            {
                return i;
            }
        }
        throw new Exception("Nincs több projectile a bufferben!");
    }

    public static GameObject spawnBullet(Vector3 position, Quaternion rotation, float damage, float speed, float lifeTime, bool shotByPlayer = false)
    {
        int available = findNextAvailable();

        projectileBuffer[available].transform.position = position;
        projectileBuffer[available].transform.rotation = rotation;

        Projectile projectileScript = projectileBuffer[available].GetComponent<Projectile>();

        projectileScript.damage = damage;
        projectileScript.speed = speed;
        projectileScript.lifeTime = lifeTime;
        projectileScript.spawnedByPlayer = shotByPlayer;

        projectileBuffer[available].GetComponent<SpriteRenderer>().color = shotByPlayer ? playerProjectileColor : enemyProjectileColor;

        projectileBuffer[available].SetActive(true);
        return projectileBuffer[available];
    }
}
