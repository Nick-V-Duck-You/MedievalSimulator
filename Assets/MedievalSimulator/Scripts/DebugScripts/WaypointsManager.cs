using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    public string targetTag = "Waypoints";
    public GameObject[] existingObjects;
    public List<Transform> myTransforms;

    public void Start()
    {
        //stats = GetComponent<EnemyStats>();
        existingObjects = GameObject.FindGameObjectsWithTag(targetTag);
        for (int i = 0; i < existingObjects.Length ;i++) 
        {
            myTransforms.Add(existingObjects[i].transform);
        }
        stats.waypoints = myTransforms.ToArray();
    }
}
