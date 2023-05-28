using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager
{
	enum Character
	{
		OHS,
		THS
	}

	enum EnemyType
	{
		Bat,
		TurtleShell,
		Skeleton,
		Spider,
		Golem,
		Orc
	}
	
	// 플레이어 리스트 
	Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
	
	// 몬스터 리스트 
	Dictionary<int, GameObject> _enemys = new Dictionary<int, GameObject>();

	private bool hostUser = false;

	private int stage = 0;

	// 몬스터와 플레이어 Add 함수 구분
	public void PlayerAdd(PlayerInfo playerInfo, bool myPlayer = false)
	{
		if (myPlayer)
		{
			GameObject gameObject = null;
			MyPlayer mp = null;
			
			if (playerInfo.PlayerType == (int)Character.OHS)
			{
				gameObject = Managers.Resource.Instantiate("Player/MC01_1");
				mp = gameObject.GetComponent<MyPlayer>();
				mp.equipWeapon = GameObject.FindWithTag("OHS").GetComponent<Weapon>();
				mp.equipWeapon.damage = 30;
				mp.maxHealth = 150;
				mp.curHealth = 150;
				mp.moveSpeed = 5;
			}
			else if(playerInfo.PlayerType == (int)Character.THS)
			{
				gameObject = Managers.Resource.Instantiate("Player/MC15_1");
				mp = gameObject.GetComponent<MyPlayer>();
				mp.equipWeapon = GameObject.FindWithTag("THS").GetComponent<Weapon>();
				mp.equipWeapon.damage = 50;
				mp.maxHealth = 100;
				mp.curHealth = 100;
				mp.moveSpeed = 5;
			}
			
			PlayerCamera.targetTransform = gameObject.transform;

			mp.transform.position = new Vector3(playerInfo.PosInfo.PosX, 0 , playerInfo.PosInfo.PosZ);
			mp.id = playerInfo.PlayerId;
			mp.enabled = true;
			gameObject.name = playerInfo.Name;
			_players.Add(playerInfo.PlayerId, gameObject);
			
			UIStatus ui = GameObject.Find("UI Status").GetComponent<UIStatus>();
			ui.myPlayer = mp;
		}
		else
		{
			GameObject gameObject = null;
			Player player = null;
			
			if (playerInfo.PlayerType == (int)Character.OHS)
			{
				gameObject = Managers.Resource.Instantiate("Player/MC01");
				player = gameObject.GetComponent<Player>();
				player.maxHealth = 150;
				player.curHealth = 150;
				player.moveSpeed = 5;
			}
			else if(playerInfo.PlayerType == (int)Character.THS)
			{
				gameObject = Managers.Resource.Instantiate("Player/MC15");
				player = gameObject.GetComponent<Player>();
				player.maxHealth = 100;
				player.curHealth = 100;
				player.moveSpeed = 5;
			}
			
			gameObject.name = playerInfo.Name;
			player.transform.position = new Vector3(playerInfo.PosInfo.PosX, 0 , playerInfo.PosInfo.PosZ);
			player.enabled = true;
			_players.Add(playerInfo.PlayerId, gameObject);
		}
	}

	public void EnemyAdd(EnemyInfo enemyInfo)
	{
		GameObject gameObject = null;
		if (enemyInfo.Type == (int)EnemyType.Bat)
		{
			gameObject = Managers.Resource.Instantiate("Enemy/Bat");
		}
		else if (enemyInfo.Type == (int)EnemyType.Golem)
		{
			gameObject = Managers.Resource.Instantiate("Enemy/Golem");
		}
		else if (enemyInfo.Type == (int)EnemyType.Orc)
		{
			gameObject = Managers.Resource.Instantiate("Enemy/Orc");
		}
		else if (enemyInfo.Type == (int)EnemyType.Skeleton)
		{
			gameObject = Managers.Resource.Instantiate("Enemy/Skeleton");
		}
		else if (enemyInfo.Type == (int)EnemyType.Spider)
		{
			gameObject = Managers.Resource.Instantiate("Enemy/Spider");
		}
		else
		{
			gameObject = Managers.Resource.Instantiate("Enemy/TurtleShell");
		}

		Enemy enemy = gameObject.GetComponent<Enemy>();
		enemy.enemyId = enemyInfo.EnemyId;
		enemy.enabled = true;
		enemy.moveSpeed = 5;
		enemy.maxHealth = 100 + stage * 10;
		enemy.curHealth = enemy.maxHealth;
		enemy.attackRange = 3;
		enemy.attackDelay = 2;
		enemy.attackDamage = 10 + stage * 2;
		_players.TryGetValue(enemyInfo.PlayerId, out GameObject target);
		enemy.player = target.GetComponent<Player>();
		enemy.transform.position = new Vector3(enemyInfo.PosInfo.PosX, 0, enemyInfo.PosInfo.PosZ);
		_enemys.Add(enemyInfo.EnemyId, gameObject);
	}

	public void EnemyTargetChange(int playerId, int enemyId)
	{
		_players.TryGetValue(playerId, out GameObject target);
		_enemys.TryGetValue(enemyId, out GameObject gameObject);
		Enemy enemy = gameObject.GetComponent<Enemy>();
		enemy.player = target.GetComponent<Player>();
	}

	public void PlayerDisabled(int id)
	{
		_players.TryGetValue(id, out GameObject gameObject);
		gameObject.GetComponent<Player>().disabled = true;
		gameObject.SetActive(false);
	}
	
	public void EnemyRemove(int id)
	{
		_enemys.TryGetValue(id, out GameObject gameObject);
		_enemys.Remove(id);
		gameObject.GetComponent<Enemy>().Die();
	}

	public GameObject PlayerFindById(int id)
	{
		_players.TryGetValue(id, out GameObject gameObject);
		return gameObject;
	}

	public GameObject EnemyFindById(int id)
	{
		_enemys.TryGetValue(id, out GameObject gameObject);
		return gameObject;
	}

	public void isHostUser()
	{
		hostUser = true;
	}

	public bool getHostUser()
	{
		return hostUser;
	}

	public void cameraTargetChange(Player p)
	{
		foreach (var v in _players)
		{
			if (v.Value.GetComponent<Player>() != p)
			{
				PlayerCamera.targetTransform = v.Value.transform;
			}
		}
	}

	public void myStageClear(PlayerInfo playerInfo)
	{
		_players.TryGetValue(playerInfo.PlayerId, out GameObject gameObject);
		gameObject.transform.position = new Vector3(playerInfo.PosInfo.PosX, 0, playerInfo.PosInfo.PosZ);
		MyPlayer player = gameObject.GetComponent<MyPlayer>();
		if (!player.disabled) player.curHealth = player.maxHealth;
	}

	public void stageClear(PlayerInfo playerInfo)
	{
		_players.TryGetValue(playerInfo.PlayerId, out GameObject gameObject);
		gameObject.transform.position = new Vector3(playerInfo.PosInfo.PosX, 0, playerInfo.PosInfo.PosZ);
		Player player = gameObject.GetComponent<Player>();
		if(!player.disabled) player.curHealth = player.maxHealth;
	}

	public void enemyClear()
	{
		_enemys.Clear();
	}

	public void stageUp()
	{
		stage++;
	}
}
