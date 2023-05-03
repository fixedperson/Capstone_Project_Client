using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int enemyId;
    
    public float maxHealth;

    public float curHealth;

    private Rigidbody rigid;
    private Animator anim;
    private BoxCollider boxCollider;

    public Vector3 moveVec;
    public bool isHit;
    public bool isAttackReady = true;
    public bool isAttack;
    public bool isDelay;

    public Player player;
    public float moveSpeed;
    public float attackRange = 3;
    public float attackDelay = 2;
    public int attackDamage;

    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset;
    
    private Canvas hpCanvas;
    private GameObject hpBar;
    private Slider hpSlider;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        HPbarInit();
    }

    // Update is called once per frame
    void Update()
    {
        // Attack();
        Move();

        if (Managers.Object.getHostUser())
        {
            SendMovePacket();
        }
        
        if (curHealth <= 0)
        {
            //Destroy();
        }

        if (maxHealth == curHealth) hpBar.SetActive(false);
        else
        {
            hpBar.SetActive(true);
            HpSliderUpdate();
        }
    }

    void SendMovePacket()
    {
        if (!isDelay)
        {
            C_EnemyMove enemyMove = new C_EnemyMove();
            EnemyPositionInfo posInfo = new EnemyPositionInfo();
            posInfo.PosX = transform.position.x;
            posInfo.PosZ = transform.position.z;
            enemyMove.Posinfo = posInfo;
            enemyMove.EnemyId = enemyId;
            Managers.Network.Send(enemyMove);

            StopCoroutine("SendDelay");
            StartCoroutine("SendDelay", 0.1f);
        }
    }
    
    IEnumerator SendDelay(float time)
    {
        isDelay = true;
        yield return new WaitForSeconds(time);
        
        isDelay = false;
    }

    void Move()
    {
        if (!isHit && !isAttack)
        {
            moveVec = (player.transform.position - transform.position).normalized;
            transform.LookAt(player.transform);
            rigid.MovePosition(transform.position + moveSpeed * Time.fixedDeltaTime * moveVec);
        }
    }

    // attackRange(사거리)가 플레이어와 적 위치 차이 벡터보다 클 시 공격 가능
    void Attack()
    {
        if (!isHit && isAttackReady)
        {
            anim.SetTrigger("isAttack");
            player.Hit(attackDamage);
            
            StopCoroutine("Attacking");
            StartCoroutine("Attacking", 0.5f);
            StartCoroutine(AttackDelay());
        }
    }
    
    bool InRange()
    {
        return (transform.position - player.transform.position).magnitude <= attackRange;
    }
    
    // 공격 도중 재공격 및 다른 모션 불가
    IEnumerator Attacking(float time)
    {
        isAttack = true;
        yield return new WaitForSeconds(time);
        
        isAttack = false;
    }
    
    // 공격 딜레이
    IEnumerator AttackDelay()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(attackDelay);
    
        isAttackReady = true;
    }
    
    // Hp가 0이 될 시 Destroy, 죽는 애니메이션 실행 및 콜라이더 제거 후 일정 시간뒤에 삭제(추가 예정)
    void Destroy()
    {
        Destroy(hpBar);
        GameObject.Destroy(gameObject);
    }
    
    // 피격 시 웨폰의 최근 공격 리스트에 추가
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            if (!weapon.recentDamageList.Contains(this)) {
                anim.SetTrigger("isHit");
                weapon.recentDamageList.Add(this);
                curHealth -= weapon.damage;
    
                StopCoroutine("Hit");
                StartCoroutine("Hit");
            }
        }
    }
    
    // 피격도중 다른 모션 불가
    IEnumerator Hit()
    {
        isHit = true;
        yield return new WaitForSeconds(1.0f);
        
        isHit = false;
    }

    void HPbarInit()
    {
        hpCanvas = GameObject.Find("HP Canvas").GetComponent<Canvas>();
        hpBar = Instantiate<GameObject>(hpBarPrefab, hpCanvas.transform);
        hpSlider = hpBar.GetComponentInChildren<Slider>();

        var hpbar = hpBar.GetComponent<HPbar>();
        hpbar.targetTr = gameObject.transform;
        hpbar.offset = hpBarOffset;
    }

    void HpSliderUpdate()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, curHealth / maxHealth, Time.deltaTime * 10);
    }
}
