using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class ObjectManager
{
	// 플레이어 리스트 
	Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
	
	// 몬스터 리스트 
	Dictionary<int, GameObject> _enemys = new Dictionary<int, GameObject>();

	Random rand = new Random();
	
	// 몬스터와 플레이어 Add 함수 구분
	public void PlayerAdd(PlayerInfo playerInfo, bool myPlayer = false)
	{
		if (myPlayer)
		{
			GameObject gameObject = Managers.Resource.Instantiate("Player/HeroPlayer");
			PlayerCamera.targetTransform = gameObject.transform;
			MyPlayer temp = gameObject.GetComponent<MyPlayer>();
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
		temp.enabled = true;
		temp.moveSpeed = 3;
		temp.maxHealth = 100;
		temp.curHealth = temp.maxHealth;
		temp.player = _players.ElementAt(rand.Next(0, _players.Count)).Value.GetComponent<Player>();
		temp.transform.position = new Vector3(enemyInfo.PosInfo.PosX, 0, enemyInfo.PosInfo.PosZ);
	}
	
	public void Remove(int id)
	{
		GameObject gameObject = _players[id];
		Managers.Resource.Destroy(gameObject);
		_players.Remove(id);
	}
	
	public GameObject FindById(int id)
	{
		GameObject gameObject = null;
		_players.TryGetValue(id, out gameObject);
		return gameObject;
	}
	
}
