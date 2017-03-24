using System.Collections.Generic;
using UnityEngine;
using TAMKShooter.Data;
using TAMKShooter.Systems;
using System;

namespace TAMKShooter
{
	public class PlayerUnits : MonoBehaviour
	{
		private Dictionary<PlayerData.PlayerId, PlayerUnit> _players =
			new Dictionary<PlayerData.PlayerId, PlayerUnit> ();

		public void Init(params PlayerData[] players)
		{
			foreach (PlayerData playerData in players)
			{
				// Get prefab by UnitType
				PlayerUnit unitPrefab =
					Global.Instance.Prefabs.
					GetPlayerUnitPrefab ( playerData.UnitType );

				if(unitPrefab != null)
				{
					// Initialize unit
					PlayerUnit unit = Instantiate ( unitPrefab, transform );
					unit.transform.position = Vector3.zero;
					switch (playerData.Id) {
					case PlayerData.PlayerId.Player1:
					{
							var spawnPoint = GameObject.Find ("Spawnpoint 1");
							if (null != spawnPoint) {
								unit.transform.position = spawnPoint.transform.position;
							} else {
								Debug.Log ("Spawn Point 1 not found; using default position.");
								unit.transform.position = new Vector3 (-12f, 0f, -4f);
							}
							break;
					}
					case PlayerData.PlayerId.Player2:
					{
						var spawnPoint = GameObject.Find ("Spawnpoint 2");
						if (null != spawnPoint) {
							unit.transform.position = spawnPoint.transform.position;
						} else {
							Debug.Log ("Spawn Point 2 not found; using default position.");
							unit.transform.position = new Vector3 (-4f, 0f, -4f);
						}
						break;
					}
					case PlayerData.PlayerId.Player3:
					{
						var spawnPoint = GameObject.Find ("Spawnpoint 3");
						if (null != spawnPoint) {
							unit.transform.position = spawnPoint.transform.position;
						} else {
							Debug.Log ("Spawn Point 3 not found; using default position.");
							unit.transform.position = new Vector3 (4f, 0f, -4f);
						}
						break;
					}
					case PlayerData.PlayerId.Player4:
						{
							var spawnPoint = GameObject.Find ("Spawnpoint 4");
							if (null != spawnPoint) {
								unit.transform.position = spawnPoint.transform.position;
							} else {
								Debug.Log ("Spawn Point 4 not found; using default position.");
								unit.transform.position = new Vector3 (12f, 0f, -4f);
							}
							break;
						}
					}

					unit.transform.rotation = Quaternion.identity;
					unit.Init ( playerData );

					// Add player to dictionary
					_players.Add ( playerData.Id, unit );
				}
				else
				{
					Debug.LogError ( "Unit prefab with type " + playerData.UnitType +
						" could not be found!" );
				}
			}
		}

		public void UpdateMovement ( InputManager.ControllerType controller, 
			Vector3 input, bool shoot )
		{
			PlayerUnit playerUnit = null;
			foreach (var player in _players)
			{
				if(player.Value.Data.Controller == controller)
				{
					playerUnit = player.Value;
				}
			}

			if(playerUnit != null)
			{
				playerUnit.HandleInput ( input, shoot );
			}
		}

	}
}

