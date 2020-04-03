using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Private health value.-Float
    /// </summary>
    private float health = 100.0f;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject enemyBody;

    private FieldOfView fov;

    [Range(0, 1)]
    [SerializeField]
    private float roationSpeed;


    [SerializeField]
    private Animator animator;

    private bool canDie;

    private bool isDying;

    private void Start()
    {
        fov = GetComponentInChildren<FieldOfView>();
        canDie = false;
        isDying = false;
    }
    private void Update()
    {
        Die();

        if (fov.playerSpotted && !isDying)
            Shoot();
    }
    public void TakeDamage(float _amount)
    {
        health -= _amount;
    }

    private void Die()
    {
        if (health <= 0)
        {
            isDying = true;

            if(!canDie)
             animator.SetTrigger("die");

            StartCoroutine(DieTimer());
            if(canDie)
                Destroy(enemyBody.gameObject);
        }
    }

    private IEnumerator DieTimer()
    {
        yield return new WaitForSeconds(5.0f);
        canDie = true;
    }

    private void Shoot()
    {   
        Quaternion lookOnLook = Quaternion.LookRotation(player.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, roationSpeed);

    }

}
