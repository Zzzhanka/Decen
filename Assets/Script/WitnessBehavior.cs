using UnityEngine;
using System.Collections;

public class WitnessBehavior : MonoBehaviour
{
    public float escapeSpeed = 5f;  // Скорость бегства

    public void StartEscape()
    {
        Vector2 escapeDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        StartCoroutine(EscapeCoroutine(escapeDirection));
    }

    private IEnumerator EscapeCoroutine(Vector2 direction)
    {
        float escapeTime = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < escapeTime)
        {
            transform.Translate(direction * escapeSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);  // Уничтожаем свидетеля после бегства
    }
}
