using UnityEngine;

public class CarAudioHandler : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource engineAudioSource;
    [SerializeField] private AudioSource screechAudioSource;

    [Header("Engine Pitch")]
    [SerializeField] private float minPitch = 0.5f;
    [SerializeField] private float maxPitch = 2.0f;
    [SerializeField] private float pitchChangeSpeed = 5f;
    [SerializeField] private float maxSpeedForPitch = 30f;

    [Header("Screeching")]
    [SerializeField] private float slipThreshold = 3.0f;
    [SerializeField] private float volumeChangeSpeed = 10f;

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
        HandleEngineSound();
        HandleScreechSound();
    }

    void HandleEngineSound()
    {
        float speedPercent = rb.linearVelocity.magnitude / maxSpeedForPitch;
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, speedPercent);

        if (inputHandler.IsAccelerating || inputHandler.IsBraking)
        {
            targetPitch = Mathf.Min(targetPitch + 0.3f, maxPitch);
        }

        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, targetPitch, Time.deltaTime * pitchChangeSpeed);
    }

    void HandleScreechSound()
    {
        if (!groundCheck.IsGrounded || screechAudioSource == null)
        {
            if (screechAudioSource != null) screechAudioSource.volume = 0;
            return;
        }

        float sidewaysSpeed = Mathf.Abs(Vector3.Dot(rb.linearVelocity, transform.right));
        float targetVolume = 0f;

        if (sidewaysSpeed > slipThreshold)
        {
            targetVolume = Mathf.InverseLerp(slipThreshold, 15f, sidewaysSpeed);
        }

        screechAudioSource.volume = Mathf.Lerp(screechAudioSource.volume, targetVolume, Time.deltaTime * volumeChangeSpeed);
    }
}