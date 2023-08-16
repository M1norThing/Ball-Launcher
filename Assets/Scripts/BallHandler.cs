using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivot;
    [SerializeField] float spawnDelay = 2f;
    [SerializeField] public float delayValue = 0.2f;

    Rigidbody2D currentBallRigidbody;
    SpringJoint2D currentBallSpringJoint;

    bool isDragging;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    void Update()
    {
        if (currentBallRigidbody == null) return;
      
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            return;
        }
        isDragging = true; 
        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        currentBallRigidbody.position = worldPosition;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;
        Invoke("DetachBall", delayValue);
    }
    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        Invoke("SpawnNewBall", spawnDelay);
    }
}
