using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;  // Ссылка на TextMeshPro для отображения времени
    public int startHour = 7;  // Время начала дня (7 утра)

    private int currentHour;
    private int currentMinute;
    private float realSecondCounter;

    private void Start()
    {
        currentHour = startHour;
        currentMinute = 0;
        realSecondCounter = 0f;
    }

    private void Update()
    {
        // Считаем секунды в реальном времени
        realSecondCounter += Time.deltaTime;

        if (realSecondCounter >= 1f)  // Каждую секунду (1 мин = 1 сек)
        {
            realSecondCounter = 0f;
            IncrementGameTime();  // Обновляем игровое время
        }

        // Обновляем текст на экране
        timeText.text = $"{currentHour:D2}:{currentMinute:D2}";
    }

    private void IncrementGameTime()
    {
        currentMinute++;

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;

            if (currentHour >= 24)  // Если прошло 24 часа, начинаем с полуночи
                currentHour = 0;
        }
    }

    // Метод для получения текущего времени в игре
    public int GetCurrentHour() => currentHour;
    public int GetCurrentMinute() => currentMinute;
}
