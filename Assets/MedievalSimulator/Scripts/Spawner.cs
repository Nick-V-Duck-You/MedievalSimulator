using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    public GameObject npcPrefab; // Префаб NPC, который будет спавнить
    public float spawnInterval = 5f; // Интервал между спавном в секундах

    [Header("Точки спавна")]
    public List<Transform> spawnPoints = new List<Transform>(); // Список точек спавна

    private void Start()
    {
        // Запускаем повторяющийся спавн
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true) // Бесконечный цикл
        {
            yield return new WaitForSeconds(spawnInterval); 

            SpawnNPC(); 
        }
    }

    private void SpawnNPC()
    {
        
        if (spawnPoints.Count == 0 || npcPrefab == null)
        {
            Debug.LogWarning("Не назначены точки спавна или префаб NPC!");
            return;
        }

        
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        
        Instantiate(npcPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        Debug.Log($"NPC заспавнен в точке: {selectedSpawnPoint.name}");
    }

    
    public void SpawnNPCManual()
    {
        SpawnNPC();
    }
}

