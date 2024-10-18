using System.Collections;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float dayDuration = 30f; // ������������ ��� � ��������
    public delegate void TimeChanged(bool isDay); // ������� ��� ����� �������
    public static event TimeChanged OnTimeChanged;

    private bool isDay = true;

    void Start()
    {
        StartCoroutine(TimeCycle());
    }

    IEnumerator TimeCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(dayDuration);
            isDay = !isDay;
            OnTimeChanged?.Invoke(isDay); // ���������� ���� � ����� �������
        }
    }
}
