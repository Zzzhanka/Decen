using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCController : MonoBehaviour
{
    public Animator animator; // Аниматор для смены анимаций
    public float moveDelay = 1f; // Задержка между шагами
    public float walkSpeed = 2f; // Скорость перехода
    public Tilemap tilemap; // Ссылка на Tilemap для движения по клеткам

    public Vector3Int[] path; // Путь из позиций тайлмапы
    public string houseTag; // Тег дома для NPC

    private int currentStep = 0; // Текущая цель на пути
    private bool isMoving = false; // Флаг движения

    void Start()
    {
        StartCoroutine(WalkRoutine()); // Запуск корутины для перемещения
    }

    IEnumerator WalkRoutine()
    {
        while (true)
        {
            if (!isMoving)
            {
                Vector3 targetPosition = tilemap.CellToWorld(path[currentStep]) + new Vector3(0.5f, 0.5f, 0);
                StartCoroutine(MoveTo(targetPosition)); // Начинаем движение к следующей клетке

                currentStep = (currentStep + 1) % path.Length; // Переход к следующей клетке
            }

            yield return new WaitForSeconds(moveDelay); // Задержка между перемещениями
        }
    }

    IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;
        animator.SetBool("isWalking", true);

        while (Vector2.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, walkSpeed * Time.deltaTime);
            yield return null; // Ждём следующий кадр
        }

        transform.position = target; // Убедимся, что встали точно на клетку
        animator.SetBool("isWalking", false);
        isMoving = false; // Останавливаемся
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(houseTag)) // Проверка на дом с нужным тегом
        {
            animator.SetTrigger("enterHouse"); // Анимация входа в дом
            gameObject.SetActive(false); // Скрываем NPC
        }
    }
}
