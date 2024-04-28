using UnityEngine;

public class MobileCameraOrbit : MonoBehaviour
{

    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 5.0f;
    public float ySpeed = 5.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public float zoomRate = 10.0f;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;

    private Vector3 FirstPosition;
    private Vector3 SecondPosition;
    private Vector3 delta;
    private Vector3 lastOffset;
    private Vector3 lastOffsettemp;
    //private Vector3 CameraPosition;
    //private Vector3 Targetposition;
    //private Vector3 MoveDistance;


    void Start() { Init(); }
    void OnEnable() { Init(); }

    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }

        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }

    /*
      * Camera logic on LateUpdate to only update after all character movement logic has been handled.
      */
    void LateUpdate()
    {
        // If Control and Alt and Middle button? ZOOM!
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);

            Touch touchOne = Input.GetTouch(1);



            Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;

            Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;



            float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;

            float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;



            float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;

            desiredDistance += deltaMagDiff * Time.deltaTime * zoomRate * 0.25f * Mathf.Abs(desiredDistance);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            desiredDistance += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * 0.25f * Mathf.Abs(desiredDistance);
        }

        if (Input.GetMouseButton(0))
        {
            var pos = Input.mousePosition;
            var dpiScale = 1f;
            if (Screen.dpi < 1) dpiScale = 1;
            if (Screen.dpi < 200) dpiScale = 1;
            else dpiScale = Screen.dpi / 200f;

            if (pos.x < 380 * dpiScale && Screen.height - pos.y < 250 * dpiScale) return;

            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.08f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.08f;
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
        }

        // If middle mouse and left alt are selected? ORBIT
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchposition = Input.GetTouch(0).deltaPosition;
            xDeg += touchposition.x * xSpeed * 0.002f;
            yDeg -= touchposition.y * ySpeed * 0.002f;
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

        }
        desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
        currentRotation = transform.rotation;
        rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        transform.rotation = rotation;


        if (Input.GetMouseButtonDown(1))
        {
            FirstPosition = Input.mousePosition;
            lastOffset = targetOffset;
        }

        if (Input.GetMouseButton(1))
        {
            SecondPosition = Input.mousePosition;
            delta = SecondPosition - FirstPosition;
            targetOffset = lastOffset + transform.right * delta.x * 0.003f + transform.up * delta.y * 0.003f;

        }

        ////////Orbit Position

        // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        position = target.position - (rotation * Vector3.forward * currentDistance);

        position = position - targetOffset;

        transform.position = position;




    }
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
