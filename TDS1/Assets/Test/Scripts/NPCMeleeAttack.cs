using UnityEngine;

public class NPCMeleeAttack : MonoBehaviour
{
    public float attackRange = 2f;  // Радиус атаки
    public float attackDamage = 10f;  // Урон атаки
    public float attackCooldown = 1f;  // Время между атаками
    private float lastAttackTime = 0f;  // Время последней атаки

    public Transform target;  // Цель (игрок или другой NPC)
    public LayerMask targetLayer;  // Слой, на котором находятся цели (например, игроки)

    private void Update()
    {
        // Проверка времени на атаку
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        // Проверка, находится ли цель в радиусе атаки
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

        foreach (Collider hit in hitTargets)
        {
            // Проверка, является ли цель допустимой
            if (hit.transform == target)
            {
                // Применяем урон к цели (например, у объекта игрока или NPC есть компонент здоровья)
                hit.transform.GetComponent<Entity>().TakeDamage(attackDamage);
                lastAttackTime = Time.time;  // Обновляем время последней атаки
                Debug.Log("Attack hit: " + target.name);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Отображаем радиус атаки в редакторе для удобства
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

