using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float targetHeight = 1.7f;
    public float distance = 5.0f;
    public float offsetFromWall = 0.1f;
    public float maxDistance = 7.0f;
    public float minDistance = 2.0f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;
    public float zoomRate = 40f;
    public float rotationDampening = 3.0f;
    public float zoomDampening = 5.0f;
    public LayerMask collisionLayers = -1;
    public bool lockToRearOfTarget = false;
    public bool allowMouseInputX = true;
    public bool allowMouseInputY = true;
    public bool useController = false;
    public bool rotateBehind;

    private float delay;
    public float maxDelay;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;
    new private Rigidbody rigidbody;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;
        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;
        rigidbody = GetComponent<Rigidbody>();
        useController = false;

        foreach (string joystick in Input.GetJoystickNames())
        {
            if (useController)
                continue;

            useController = (joystick != string.Empty);
        }

        Cursor.visible = !useController;

        if (rigidbody)
            rigidbody.freezeRotation = true;
    }

    void LateUpdate()
    {
        if (PauseManager.instance.state == PauseState.Pause) return;

        if (!target)
            return;

        Vector3 vTargetOffset;

        if (useController)
        {
            if (allowMouseInputX)
                xDeg += Input.GetAxis("RightStickX") * xSpeed * 0.02f;
            else
                RotateBehindTarget();

            if (allowMouseInputY)
                yDeg -= Input.GetAxis("RightStickY") * ySpeed * 0.02f;
        }
        else
        {
            if (GUIUtility.hotControl == 0 && Input.GetMouseButton(0))
            {
                if (allowMouseInputX)
                    xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                else
                    RotateBehindTarget();

                if (allowMouseInputY)
                    yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
            else
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");

                if (h == 0.0f && v == 0.0f)
                {
                    if (delay >= 0.0f)
                        delay -= Time.deltaTime;
                    else
                        RotateBehindTarget();
                }
                else
                {
                    delay = maxDelay;
                }
            }
        }

        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);


        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        correctedDistance = desiredDistance;

        vTargetOffset = new Vector3(0, -targetHeight, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(target.position.x, target.position.y + targetHeight, target.position.z);

        bool isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers))
        {
            correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
            isCorrected = true;
        }

        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;


        position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

    void RotateBehindTarget()
    {
        float targetRotationAngle = target.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;

        xDeg = Mathf.Lerp(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);

        if (targetRotationAngle == currentRotationAngle)
        {
            if (!lockToRearOfTarget)
                rotateBehind = false;
        }
        else
            rotateBehind = true;
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

}
