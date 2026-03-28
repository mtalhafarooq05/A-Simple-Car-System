using UnityEngine;

public class CarVfxHandler : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private TrailRenderer[] skidTrails;
    [SerializeField] private ParticleSystem[] smokeParticles;
     
    [Header("Triggers")]
    [SerializeField] private float slipThreshold = 3.0f;
    [SerializeField] private float minSteerSpeedForBrake = 0.5f;

    private Rigidbody rb;
    private CarInputHandler inputHandler;
    private CarGroundCheck groundCheck;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<CarInputHandler>();
        groundCheck = GetComponent<CarGroundCheck>();
    }

    void Update()
    {
        HandleSkidMarks();
    }

    void HandleSkidMarks()
    {
        if (!groundCheck.IsGrounded)
        {
            UpdateEffects(false);
            return;
        }

        float sidewaysSpeed = Mathf.Abs(Vector3.Dot(rb.linearVelocity, transform.right));

        bool isSkidding = sidewaysSpeed > slipThreshold;
        bool isBraking = inputHandler.IsBraking && rb.linearVelocity.magnitude > minSteerSpeedForBrake;

        bool shouldBeEmitting = isSkidding || isBraking;

        UpdateEffects(shouldBeEmitting);
    }

    void UpdateEffects(bool emit)
    {
        foreach (TrailRenderer trail in skidTrails)
        {
            trail.emitting = emit;
        }

        foreach (ParticleSystem smoke in smokeParticles)
        {
            var emission = smoke.emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);

            if (bursts.Length > 0)
            {
                bursts[0].probability = emit ? 1f : 0f;
                emission.SetBursts(bursts);
            }
        }
    }
}