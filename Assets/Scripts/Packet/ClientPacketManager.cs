using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
	
	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }
	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.SEnterGame, MakePacket<S_EnterGame>);
		_handler.Add((ushort)MsgId.SEnterGame, PacketHandler.S_EnterGameHandler);		
		_onRecv.Add((ushort)MsgId.SLeaveGame, MakePacket<S_LeaveGame>);
		_handler.Add((ushort)MsgId.SLeaveGame, PacketHandler.S_LeaveGameHandler);		
		_onRecv.Add((ushort)MsgId.SOtherPlayerSpawn, MakePacket<S_OtherPlayerSpawn>);
		_handler.Add((ushort)MsgId.SOtherPlayerSpawn, PacketHandler.S_OtherPlayerSpawnHandler);		
		_onRecv.Add((ushort)MsgId.SEnemySpawn, MakePacket<S_EnemySpawn>);
		_handler.Add((ushort)MsgId.SEnemySpawn, PacketHandler.S_EnemySpawnHandler);		
		_onRecv.Add((ushort)MsgId.SPlayerDestroy, MakePacket<S_PlayerDestroy>);
		_handler.Add((ushort)MsgId.SPlayerDestroy, PacketHandler.S_PlayerDestroyHandler);		
		_onRecv.Add((ushort)MsgId.SEnemyDestroy, MakePacket<S_EnemyDestroy>);
		_handler.Add((ushort)MsgId.SEnemyDestroy, PacketHandler.S_EnemyDestroyHandler);		
		_onRecv.Add((ushort)MsgId.SEnemyMove, MakePacket<S_EnemyMove>);
		_handler.Add((ushort)MsgId.SEnemyMove, PacketHandler.S_EnemyMoveHandler);		
		_onRecv.Add((ushort)MsgId.SEnemyTargetReset, MakePacket<S_EnemyTargetReset>);
		_handler.Add((ushort)MsgId.SEnemyTargetReset, PacketHandler.S_EnemyTargetResetHandler);		
		_onRecv.Add((ushort)MsgId.SPlayerMove, MakePacket<S_PlayerMove>);
		_handler.Add((ushort)MsgId.SPlayerMove, PacketHandler.S_PlayerMoveHandler);		
		_onRecv.Add((ushort)MsgId.SPlayerAction, MakePacket<S_PlayerAction>);
		_handler.Add((ushort)MsgId.SPlayerAction, PacketHandler.S_PlayerActionHandler);		
		_onRecv.Add((ushort)MsgId.STimeInfo, MakePacket<S_TimeInfo>);
		_handler.Add((ushort)MsgId.STimeInfo, PacketHandler.S_TimeInfoHandler);		
		_onRecv.Add((ushort)MsgId.SEndStage, MakePacket<S_EndStage>);
		_handler.Add((ushort)MsgId.SEndStage, PacketHandler.S_EndStageHandler);		
		_onRecv.Add((ushort)MsgId.SHostUser, MakePacket<S_HostUser>);
		_handler.Add((ushort)MsgId.SHostUser, PacketHandler.S_HostUserHandler);		
		_onRecv.Add((ushort)MsgId.SEnemyHit, MakePacket<S_EnemyHit>);
		_handler.Add((ushort)MsgId.SEnemyHit, PacketHandler.S_EnemyHitHandler);		
		_onRecv.Add((ushort)MsgId.SPlayerHit, MakePacket<S_PlayerHit>);
		_handler.Add((ushort)MsgId.SPlayerHit, PacketHandler.S_PlayerHitHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
		if (CustomHandler != null)
		{
			CustomHandler.Invoke(session,pkt,id);
		}
		else
		{
			Action<PacketSession, IMessage> action = null;
			if (_handler.TryGetValue(id, out action))
				action.Invoke(session, pkt);
			
		}
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}