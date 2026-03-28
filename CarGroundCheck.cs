using UnityEngine;

public class CarGroundCheck : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float raycastDistance = 3.0f;
    [SerializeField] private float raycastStartOffset = 3.0f;

    public bool IsGrounded { get; private set; }

    void FixedUpdate()
    {
        Vector3 rayStartPoint = transform.position + (Vector3.up * raycastStartOffset);
        Vector3 rayDirection = -transform.up;

        if (Physics.Raycast(rayStartPoint, rayDirection, raycastDistance + raycastStartOffset))
        {
            IsGrounded = true;
            Debug.DrawRay(rayStartPoint, rayDirection * (raycastDistance + raycastStartOffset), Color.green);
        }
        else
        {
            IsGrounded = false;
            Debug.DrawRay(rayStartPoint, rayDirection * (raycastDistance + raycastStartOffset), Color.red);
        }
    }
}