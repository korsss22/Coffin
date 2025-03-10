using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 platformVelocity = new Vector3(1, 0, 0);
    [SerializeField] private float moveDistance = 20f;
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 roatationVelocity;

    private Vector3 startLocation;
    void Start()
    {
        startLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
        RotatePlatform();
    }

    private void MovePlatform()
    {
        if(SholdPlatformReturn())
        {
            Vector3 moveDirection = platformVelocity.normalized;
            startLocation += moveDirection * moveDistance;
            transform.position = startLocation;
            platformVelocity = -platformVelocity;
        }
        else
        {
            transform.position += platformVelocity * Time.deltaTime;
        }
    }

    private void RotatePlatform()
    {
        transform.Rotate(roatationVelocity*Time.deltaTime);
    }

    private bool SholdPlatformReturn()
    {
        return GetDistanceMoved() > moveDistance;
    }

    private float GetDistanceMoved()
    {
        return Vector3.Distance(startLocation,transform.position);
    }
}
