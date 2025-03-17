
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private Transform _player;
    public Transform player; // ��������� ������
    private NavMeshAgent agent; // ����� ���������

    void Start()
    {
        // �������� ��������� NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        // ������������� ���� ��� ������
        agent.SetDestination(_player.position);
    }
}


