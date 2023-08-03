using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawline : MonoBehaviour
{
    public LineRenderer line;
    private Vector3 prevpos;

    [SerializeField]
    private float mindist = 0.1f;

    public GameObject circlePrefab;
    public int numberOfCircles = 10;
    public float minRange = 5f;
    public float maxRange = 10f;

    private List<GameObject> circles = new List<GameObject>();

    void Start()
    {
        line = GetComponent<LineRenderer>();
        prevpos = transform.position;

        SpawnCircles();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentpos.z = 0f;

            if (Vector3.Distance(currentpos, prevpos) > mindist)
            {
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, currentpos);
                prevpos = currentpos;
            }

            CheckCollisions();
        }
    }
    public void ResetLine()
    {
        line.positionCount = 0;
    }
    public  void SpawnCircles()
    {
        for (int i = 0; i < numberOfCircles; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject circle = Instantiate(circlePrefab, randomPosition, Quaternion.identity);
            circle.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
            circles.Add(circle);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = Random.onUnitSphere * Random.Range(minRange, maxRange);
        randomPosition.z = 0f;
        return randomPosition;
    }

    void CheckCollisions()
    {

        for (int i = circles.Count - 1; i >= 0; i--)
        {
            GameObject circle = circles[i];

            if (circle == null) // Check if the circle object has been destroyed
            {
                circles.RemoveAt(i);
                continue;
            }

            Vector3 circlePosition = circle.transform.position;
            float distance = Vector3.Distance(line.GetPosition(line.positionCount - 1), circlePosition);
            float circleRadius = circle.transform.localScale.x * 0.5f;

            if (distance <= circleRadius)
            {
                Destroy(circle);
                circles.RemoveAt(i);
            }
        }
    }
}