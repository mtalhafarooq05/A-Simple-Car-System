using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarPhysicsController : MonoBehaviour
{
    

    private Rigidbody rb;
    private CarInputHandler inputHandler;
    private CarGroundCheck groundCheck;
    private Vector3 originalCenterOfMass;
    private CarProperties carProperties;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<CarInputHandler>();
        groundCheck = GetComponent<CarGroundCheck>();
        carProperties = GetComponent<CarProperties>();
        originalCenterOfMass = rb.centerOfMass;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        HandleGrip();
        HandleAutoBalancing();
    }

    void HandleMovement()
    {
        if (!groundCheck.IsGrounded) return;

        float forceValue = inputHandler.VerticalInput;
        Vector3 forceDirection = transform.forward * forceValue;

        

        if (forceValue > 0)
        {
            if (rb.linearVelocity.magnitude < carProperties.maxSpeed)
            {
                rb.AddForceAtPosition(forceDirection * carProperties.accelerationPower, carProperties.forceApplyPoint.position, ForceMode.Acceleration);
            }
        }
        else if (forceValue < 0)
        {
            rb.AddForceAtPosition(forceDirection * carProperties.brakePower, carProperties.forceApplyPoint.position, ForceMode.Acceleration);
        }
    }

    void HandleSteering()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        float movementSpeed = (rb.linearVelocity / carProperties.maxSpeed).magnitude;
        float movementDir = Vector3.Dot(rb.linearVelocity.normalized, rb.transform.forward);
        //Debug.Log(movementSpeed);

        if (groundCheck.IsGrounded && currentSpeed > carProperties.minSteerSpeed)
        {
            Vector3 turnTorque =  movementDir * transform.up * inputHandler.HorizontalInput * carProperties.turnPower * movementSpeed;
            rb.AddTorque(turnTorque, ForceMode.Acceleration);
        }
    }

    void HandleGrip()
    {
        if (!groundCheck.IsGrounded) return;

        Vector3 sidewaysVelocity = Vector3.Dot(rb.linearVelocity, transform.right) * transform.right;
        Vector3 gripForce = -sidewaysVelocity * carProperties.gripFactor;
        rb.AddForce(gripForce, ForceMode.Acceleration);
    }

    void HandleAutoBalancing()
    {
        rb.centerOfMass = groundCheck.IsGrounded ? originalCenterOfMass : carProperties.airCenterOfMass;
    }
}