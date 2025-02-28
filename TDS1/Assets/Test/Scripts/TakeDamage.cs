using UnityEngine;

public class Health : MonoBehaviour
{
    // Максимальное здоровье
    public float maxHealth = 100f;
    // Текущее здоровье
    private float currentHealth;

    // Событие при смерти
    public delegate void OnDeath();
    public event OnDeath DeathEvent;

    // Инициализация компонента
    private void Start()
    {
        // Устанавливаем текущее здоровье равным максимальному
        currentHealth = maxHealth;
    }

    // Метод, который вызывается для получения урона
    public void TakeDamage(float damage)
    {
        // Уменьшаем текущее здоровье на величину урона
        currentHealth -= damage;

        // Проверяем, не умер ли объект
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Метод, который вызывается, когда здоровье падает до нуля или ниже
    private void Die()
    {
        // Вызываем событие смерти, если оно подписано
        if (DeathEvent != null)
        {
            DeathEvent.Invoke();
        }

        // Здесь можно добавить дополнительную логику смерти, например, анимацию или удаление объекта
        Debug.Log($"{gameObject.name} has died.");

        // Уничтожаем объект (или можно отключить его)
        Destroy(gameObject);
    }

    // Метод для восстановления здоровья (если необходимо)
    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }

    // Метод для получения текущего здоровья
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}

