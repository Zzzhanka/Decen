using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCController : MonoBehaviour
{
    public Tilemap walkableTilemap; // Таймэп с проходимыми клетками
    public Transform[] waypoints; // Точки, к которым будет двигаться NPC
    public float moveSpeed = 2f; // Скорость NPC
    private List<Vector3> path; // Путь, который будет следовать NPC
    private int currentPoint = 0; // Текущий целевой узел
    private Rigidbody2D rb; // Rigidbody NPC

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveToWork());
    }


    private void MoveToPoint(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Здесь можно добавить логику анимации, если необходимо

    }
    private IEnumerator MoveToWork()
    {
        yield return new WaitForSeconds(1f); // Ждём немного, прежде чем начать

        // Начинаем с 7:00
        TimeManager.Instance.SetTime(7, 0); // Установить время в 7:00

        // NPC выходит из дома в 8:00
        while (TimeManager.Instance.CurrentHour < 8)
        {
            yield return null; // Ждём, пока не наступит 8:00
        }

        // Получаем путь от дома до офиса
        path = AstarPathfinding.FindPath(transform.position, waypoints[1].position, walkableTilemap);
        currentPoint = 0;

        while (currentPoint < path.Count)
        {
            MoveToPoint(path[currentPoint]);
            if (Vector3.Distance(transform.position, path[currentPoint]) < 0.1f) // Если достигли текущей точки
            {
                currentPoint++;
            }
            yield return null; // Ждём один кадр
        }

        // NPC задержится на работе до 18:30
        yield return new WaitForSeconds(10f); // Задержка для симуляции времени на работе

        // NPC возвращается домой
        path = AstarPathfinding.FindPath(transform.position, waypoints[0].position, walkableTilemap);
        currentPoint = 0;

        while (currentPoint < path.Count)
        {
            MoveToPoint(path[currentPoint]);
            if (Vector3.Distance(transform.position, path[currentPoint]) < 0.1f) // Если достигли текущей точки
            {
                currentPoint++;
            }
            yield return null; // Ждём один кадр
        }
    }

}
