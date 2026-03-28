using UnityEngine;

public class CarProperties : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] public float accelerationPower = 500f;
    [SerializeField] public float brakePower = 300f;
    [SerializeField] public float turnPower = 250f;
    [SerializeField] public float minSteerSpeed = 0.5f;

    [Header("Grip & Stability")]
    [Tooltip("How much grip the car has. Higher = less drift.")]
    [SerializeField] public float gripFactor = 0.5f;
    [Tooltip("The point where acceleration/brake forces are applied.")]
    [SerializeField] public Transform forceApplyPoint;
    [Tooltip("The Center of Mass when in the air, to help auto-balance.")]
    [SerializeField] public Vector3 airCenterOfMass = new Vector3(0, -1, 0);
    [SerializeField] public float maxSpeed = 30f;
}
