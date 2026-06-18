using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInspector : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 0.15f;
    [SerializeField] private float smoothness = 8f;

    [Header("Auto-Rotate (Idle)")]
    [SerializeField] private bool autoRotate = true;
    [SerializeField] private float autoRotateSpeed = 20f;

    [Header("Pivot Correction")]
    [Tooltip("Automatically finds the center of the weapon meshes so it doesn't spin around the handle.")]
    [SerializeField] private bool autoCenterPivot = true;
    [Tooltip("Manual fine-tuning offset if auto-center isn't perfectly where you want it.")]
    [SerializeField] private Vector3 customOffset;

    private Vector3 _calculatedCenter;
    private float _yaw;
    private float _pitch;

    private Quaternion _targetRotation;
    private Vector3 _targetPosition;
    private bool _isDragging = false;

    void Start()
    {
        // 1. Find the true visual center of the weapon based on its renderers
        _calculatedCenter = CalculateVisualCenter();

        // 2. Initialize our angles based on how the weapon is currently rotated in the scene
        Vector3 currentAngles = transform.eulerAngles;
        _yaw = currentAngles.y;
        _pitch = currentAngles.x;

        _targetRotation = transform.rotation;
        _targetPosition = transform.position;
    }

    void Update()
    {
        // Handle input detection using New Input System
        HandleInput();

        // Calculate updates based on dragging or idling
        if (_isDragging)
        {
            Vector2 mouseDelta = GetMouseDelta();

            // Dragging left/right changes Yaw (around world Up)
            _yaw -= mouseDelta.x * rotationSpeed;
            // Dragging up/down changes Pitch (around world Right)
            _pitch += mouseDelta.y * rotationSpeed;

            // Clamp vertical tilt so you can't view it completely upside down
            _pitch = Mathf.Clamp(_pitch, -60f, 60f);

            _targetRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
        else if (autoRotate)
        {
            // Smoothly continue the idle spin showcase
            _yaw += autoRotateSpeed * Time.deltaTime;
            _targetRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }

        // Apply smooth interpolation (Slerp) for the rotation
        Quaternion nextRotation = Quaternion.Slerp(transform.rotation, _targetRotation, smoothness * Time.deltaTime);

        // Mathematical pivot shift: Rotate around our calculated center point instead of the transform pivot
        Vector3 pivotPoint = transform.position + transform.TransformDirection(_calculatedCenter + customOffset);
        Vector3 positionOffset = transform.position - pivotPoint;

        // Rotate the offset vector
        positionOffset = nextRotation * Quaternion.Inverse(transform.rotation) * positionOffset;

        // Apply both changes seamlessly
        transform.rotation = nextRotation;
        transform.position = pivotPoint + positionOffset;
    }

    private Vector3 CalculateVisualCenter()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0 || !autoCenterPivot) return Vector3.zero;

        // Combine the bounds of all meshes attached to this object
        Bounds combinedBounds = renderers[0].bounds;
        foreach (Renderer rend in renderers)
        {
            combinedBounds.Encapsulate(rend.bounds);
        }

        // Return the center point local to this transform
        return transform.InverseTransformPoint(combinedBounds.center);
    }

    private void HandleInput()
    {
        if (Mouse.current != null)
        {
            _isDragging = Mouse.current.leftButton.isPressed;
        }
        else if (Touchscreen.current != null)
        {
            _isDragging = Touchscreen.current.primaryTouch.press.isPressed;
        }
    }

    private Vector2 GetMouseDelta()
    {
        if (Mouse.current != null) return Mouse.current.delta.ReadValue();
        if (Touchscreen.current != null) return Touchscreen.current.primaryTouch.delta.ReadValue();
        return Vector2.zero;
    }
}