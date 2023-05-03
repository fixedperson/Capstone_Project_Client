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
			MyPlayer temp = gameObject.GetComponent<MyPlayer>();
			temp.id = playerInfo.PlayerId;
			temp.enabled = true;
			temp.moveSpeed = 5;
			gameObject.name = playerInfo.Name;
			_players.Add(playerInfo.PlayerId, gameObject);
		}
		else
		{
			GameObject gameObject = Managers.Resource.Instantiate("Player/MC01");
			gameObject.name = playerInfo.Name;
			Player temp = gameObject.GetComponent<Player>();
			temp.enabled = true;
			temp.moveSpeed = 5;
			_players.Add(playerInfo.PlayerId, gameObject);
		}
	}

	public void EnemyAdd(EnemyInfo enemyInfo)
	{
		GameObject gameObject = Managers.Resource.Instantiate("Enemy/Bat");
		Enemy temp = gameObject.GetComponent<Enemy>();
		temp.enemyId = enemyInfo.EnemyId;
		temp.enabled = true;
		temp.moveSpeed = 3;
		temp.maxHealth = 100;
		temp.curHealth = temp.maxHealth;
		GameObject target;
		_players.TryGetValue(enemyInfo.PlayerId, out target);
		temp.player = target.GetComponent<Player>();
		temp.transform.position = new Vector3(enemyInfo.PosInfo.PosX, 0, enemyInfo.PosInfo.PosZ);
		_enemys.Add(enemyInfo.EnemyId, gameObject);
	}
	
	public void Remove(int id)
	{
		GameObject gameObject = _players[id];
		Managers.Resource.Destroy(gameObject);
		_players.Remove(id);
	}
	
	public GameObject PlayerFindById(int id)
	{
		GameObject gameObject = null;
		_players.TryGetValue(id, out gameObject);
		return gameObject;
	}

	public GameObject EnemyFindById(int id)
	{
		GameObject gameObject = null;
		_enemys.TryGetValue(id, out gameObject);
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
}
