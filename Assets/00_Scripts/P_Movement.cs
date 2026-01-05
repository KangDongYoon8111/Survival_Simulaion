using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class P_Movement : MonoBehaviour
{
    [Header("Movement Settings")] 
    public float moveSpeed = 5.0f;
    
    [Header("Mouse Rotation")]
    public LayerMask groundLayer;
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Animator animator;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        RotateTowardsMouse();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        
        cameraForward.y = 0;
        cameraRight.y = 0;
        
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 moveDirection = cameraRight * horizontal + cameraForward * vertical;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        
        float currentSpeed = moveDirection.magnitude * moveSpeed;
        animator.SetFloat("a_Speed", currentSpeed);
    }

    private void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
