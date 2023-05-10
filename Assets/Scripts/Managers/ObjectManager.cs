using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager
{
	// 플레이어 리스트 
	Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
	
	// 몬스터 리스트 
	Dictionary<int, GameObject> _enemys = new Dictionary<int, GameObject>();

	private bool hostUser = false;

	// 몬스터와 플레이어 Add 함수 구분
	public void PlayerAdd(PlayerInfo playerInfo, bool myPlayer = false)
	{
		if (myPlayer)
		{
			GameObject gameObject = Managers.Resource.Instantiate("Player/HeroPlayer");
			PlayerCamera.targetTransform = gameObject.transform;
			MyPlayer mp = gameObject.GetComponent<MyPlayer>();

			mp.transform.position = new Vector3(playerInfo.PosInfo.PosX, 0 , playerInfo.PosInfo.PosZ);
			mp.id = playerInfo.PlayerId;
			mp.enabled = true;
			mp.moveSpeed = 5;
			mp.maxHealth = 100;
			mp.curHealth = 100;
			mp.equipWeapon = GameObject.FindWithTag("Sword").GetComponent<Weapon>();
			gameObject.name = playerInfo.Name;
			_players.Add(playerInfo.PlayerId, gameObject);
			
			UIStatus ui = GameObject.Find("Red").GetComponent<UIStatus>();
			ui.myPlayer = mp;
		}
		else
		{
			GameObject gameObject = Managers.Resource.Instantiate("Player/MC01");
			gameObject.name = playerInfo.Name;
			Player player = gameObject.GetComponent<Player>();
			player.transform.position = new Vector3(playerInfo.PosInfo.PosX, 0 , playerInfo.PosInfo.PosZ);
			player.enabled = true;
			player.moveSpeed = 5;
			player.maxHealth = 100;
			player.curHealth = 100;
			_players.Add(playerInfo.PlayerId, gameObject);
		}
	}

	public void EnemyAdd(EnemyInfo enemyInfo)
	{
		GameObject gameObject = Managers.Resource.Instantiate("Enemy/Bat");
		Enemy enemy = gameObject.GetComponent<Enemy>();
		enemy.enemyId = enemyInfo.EnemyId;
		enemy.enabled = true;
		enemy.moveSpeed = 3;
		enemy.maxHealth = 100;
		enemy.curHealth = enemy.maxHealth;
		enemy.attackRange = 3;
		enemy.attackDelay = 2;
		enemy.attackDamage = 10;
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
		GameObject gameObject = _players[id];
		gameObject.SetActive(false);
	}
	
	public void EnemyRemove(int id)
	{
		GameObject gameObject = _enemys[id];
		gameObject.GetComponent<Enemy>().Die();
		_enemys.Remove(id);
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
}
