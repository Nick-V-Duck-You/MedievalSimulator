using UnityEngine;
using UnityEngine.AI;

public class FollowerController : MonoBehaviour
{
    //Transform that NPC has to follow
    public Transform transformToFollow;
    //NavMesh Agent variable
    NavMeshAgent agent;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private FollowerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Follow the player with player speed
        if (stats.isRecruited)
        {
            agent.destination = transformToFollow.position;
            agent.speed = playerController._speed;
        }
        
    }
}
