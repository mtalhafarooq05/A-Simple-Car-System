using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputHandler : MonoBehaviour
{
    private CarInput carInputActions;
    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }
    public bool IsBraking { get; private set; }
    public bool IsAccelerating { get; private set; }

    void Awake()
    {
        carInputActions = new CarInput();
    }

    private void OnEnable()
    {
        carInputActions.Move.Enable();
    }

    private void OnDisable()
    {
        carInputActions.Move.Disable();
    }

    void Update()
    {
        //steering
        HorizontalInput = carInputActions.Move.Steer.ReadValue<float>();

        //acceleration and braking
        IsAccelerating = carInputActions.Move.Accel.IsPressed();
        IsBraking = carInputActions.Move.Decel.IsPressed();

        float accel = IsAccelerating ? 1f : 0f;
        float brake = IsBraking ? -1f : 0f;
        VerticalInput = accel + brake;
    }
}