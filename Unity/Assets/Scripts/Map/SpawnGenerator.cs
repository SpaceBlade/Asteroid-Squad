﻿using UnityEngine;
using System.Collections;

public class SpawnGenerator : MonoBehaviour {

    public GameObject[] SpawnPrefabs;
    public int MinimumSpawns = 1;
    public int MaximumSpawns = 1;
    
    public bool ResetSpawn = true;
    public string SpawnName = "Nonnamed";

    private Transform Origin;
    private BattleTurnSystem BattleTurnSystem;

    // Use this for initialization
    void Start () {
        var mapProperties = GetComponentInParent<MapProperties>();
        BattleTurnSystem = mapProperties.BattleTurnSystem;
        
        Origin = GetComponentInParent<Transform>();
        Random.seed = 12345678;

        Spawn();
	}
	
	// Update is called once per frame
	void Update () {
	    if(!ResetSpawn || SpawnPrefabs == null || SpawnPrefabs.Length < 1)
        {
            return;
        }

        Spawn();
    }

    private void Spawn()
    {
        int spawnIndex = 0;
        var generatorIndex = MinimumSpawns;
        if (!BattleTurnSystem.Squads.ContainsKey(SpawnName))
        {
            var squad = new GameObject[MaximumSpawns];
            Debug.Log(string.Format("Team {0} ({1:n}) spawning points", SpawnName, MaximumSpawns));
            while (generatorIndex <= MaximumSpawns)
            {
                // Add NPC
                Vector3 spawnOffset = new Vector3(Random.Range(0.0f, 5.0f), 15.0f, Random.Range(0.0f, 5.0f));
                squad[spawnIndex++] = (GameObject)Instantiate(SpawnPrefabs[Random.Range(0, SpawnPrefabs.Length - 1)], Origin.position + spawnOffset, Origin.rotation);
                generatorIndex++;
            }

            BattleTurnSystem.Squads.Add(SpawnName, squad);
        }

        ResetSpawn = false;
    }
}