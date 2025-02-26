
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public Transform player; // Трансформ игрока
    private NavMeshAgent agent; // Агент навигации

    void Start()
    {
        // Получаем компонент NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Устанавливаем цель для агента
        agent.SetDestination(player.position);
    }
}


