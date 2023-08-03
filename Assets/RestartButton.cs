using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public drawline LineDrawer; // Reference to the drawline script
    public GameObject circlePrefab;
    public int numberOfCircles = 10;
    public float minRange = 5f;
    public float maxRange = 10f;

    private void Start()
    {
       
    }

    private void RestartScene()
    {
        // Reset the line renderer
        LineDrawer.ResetLine();

        // Destroy existing circles
        GameObject[] circles = GameObject.FindGameObjectsWithTag("Circle");
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }

        // Spawn new circles
       LineDrawer.SpawnCircles();
    }
}