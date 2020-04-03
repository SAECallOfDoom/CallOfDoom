using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    /// <summary>
    /// Private animator reference.-Animator
    /// </summary>
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// Private crosshair image reference.-Image
    /// </summary>
    [SerializeField]
    private Image crosshair;

    /// <summary>
    /// Private attackpoint transfrom reference.-Transform
    /// </summary>
    [SerializeField]  
    public Transform attackPoint;

    /// <summary>
    /// Private attack range.-Float
    /// </summary>
    [SerializeField]
    private float attackRange = 10F;

    /// <summary>
    /// Private. Determines how fast we can shoot.-Float
    /// </summary>
    private float basicAttackFireRate = 0.7f;

    private float heavyAttackFireRate = 1.45f;

    private BladeSliceMesh sliceMesh;


    /// <summary>
    /// Private. Stores time to check if we can shoot.-Float
    /// </summary>
    private float nextFire = 0.0f;



    /// <summary>
    /// Private enemies layermasks.-Layermask
    /// </summary>
    [SerializeField]
    private LayerMask enemyMasks;

    [SerializeField]
    private float basicAttackDamage;

    [SerializeField]
    private float heavyAttackDamage;

    private void Start()
    {
        sliceMesh = GetComponentInParent<BladeSliceMesh>();
        crosshair.gameObject.SetActive(false);
    }
    void Update()
    {
        if(!sliceMesh.isBladeMode)
        {

            if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire)
            {
                nextFire = Time.time + basicAttackFireRate;
                Attack(true);
            }

            if (Input.GetKey(KeyCode.Mouse1) && Time.time > nextFire)
            {
                nextFire = Time.time + heavyAttackFireRate;
                Attack(false);
            }
        }
    }

    private void Attack(bool _isBasicAttack)
    {
        if(_isBasicAttack)
        {
            animator.SetTrigger("basicAttack");

            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, enemyMasks);

            foreach (Collider enemy in hitEnemies)
            {
                Enemy myEnemy = enemy.GetComponentInParent<Enemy>();
                if(myEnemy != null)
                    myEnemy.TakeDamage(basicAttackDamage);
                
            }

        }
        else
        {
            animator.SetTrigger("heavyAttack");

            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, enemyMasks);

            foreach (Collider enemy in hitEnemies)
            {
                Enemy myEnemy = enemy.GetComponentInParent<Enemy>();
                if (myEnemy != null)
                    myEnemy.TakeDamage(heavyAttackDamage);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

