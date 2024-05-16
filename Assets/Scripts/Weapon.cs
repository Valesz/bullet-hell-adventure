using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapon : MonoBehaviour
{

    [SerializeField]Sprite sprite;
    [SerializeField]Player player;
    public ParticleSystem shootParticle;
    public Transform frontOfWeapon;
    public GameObject projectile;
    public float projectileSpeed;
    public float projectileLifeTime;
    public float fireRate;
    public float accuracyDeg;
    public float damage;
    float timeSinceShoot;

    Weapon(Player player)
    {
        this.player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        if (Input.GetMouseButton(0))
        {
            Shoot();

        }

        FlipSprite();
    }

    void LookAtMouse()
    {
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = -10f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shoot()
    {
        if (timeSinceShoot > fireRate)
        {
            float randomZ = Random.Range(-accuracyDeg, +accuracyDeg);
            Quaternion direction = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + randomZ);
            GameObject _projectile = GameManager.spawnBullet(frontOfWeapon.position, transform.rotation, damage, projectileSpeed, projectileLifeTime, true);
            _projectile.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + Random.Range(-accuracyDeg, accuracyDeg));
            timeSinceShoot = 0;
            shootParticle.Emit(5);
        }
        timeSinceShoot += Time.deltaTime;

    }

    public void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void FlipSprite()
    {
        if (player.facingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        } else
        {
            transform.localScale = new Vector3(-1, -1, 1);
        }
    }
}
