using UnityEngine;

public class SnakeAI : MonoBehaviour
{
    public float moveSpeed;
    public GameObject[] waypoints;

    int nextWaypoint = 1;
    float distToPoint;

    void Update()
    {
        Move();
    }

    void Move()
    {
        distToPoint = Vector3.Distance(transform.position, waypoints[nextWaypoint].transform.position);

        transform.position = Vector3.MoveTowards(
            transform.position,
            waypoints[nextWaypoint].transform.position,
            moveSpeed * Time.deltaTime
        );

        if (distToPoint < 0.05f)
        {
            transform.position = waypoints[nextWaypoint].transform.position; // snap đúng vị trí
            TakeTurn();
        }
    }

    void TakeTurn()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation += waypoints[nextWaypoint].transform.eulerAngles;
        transform.eulerAngles = currentRotation;
        ChooseNextWaypoint();
    }
    void ChooseNextWaypoint()
    {
        nextWaypoint++;
        if (nextWaypoint == waypoints.Length)
        {
            nextWaypoint = 0;
        }
    }
}
