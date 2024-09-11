using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("����ֵ���")]
    public float HP;
    public float maxHp;
    [Header("�������")]
    public float followDistance_x;
    public float followDistance_y;
    public float MinDistance;
    [Header("�ٶ����")]
    public float enemyMoveSpeed;
    [Header("�ܻ����")]
    public float enemyHitSpeed;
    public bool isHit;
    [Header("����ʱ")]
    public float AttackTime = 0;
    public float HitTime = 1;
    [Header("�ж����")]
    [Header("������")]
    public Vector2 direction;
    public Transform target;
    private Animator animator;
    public Rigidbody enemyRB;

    void Start()
    {
        HP = maxHp;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = gameObject.GetComponent<Animator>();
        enemyRB = gameObject.GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (HP>0)
        {
            FollowPlayer();
        }
        else
        {
            EnemyDie();
        }      
    }

    void FollowPlayer()
    {
        var Distance_x = Mathf.Abs(transform.position.x - target.position.x);
        var Distance_y = Mathf.Abs(transform.position.y - target.position.y);
        if (Distance_x < followDistance_x && Distance_x > MinDistance &&  3 > Distance_y)
        {
            animator.SetBool("_walk" , true);
            transform.position = Vector2.MoveTowards(transform.position, target.position, enemyMoveSpeed * Time.deltaTime);
            LookPlayer();
        }
        else if(Distance_x < followDistance_x && Distance_x < MinDistance && 3 > Distance_y)
        {
            LookPlayer();
            animator.SetBool("_walk", false);
            AttackTime += 1 * Time.deltaTime;
            animator.SetFloat("_Attack", AttackTime);
            if(AttackTime >= 4)
            {
                AttackTime = 0;
            }
        }
        else
        {
            animator.SetBool("_walk", false);
        }
    }

    void LookPlayer()
    {
        if (transform.position.x - target.position.x > 0) transform.eulerAngles = new Vector3(0, 180, 0);
        if (transform.position.x - target.position.x < 0) transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void EnemyHit()
    {
        bool hittime = true;
        if(hittime) HitTime -= Time.deltaTime;
        if(HitTime < 0) isHit=true; else isHit=false;
    }

    public void GetEnemyHit(Vector2 direction)
    {
        if (!isHit)
        {
            transform.localScale = new Vector3 (direction.x * 4 , 4 , 3 );
            HitTime = 1;
            HP -= 20;
            animator.SetTrigger("_Hit");
        }
    }

    void EnemyDie()
    {
        animator.SetBool("_Del" , true);
        Invoke("Dlete", 5f);
    }

    void Dlete()
    {
        Destroy(gameObject);
    }
}
