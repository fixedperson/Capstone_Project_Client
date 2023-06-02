using System;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
			Managers.Object.EnemyAdd(enemy);
		}
	}

	public static void S_PlayerDestroyHandler(PacketSession session, IMessage packet)
	{
		S_PlayerDestroy playerDestroy = packet as S_PlayerDestroy;
		Managers.Object.PlayerDisabled(playerDestroy.PlayerId);
	}

	public static void S_EnemyDestroyHandler(PacketSession session, IMessage packet)
	{
		S_EnemyDestroy enemyDestroy = packet as S_EnemyDestroy;
		Managers.Object.EnemyRemove(enemyDestroy.EnemyId);
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

		player.curPos = curPos;
		player.moveVec = moveVec;
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

		enemy.curPos = new Vector3(movePacket.Posinfo.PosX, 0, movePacket.Posinfo.PosZ);
		enemy.moveVec = (enemy.curPos - enemy.transform.position).normalized;
	}

	public static void S_TimeInfoHandler(PacketSession session, IMessage packet)
	{
		S_TimeInfo timeInfoPacket = packet as S_TimeInfo;
		GameObject gameObject = GameObject.Find("UIManager");

		TimerManager timerManager = gameObject.GetComponent<TimerManager>();
		timerManager.setTime = timeInfoPacket.Now;
	}

	public static void S_EndStageHandler(PacketSession session, IMessage packet)
	{
		S_EndStage endStage = packet as S_EndStage;
		Managers.Object.stageUp();
		int stage = endStage.CurStage % 3;

		if (stage == 0)
		{
			SceneManager.LoadScene("Scenes/Main1");
		}

		else if (stage == 1)
		{
			SceneManager.LoadScene("Scenes/Main2");
		}

		else
		{
			SceneManager.LoadScene("Scenes/Main3");
		}

		Time.timeScale = 1;
		Managers.Object.myStageClear(endStage.Players);
		foreach (PlayerInfo playerInfo in endStage.Otherplayers)
		{
			Managers.Object.stageClear(playerInfo);
		}

		Managers.Object.enemyClear();
		StageClearManager ui = GameObject.Find("StageClearManager").GetComponent<StageClearManager>();
		ui.gameClearScene.SetActive(false);
	}

	public static void S_GameClearHandler(PacketSession session, IMessage packet)
	{
		Time.timeScale = 0;
		GameObject gameObject = GameObject.Find("StageClearManager");
		gameObject.GetComponent<StageClearManager>().GameClearActive();
	}

	public static void S_HostUserHandler(PacketSession session, IMessage packet)
	{
		Managers.Object.isHostUser();
	}

	public static void S_EnemyTargetResetHandler(PacketSession session, IMessage packet)
	{
		S_EnemyTargetReset enemyTargetReset = packet as S_EnemyTargetReset;
		Managers.Object.EnemyTargetChange(enemyTargetReset.PlayerId, enemyTargetReset.EnemyId);
	}

	public static void S_EnemyHitHandler(PacketSession session, IMessage packet)
	{
		S_EnemyHit enemyHit = packet as S_EnemyHit;
		GameObject gameObject = Managers.Object.EnemyFindById(enemyHit.EnemyId);
		if (gameObject == null)
			return;
		Enemy enemy = gameObject.GetComponent<Enemy>();
		if (enemy == null)
			return;

		enemy.curHealth = enemyHit.CurHp;
		enemy.onHit = true;
	}

	public static void S_PlayerHitHandler(PacketSession session, IMessage packet)
	{
		S_PlayerHit playerHit = packet as S_PlayerHit;
		GameObject gameObject = Managers.Object.PlayerFindById(playerHit.PlayerId);
		if (gameObject == null)
			return;

		Player player = gameObject.GetComponent<Player>();
		player.curHealth = playerHit.CurHp;
	}

	public static void S_PlayerChatHandler(PacketSession session, IMessage packet)
	{
		S_PlayerChat playerChat = packet as S_PlayerChat;
		GameObject gameObject = GameObject.Find("ChatManager");
		if (gameObject == null)
			return;
		ChatManager chatManager = gameObject.GetComponent<ChatManager>();
		if (chatManager == null)
			return;

		string msg = string.Format("아군 : {0}", playerChat.Chat);
		chatManager.ReceiveMsg(msg);
	}

	public static void S_GameOverHandler(PacketSession session, IMessage packet)
	{
		S_GameOver gameOver = packet as S_GameOver;
		
		GameObject gameObject = GameObject.Find("GameOverManager");
		gameObject.GetComponent<GameOverManager>().StartFadein(gameOver.Stage);
	}

	public static void S_GameStartHandler(PacketSession session, IMessage packet)
	{
		SceneManager.LoadScene("Scenes/SelectScene");
	}

	public static void S_PlayerAlreadySelectedHandler(PacketSession session, IMessage packet)
	{
		GameObject gameObject = GameObject.Find("ErrorManager");
		gameObject.GetComponent<ErrorManager>().ErrorDisplay();
	}

	public static void S_MainGameStartHandler(PacketSession session, IMessage packet)
	{
		SceneManager.LoadScene("Scenes/Main1");
		Managers.Network.Send(new C_EnterGame());
	}

	public static void S_GameReadyHandler(PacketSession session, IMessage packet)
	{
		GameObject gameObject = GameObject.Find("ErrorManager");
		gameObject.GetComponent<ErrorManager>().BtnDisappear();
	}
}