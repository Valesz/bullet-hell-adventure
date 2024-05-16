using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [Header("Stats")]
    public float curHp;
    public float maxHp;
    public Image hpIndicator;
    public ParticleSystem damagedParticle;

    [Header("Movement")]
    public float movementSpeed;
    public bool facingRight = true;

    [Header("Weapon")]
    public Weapon weapon;

    [Header("Animation")]
    public Animator animator;

    [Header("Misc")]
    public static Player instance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMovementDirection();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Move();
        }
    }

    void Move()
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (movementVector.magnitude > 1)
        {
            rb.velocity = movementVector.normalized * movementSpeed * Time.fixedDeltaTime;
        } else
        {
            rb.velocity = movementVector * movementSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            animator.SetBool("Run", true);
        } else
        {
            animator.SetBool("Run", false);
        }
    }

    void LookAtMovementDirection()
    {
        facingRight = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x > 0;
        if (facingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        } else
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }
    }

    public void ChangeHp(float value) {
        curHp = Mathf.Min(maxHp, Mathf.Max(0, curHp + value));
        UpdateHpIndicator();
        if (value < 0)
        {
            damagedParticle.Emit(30);
        }
        if (curHp <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    public void UpdateHpIndicator()
    {
        hpIndicator.fillAmount = curHp / maxHp;
    }

    public void Die()
    {
        GameManager.OpenGameOverPanel();
        gameObject.SetActive(false);
    }
}
