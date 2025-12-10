using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Transform that NPC has to follow
    public Transform target;
    //NavMesh Agent variable
    NavMeshAgent agent;

    public float distance;

    [SerializeField] private EnemyStats stats;

    [SerializeField] private bool isNeedWaiting;
    [SerializeField] private float waitTime;

    public string State; //это для отладки, можно убрать


    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //получаем навмеш
        StartCoroutine(MainBehaviourLoop()); //начинаем корутину MainBehaviourLoop
    }

    void Update()
    {
        distance = Vector3.Distance(this.gameObject.transform.position, GameObject.FindWithTag("Player").transform.position);//рассчитываем дистанцию между нпс и игроком
    }

    IEnumerator MainBehaviourLoop()
    {
        while (true)
        {
            if (distance < 3)
            {
                yield return StartCoroutine(Attack()); //запускаем корутину атаки и ждем её полного завершения перед следующим циклом
            }
            else if (distance <= 7)
            {
                yield return StartCoroutine(Chase()); //запускаем корутину преследования и ждем её завершения
            }
            else if (distance > 7)
            {
                yield return StartCoroutine(Patrol());  //запускаем корутину патрулирования и ждем её завершения
            }
            yield return null; //приостанавливает выполнение цикла до следующего кадра
        }
    }

    IEnumerator Attack()
    {
        State = "Attacking";
        agent.isStopped = true;
        yield return null; //завершаем выполнение корутины
    }

    IEnumerator Chase()
    {
        State = "Chasing";
        agent.isStopped = false;

        target = GameObject.FindWithTag("Player").transform;

        agent.destination = target.position;
        agent.speed = stats.speed;

        yield return null; //завершаем выполнение корутины
    }

    IEnumerator Patrol()
    {
        State = "Patrolling";
        agent.isStopped = false;

        for (int i = 0; i < stats.waypoints.Length/*-1*/; i++)
        {
            target = stats.waypoints[i];

            if (target == null) continue;

            agent.destination = target.position;
            agent.speed = stats.speed;

            while (Vector3.Distance(transform.position, target.position) > agent.stoppingDistance + 0.1f)
            {
                if (distance < 7)
                {
                    yield break; //немедленный выход из текущей корутины Patrol
                }
                yield return null;
            }
            if (isNeedWaiting)
            {
                yield return StartCoroutine(Waiting(waitTime)); //запускаем корутину ожидания
            }
            if (i == stats.waypoints.Length - 1)
            {
                i = -1;
            }
        }
    }
    IEnumerator Waiting(float waitDelay)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(waitDelay); // пауза на указанное количество секунд
        agent.isStopped = false;
    }
}
