using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyPlayer : Player
{
    public int id;
    
    public string name;
    public float rollCoolTime = 3.0f;
    public float attackDelay = 0.3f; // 공격 딜레이

    public bool isRollReady = true; // 구르기 가능 여부
    public bool isAttackReady = true; // 공격 가능 여부

    private PlayerPositionInfo _positionInfo = new PlayerPositionInfo();
    private PlayerActionInfo _actionInfo = new PlayerActionInfo();
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Roll();
        Attack();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetKeyDown(KeyCode.Space);
        aDown = Input.GetMouseButton(0);
    }
    
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        // 구르기중이라면 방향 고정
        if (isRoll)
        {
            moveVec = rollVec;
        }

        // 공격중이라면 이동 불가
        else if (isAttack)
        {
            moveVec = Vector3.zero;
        }
        
        anim.SetBool("isRun", moveVec != Vector3.zero);
        rigid.MovePosition(transform.position + moveSpeed * Time.fixedDeltaTime * moveVec);
        
        // 이동 패킷 전송 
        SendMovePacket();
    }
    
    // 이동 패킷 전송
    void SendMovePacket()
    {
        C_PlayerMove movePacket = new C_PlayerMove();
        _positionInfo.PosX = transform.position.x;
        _positionInfo.PosZ = transform.position.z;
        _positionInfo.HAxis = hAxis;
        _positionInfo.VAxis = vAxis;
        movePacket.PosInfo = _positionInfo;
        Managers.Network.Send(movePacket);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Roll()
    {
        if (rDown && !isRoll && isRollReady && !isAttack && !isHit)
        {
            SendActionPacket();
            rollVec = moveVec; // 구르기시 방향 저장
            
            anim.SetTrigger("doRoll");
            
            isRoll = true;
            isRollReady = false;
            
            StartCoroutine(RollCoolTime(rollCoolTime));
            StartCoroutine(Rolling(0.833f));
        }
    }

    // 구르기 도중 이동속도 2배, 방향전환 불가능 기능
    IEnumerator Rolling(float time)
    {
        moveSpeed *= 1.5f;
        yield return new WaitForSeconds(time);
        
        isRoll = false;
        moveSpeed /= 1.5f;
    }

    // 구르기 쿨타임 적용
    IEnumerator RollCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        
        isRollReady = true;
    }

    void Attack()
    {
        if (aDown && isAttackReady && moveVec == Vector3.zero && !isRoll && !isHit && !isInvincible)
        {
            SendActionPacket();
            anim.SetTrigger("comboAttack");
            
            StopCoroutine("Attacking");
            StartCoroutine("Attacking", 0.5f);
            StartCoroutine(AttackDelay(attackDelay));
            
            //equipWeapon.Use();
        }
    }
    
    // 액션 패킷 전송
    void SendActionPacket()
    {
        C_PlayerAction actionPacket = new C_PlayerAction();
        _actionInfo.ADown = aDown;
        _actionInfo.RDown = rDown;
        actionPacket.ActInfo = _actionInfo;
        Managers.Network.Send(actionPacket);
    }
    // 공격 도중 재공격 및 다른 모션 불가
    IEnumerator Attacking(float time)
    {
        isAttack = true;
        yield return new WaitForSeconds(time);
        
        isAttack = false;
    }
    
    // 공격 딜레이
    IEnumerator AttackDelay(float time)
    {
        isAttackReady = false;
        yield return new WaitForSeconds(time);
        
        isAttackReady = true;
    }
    
}