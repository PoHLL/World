using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play1_Manager : MonoBehaviour
{
    [Header("速度相关")]
    public float Speed = 10;
    public float Jump_Speed = 15;
    [Header("跳跃次数")]
    public float PlayerJumpCount;
    [Header("可攻击次数")]
    public float PlayerJumpAttack = 1;
    [Header("倍率有关")]
    public float jumpMultiplier = 1;
    [Header("时间相关")]
    public float croushTime;
    public float AttackTime = 1;
    [Header("判断相关")]
    public bool Attack_yn;
    public bool isGround;
    public bool isJump;
    public bool pressedJump;
    public bool isCrouch;
    public bool pressedCrouch;
    public bool isAttack;
    public bool isAttack_1;
    [Header("碰撞体相关")]
    private CapsuleCollider2D m_CapsuleCollider2D;
    private BoxCollider2D[] attackColl;
    public Vector2 playeroffsetVector;
    public Vector2 playerSizeVector;
    [Header("其他")]
    public Transform foot;
    public LayerMask Ground;
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody2D;


    void Start()
    {
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        m_Animator = transform.Find("Play_1").GetComponent<Animator>();
        attackColl = transform.GetComponentsInChildren<BoxCollider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        playeroffsetVector = new Vector2(m_CapsuleCollider2D.offset.x, m_CapsuleCollider2D.offset.y);
        playerSizeVector = new Vector2(m_CapsuleCollider2D.size.x , m_CapsuleCollider2D.size.y);
    }

    private void Update()
    {
        UpdateCheck();
        if(isGround) PlayerAttack();
        animator_date();
    }

    void FixedUpdate()
    {
        YNisGround();
        PlayerAttack_2();
        if (!Attack_yn)
        {
            Move_1();
            PlayerJump();
            PlayerCrouch();
        }
        isGround = Physics2D.OverlapCircle(foot.position , 0.1f , Ground);
    }

    #region 左右移动
    private void Move_1()
    {
        var Horizontal_x = Input.GetAxis("Horizontal");
        var faceNum = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Speed = 20;
        }
        else
        {
            Speed = 10;
        }
        transform.position = new Vector2(y: transform.position.y, x: transform.position.x + Horizontal_x * Speed * Time.deltaTime);
        if (faceNum != 0)
        {
            transform.localScale = new Vector3(faceNum * 1, 1, 1);
        }
    }
    #endregion

    #region 动画
    private void animator_date()
    {
        var faceNum = Input.GetAxisRaw("Horizontal");
        if(isGround) m_Animator.SetFloat("_Run" , Mathf.Abs(Speed * faceNum));
        m_Animator.SetBool("_Jump", !isGround);
        m_Animator.SetBool("_Croush", isCrouch);       
        if(isAttack) m_Animator.SetTrigger("_Attack");
        if(isAttack_1) m_Animator.SetTrigger("_Attack_2");
    }

    #endregion

    #region 角色的跳跃
    private void PlayerJump()
    {
        if (pressedJump && PlayerJumpCount > 0 && isGround)
        {
            m_Rigidbody2D.velocity = new Vector2(x:m_Rigidbody2D.velocity.x, y:Jump_Speed * jumpMultiplier);
            PlayerJumpCount--;
            pressedJump = false;
        }
        if(pressedJump && PlayerJumpCount > 0 && !isGround)
        {
            m_Rigidbody2D.velocity = new Vector2(x: m_Rigidbody2D.velocity.x, y: Jump_Speed);
            PlayerJumpCount--;
            pressedJump = false;
        }

        
    }
    #endregion

    #region 跳跃、蹲下判定
    void UpdateCheck()
    {
        if (Input.GetButton("Croush"))
        {
            pressedCrouch = true;
        }
        else
        {
            pressedCrouch = false;
        }
        if (Input.GetButtonDown("Jump") && PlayerJumpCount > 0)
        {
            pressedJump = true;
        }
    }
    #endregion

    #region 下蹲
    void PlayerCrouch()
    {
        if (pressedCrouch  && isGround)
        {
            isCrouch = true;
            m_CapsuleCollider2D.size = new Vector2 (playerSizeVector.x, playerSizeVector.y * 0.5f);
            m_CapsuleCollider2D.offset = new Vector2 (playeroffsetVector.x, playeroffsetVector.y * 0.5f);
            Speed = 2;
        }
        else
        {
            isCrouch = false;
            m_CapsuleCollider2D.size = new Vector2(playerSizeVector.x, playerSizeVector.y);
            m_CapsuleCollider2D.offset = new Vector2(playeroffsetVector.x, playeroffsetVector.y);
            Speed = 4;
        }

        if (isCrouch)
        {
            croushTime += Time.deltaTime;
            if (croushTime >= 2)
            {
                croushTime = 2;
            }
            jumpMultiplier = 1 + croushTime * 1.25f;
        }
        else
        {
            croushTime = 0;
            jumpMultiplier = 1;
        }
    }
    #endregion

    #region 攻击系统
    void PlayerAttack()
    {
        if(Attack_yn) AttackTime -= Time.deltaTime;
        if (AttackTime <= 0) Attack_yn = false; else Attack_yn = true;
        if(Input.GetMouseButtonDown(0) && AttackTime <= 0)
        {
            AttackTime = 1;
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
    }

    void PlayerAttack_2()
    {
        if (!isGround)
        {
            if (Input.GetMouseButton(0) && PlayerJumpAttack > 0)
            {
                PlayerJumpAttack--;
                isAttack_1 = true;
            }
            else
            {
                isAttack_1 = false;
            }
        }
    }
    #endregion

    #region 攻击判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (transform.localScale.x > 0)
            collision.GetComponent<Enemy>().GetEnemyHit(Vector2.right);
            else if (transform.localScale.x < 0)
            collision.GetComponent<Enemy>().GetEnemyHit(Vector2.left);
            Debug.Log("检测到敌人");
        }
    }
    #endregion

    #region 落地判定
    private void YNisGround()
    {
        if (isGround)
        {
            PlayerJumpCount = 1;
            PlayerJumpAttack = 1;
            isJump = false;
        }
    }

    #endregion
}
