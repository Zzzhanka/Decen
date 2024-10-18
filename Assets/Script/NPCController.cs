using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCController : MonoBehaviour
{
    public Transform home;
    public Transform office;
    public Tilemap walkableTilemap;
    public float moveSpeed = 2f;

    private Transform currentTarget;
    private TimeManager timeManager;
    private bool isWorking;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        UpdateTarget();
        StartCoroutine(MoveToTarget());
    }

    void Update()
    {
        if (timeManager.IsDaytime != isWorking)
        {
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        isWorking = timeManager.IsDaytime;
        currentTarget = isWorking ? office : home;
    }

    IEnumerator MoveToTarget()
    {
        while (true)
        {
            List<Vector3> path = AstarPathfinding.FindPath(
                transform.position, currentTarget.position, walkableTilemap);

            foreach (var point in path)
            {
                while (Vector2.Distance(transform.position, point) > 0.1f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }

            yield return new WaitForSeconds(5f);
            UpdateTarget();
        }
    }
}
