using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    X = 0,
    Y = 1
}

public class MovingObstacle : MonoBehaviour
{
    public Axis axis;
    public float minValue;
    public float maxValue;
    public float obstacleSpeed;

    private delegate void MoveDelegate();
    private MoveDelegate moveFunction;

    void Start()
    {
        maxValue -= minValue;

        if(axis == 0)
        {
            moveFunction = MoveObstacleInX;
        }
        else
        {
            moveFunction = MoveObstacleInY;
        }
    }

    void OnDisable() => this.enabled = false;

    void Update() => moveFunction();

    void MoveObstacleInX() => transform.position = new Vector3(Mathf.PingPong(Time.time * obstacleSpeed, maxValue) + minValue, transform.position.y, transform.position.z);

    void MoveObstacleInY() => transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * obstacleSpeed, maxValue) + minValue, transform.position.z);

    public void SetProperties(MovingObstacle obstacleScript)
    {
        axis = obstacleScript.axis;
        minValue = obstacleScript.minValue;
        maxValue = obstacleScript.maxValue;
        obstacleSpeed = obstacleScript.obstacleSpeed;
    }
}
