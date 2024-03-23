using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public List<Transform> patrolPoints;
    public PlayerController player;
    public float viewAngle;
    public float damage = 30;

    private NavMeshAgent _navMeshAgent;
    private bool _isPlayerNoticed;
    private PlayerHealth _playerhealth;

    // Start is called before the first frame update
    private void Start()
    {
        InitComponentsLinks();
        PickNewPatrolPoint();
    }

    // Update is called once per frame
    private void Update()
    {
        NoticePlayerUpdate();
        AttackUpdate();
        ChaseUpdate();
        PatrolUpdate();
    }

    private void PickNewPatrolPoint()
    {
        _navMeshAgent.destination = patrolPoints[Random.Range(0, patrolPoints.Count)].position;
    }

    private void PatrolUpdate()
    {
        if (!_isPlayerNoticed)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                PickNewPatrolPoint();
            }
        }
    }

    private void InitComponentsLinks()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerhealth = player.GetComponent<PlayerHealth>();
    }

    private void ChaseUpdate()
    {
        if (_isPlayerNoticed)
        {
            _navMeshAgent.destination = player.transform.position;
        }
    }

    private void NoticePlayerUpdate()
    {
        {
            var direction = player.transform.position - transform.position;
            _isPlayerNoticed = false;

            if (Vector3.Angle(transform.forward, direction) < viewAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, direction, out hit))
                {
                    if (hit.collider.gameObject == player.gameObject)
                    {
                        _isPlayerNoticed = true;
                    }
                }
            }
        }
    }

    private void AttackUpdate()
    {
        if (_isPlayerNoticed)
        {
            if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _playerhealth.DealDamage(damage * Time.deltaTime);
            }
        }
    }
}
