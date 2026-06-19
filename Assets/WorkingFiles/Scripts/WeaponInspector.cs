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

    [Tooltip("Additional offset applied after automatic centering for fine tuning.")]
    [SerializeField] private Vector3 customOffset;

    private Vector3 _calculatedCenter;
    private float _yaw;
    private float _pitch;

    private Quaternion _targetRotation;
    private Vector3 _targetPosition;
    private bool _isDragging = false;

    private void Start()
    {
        // Calculate the visual center of the weapon so rotations occur
        // around the mesh itself rather than the imported object pivot.
        _calculatedCenter = CalculateVisualCenter();

        // Initialize rotation values from the weapon's current orientation.
        Vector3 currentAngles = transform.eulerAngles;
        _yaw = currentAngles.y;
        _pitch = currentAngles.x;

        _targetRotation = transform.rotation;
        _targetPosition = transform.position;
    }

    private void Update()
    {
        // Detect active mouse/touch interaction.
        HandleInput();

        if (_isDragging)
        {
            Vector2 mouseDelta = GetMouseDelta();

            // Horizontal drag rotates around the Y axis.
            _yaw -= mouseDelta.x * rotationSpeed;

            // Vertical drag rotates around the X axis.
            _pitch += mouseDelta.y * rotationSpeed;

            // Prevent extreme viewing angles that would flip the model.
            _pitch = Mathf.Clamp(_pitch, -60f, 60f);

            _targetRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
        else if (autoRotate)
        {
            // Showcase the weapon when idle by continuously rotating it.
            _yaw += autoRotateSpeed * Time.deltaTime;
            _targetRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }

        // Smoothly blend towards the desired rotation to avoid abrupt movement.
        Quaternion nextRotation = Quaternion.Slerp(
            transform.rotation,
            _targetRotation,
            smoothness * Time.deltaTime);

        // Rotate around the calculated mesh center instead of the object's pivot.
        Vector3 pivotPoint =
            transform.position +
            transform.TransformDirection(_calculatedCenter + customOffset);

        Vector3 positionOffset = transform.position - pivotPoint;

        // Recalculate position offset after applying rotation.
        positionOffset =
            nextRotation *
            Quaternion.Inverse(transform.rotation) *
            positionOffset;

        transform.rotation = nextRotation;
        transform.position = pivotPoint + positionOffset;
    }

    /// <summary>
    /// Calculates the combined center point of all child renderers.
    /// Used as a virtual pivot to keep rotation visually centered.
    /// </summary>
    private Vector3 CalculateVisualCenter()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0 || !autoCenterPivot)
            return Vector3.zero;

        Bounds combinedBounds = renderers[0].bounds;

        foreach (Renderer rend in renderers)
        {
            combinedBounds.Encapsulate(rend.bounds);
        }

        return transform.InverseTransformPoint(combinedBounds.center);
    }

    /// <summary>
    /// Determines whether the user is currently dragging
    /// using either mouse or touch input.
    /// </summary>
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

    /// <summary>
    /// Returns pointer movement delta from the active input device.
    /// Supports both desktop and mobile platforms.
    /// </summary>
    private Vector2 GetMouseDelta()
    {
        if (Mouse.current != null)
            return Mouse.current.delta.ReadValue();

        if (Touchscreen.current != null)
            return Touchscreen.current.primaryTouch.delta.ReadValue();

        return Vector2.zero;
    }
}