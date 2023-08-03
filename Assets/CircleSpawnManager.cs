using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    public GameObject circlePrefab;
    public int minCircleCount = 5;
    public int maxCircleCount = 10;
    public Material lineMaterial;

    private List<GameObject> circles = new List<GameObject>();
    private Vector3 lineStart;
    private TrailRenderer trailRenderer;

    private void Start()
    {
        SpawnRandomCircles(Random.Range(minCircleCount, maxCircleCount + 1));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lineStart = GetMouseWorldPosition();
            CreateTrailRenderer();
        }

        if (trailRenderer != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            UpdateTrailRenderer(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            DestroyTrailRenderer();
            CheckIntersectingCircles();
        }
    }

    private void CreateTrailRenderer()
    {
        GameObject trailGO = new GameObject("LineTrail");
        trailRenderer = trailGO.AddComponent<TrailRenderer>();
        trailRenderer.material = lineMaterial;
        trailRenderer.widthMultiplier = 0.1f;
        trailRenderer.time = 100f;
        trailRenderer.startColor = Color.red;
        trailRenderer.endColor = Color.red;
    }

    private void UpdateTrailRenderer(Vector3 endPosition)
    {
        if (trailRenderer != null)
        {
            Vector3[] positions = { lineStart, endPosition };
            trailRenderer.SetPositions(positions);
        }
    }

    private void DestroyTrailRenderer()
    {
        if (trailRenderer != null)
        {
            Destroy(trailRenderer.gameObject);
            trailRenderer = null;
        }
    }

    private void SpawnRandomCircles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            GameObject circle = Instantiate(circlePrefab, randomPosition, Quaternion.identity);
            circles.Add(circle);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit))
        {
            return new Vector3(hit.point.x, 0f, hit.point.z);
        }
        return Vector3.zero;
    }

    private void CheckIntersectingCircles()
    {
        Vector3 direction = GetMouseWorldPosition() - lineStart;
        Ray lineRay = new Ray(lineStart, direction);
        RaycastHit[] hits = Physics.RaycastAll(lineRay, direction.magnitude);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Circle"))
            {
                circles.Remove(hit.collider.gameObject);
                Destroy(hit.collider.gameObject);
            }
        }
    }
}