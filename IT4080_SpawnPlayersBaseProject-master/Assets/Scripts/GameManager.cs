using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class GameManager : NetworkBehaviour {

    public Player playerPrefab;
    public GameObject spawnPoints;

    private int spawnIndex = 0;
    private List<Vector3> avaliableSpawnPositions = new List<Vector3>();

    public void Awake()
    {
        refreshSpawnPoints();
    }

    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            SpawnPlayers();
        }
        
    }
    private void refreshSpawnPoints()
    {
        Transform[] allPoints = spawnPoints.GetComponentsInChildren<Transform>(); 
        avaliableSpawnPositions.Clear();
        foreach(Transform point in allPoints)
        {
            if(point != spawnPoints.transform)
            {
                avaliableSpawnPositions.Add(point.localPosition);
            }
        }
    }

    public Vector3 GetNextSpawnLocation()
    {
        var newPostion = avaliableSpawnPositions[spawnIndex];
        newPostion.y = 1.5f;
        spawnIndex += 1;

        if(spawnIndex > avaliableSpawnPositions.Count - 1)
        {
            spawnIndex = 0;
        }
        return newPostion;
    }

    private void SpawnPlayers()
    {
        foreach(PlayerInfo pi in GameData.Instance.allPlayers)
        {
            Debug.Log("Player Has Spawned");
            Player playerSpawn = Instantiate(playerPrefab,
                GetNextSpawnLocation(),
                Quaternion.identity);

            playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(pi.clientId);
            playerSpawn.PlayerColor.Value = pi.color;
          
        }
    }
}