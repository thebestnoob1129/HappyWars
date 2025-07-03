using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    Transform focus = default;

    [SerializeField, Range(1, 20)]
    float distance = 5f;

    [SerializeField, Min(0)]
    float focusRadius = 1f;

    [SerializeField, Range(0, 1)]
    float focusCentering = 0.5f;

    [SerializeField, Range(1, 360)]
    float rotationSpeed = 90;

    [SerializeField, Range(-89, 89)]
    float minVerticalAngle = -30, maxVerticalAngle = 60;

    [SerializeField, Min(0)]
    float alignDelay = 5f;

    [SerializeField, Range(0, 90)]
    float alignSmoothRange = 45;

    [SerializeField]
    LayerMask obstructionMask = 1;

    Quaternion gravityAlignment = Quaternion.identity;

    Quaternion orbitRotation;

    Camera regularCamera;

    Vector3 focusPoint, previousFocusPoint;

    Vector2 orbitAngles = new Vector2(45, 0);

    float lastManualRotationTime;

    private void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle)
        {
            maxVerticalAngle = minVerticalAngle;
        }
    }

    private void Awake()
    {
        regularCamera = GetComponent<Camera>();
        focusPoint = focus.position;
        transform.localRotation = orbitRotation = Quaternion.Euler(orbitAngles);
    }

    private void LateUpdate()
    {
        gravityAlignment = Quaternion.FromToRotation(gravityAlignment * Vector3.up, CustomGravity.GetUpAxis(focusPoint)) * gravityAlignment;
        UpdateFocusPoint();

        if (ManualRotation() || AutomaticRotation())
        {
            ContrainAngles();
            orbitRotation = Quaternion.Euler(orbitAngles);
        }
        Quaternion lookRotation = gravityAlignment * orbitRotation;
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;

        Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
        Vector3 rectPosition = lookPosition + rectOffset;
        Vector3 castFrom = focus.position;
        Vector3 castLine = rectPosition - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;

        if (Physics.BoxCast(
            focusPoint, CameraHalfExtends, -lookDirection, out RaycastHit hit, lookRotation, castDistance, obstructionMask
            ))
        {
            rectPosition = castFrom + castDirection * hit.distance;
            lookPosition = rectPosition - rectOffset;
        }

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    static float GetAngle(Vector2 direction)
    {
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        return direction.x < 0f  ? 360f - angle : angle;
    }

    Vector3 CameraHalfExtends
    {
        get
        {
            Vector3 halfExtends;
            halfExtends.y = regularCamera.nearClipPlane *
                Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
            halfExtends.x = halfExtends.y * regularCamera.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }

    void UpdateFocusPoint()
    {
        previousFocusPoint = focusPoint;
        Vector3 targetPoint = focus.position;

        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1 - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        else
        {
            focusPoint = targetPoint;
        }
    }

    void ContrainAngles()
    {
        orbitAngles.x = Mathf.Clamp( orbitAngles.x, minVerticalAngle, maxVerticalAngle );
        if (orbitAngles.y < 0f)
        {
            orbitAngles.y += 360;
        }
        else if (orbitAngles.y >= 360)
        {
            orbitAngles.y -= 360;
        }
        
    }

    bool AutomaticRotation()
    {
        if (Time.unscaledDeltaTime - lastManualRotationTime < alignDelay)
        {
            return false;
        }
        Vector3 alignedDelta = Quaternion.Inverse(gravityAlignment) * (focusPoint - previousFocusPoint);
        Vector2 movement = new Vector2(alignedDelta.x, alignedDelta.z);

        float movementDeltaSqr = movement.sqrMagnitude;
        if (movementDeltaSqr < 0.00001)
        {
            return false;
        }
        
        float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
        float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
        float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
        
        if (deltaAbs < alignSmoothRange)
        {
            rotationChange *= deltaAbs / alignSmoothRange;
        }
        else if (180 - deltaAbs < alignSmoothRange)
        {
            rotationChange *= (180 - deltaAbs) / alignSmoothRange;
        }
            orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
        return true;

    }

    bool ManualRotation()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Vertical"),
            Input.GetAxis("Horizontal")
        );
        const float e = 0.001f;
        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            lastManualRotationTime = Time.unscaledDeltaTime;
            return true;
        }
        return false;
    }
}