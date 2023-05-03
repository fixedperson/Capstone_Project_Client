using System;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class PacketHandler
{
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = packet as S_EnterGame;
		Managers.Object.PlayerAdd(enterGamePacket.Player, myPlayer: true);
		
	}

	public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
	{
		S_LeaveGame leaveGameHandler = packet as S_LeaveGame;
	}
	
	public static void S_OtherPlayerSpawnHandler(PacketSession session, IMessage packet)
	{
		S_OtherPlayerSpawn spawnPacket = packet as S_OtherPlayerSpawn;
		foreach (PlayerInfo player in spawnPacket.Players)
		{
			Managers.Object.PlayerAdd(player, myPlayer: false);
		}
	}
	
	public static void S_EnemySpawnHandler(PacketSession session, IMessage packet)
	{
		S_EnemySpawn spawnPacket = packet as S_EnemySpawn;
		foreach (EnemyInfo enemy in spawnPacket.Enemys)
		{
			Debug.Log(enemy.PlayerId);
			Managers.Object.EnemyAdd(enemy);
		}
	}
	
	public static void S_DestroyHandler(PacketSession session, IMessage packet)
	{
		S_Destroy despawnPacket = packet as S_Destroy;
		foreach (int id in despawnPacket.PlayerIds)
		{
			Managers.Object.Remove(id);
		}
	}
	
	// 이름 변경
	public static void S_PlayerMoveHandler(PacketSession session, IMessage packet)
	{
		S_PlayerMove movePacket = packet as S_PlayerMove;
		GameObject gameObject = Managers.Object.PlayerFindById(movePacket.PlayerId);
		if (gameObject == null)
			return;
		Player player = gameObject.GetComponent<Player>();
		if (player == null)
			return;
		
		Vector3 curPos = new Vector3(movePacket.PosInfo.PosX, 0, movePacket.PosInfo.PosZ);
		Vector3 moveVec = new Vector3(movePacket.PosInfo.HAxis, 0, movePacket.PosInfo.VAxis).normalized;
		
		// 이동 관련 정보 전송 
		player.moveVec = moveVec;
		player.curPos = curPos;

		// 이동 동기화
		gameObject.transform.position = curPos;
	}

	public static void S_PlayerActionHandler(PacketSession session, IMessage packet)
	{
		S_PlayerAction actionPacket = packet as S_PlayerAction;
		GameObject gameObject = Managers.Object.PlayerFindById(actionPacket.PlayerId);
		if (gameObject == null)
			return;
		Player player = gameObject.GetComponent<Player>();
		if (player == null)
			return;
		
		// 액션 정보 전송 
		player.rDown = actionPacket.ActInfo.RDown;
		player.aDown = actionPacket.ActInfo.ADown;
	}

	public static void S_EnemyMoveHandler(PacketSession session, IMessage packet)
	{
		S_EnemyMove movePacket = packet as S_EnemyMove;
		GameObject gameObject = Managers.Object.EnemyFindById(movePacket.EnemyId);
		if (gameObject == null)
			return;
		Enemy enemy = gameObject.GetComponent<Enemy>();
		if (enemy == null)
			return;
		
		enemy.moveVec = (new Vector3(movePacket.Posinfo.PosX, 0, movePacket.Posinfo.PosZ) - enemy.transform.position).normalized;
	}

	public static void S_TimeInfoHandler(PacketSession session, IMessage packet)
	{
		S_TimeInfo timeInfoPacket = packet as S_TimeInfo;
		GameObject gameObject = GameObject.Find("EventSystem");

		Ui ui = gameObject.GetComponent<Ui>();
		ui.setTime = timeInfoPacket.Now;
	}
	
	public static void S_EndStageHandler(PacketSession session, IMessage packet)
	{
		
	}

	public static void S_HostUserHandler(PacketSession session, IMessage packet)
	{
		Managers.Object.isHostUser();
	}
}
