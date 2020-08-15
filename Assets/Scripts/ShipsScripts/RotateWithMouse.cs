using UnityEngine;
using System.Collections;

public class RotateWithMouse : MonoBehaviour
{
    //public float speed;
    public float Acceleration = 2.0f;
    public float MaxSpeed = 1.5f;

    //public float speedV = 2.0f;
    public bool xAxis = true;
    public bool yAxis = true;

    //private float yaw = 0.0f;
    //private float pitch = 0.0f;

    //private float rotX = 0;
    //private float rotY = 0;

    // Camera Rotation when not moving
    public bool IsMoving = true;
    public float OrbitSpeed = 10f;
    public Camera MainCamera;

    private bool hasTarget = false;
    //private bool wasStopped = false;

    private Rigidbody rb;
    private Vector2 mouseInput;
    public Vector3 Tensor; // 500, 500, 500

    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.inertiaTensor = Tensor;
    }

    void Update()
    {
        Cursor.visible = false;

        rb.maxAngularVelocity = MaxSpeed;
        hasTarget = gameObject.GetComponent<FindTarget>().haveTarget;

        if (!hasTarget)
        {
            rb.inertiaTensorRotation = Quaternion.identity;
        }
        
    }

    void FixedUpdate()
    {
        if (!hasTarget)
        {
            mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (xAxis) rb.AddTorque(transform.up * mouseInput.x * Acceleration, ForceMode.Acceleration);
        }
        else
        {
            mouseInput = new Vector2(0, 0);
        }

        //if (IsMoving)
        //{
            
        //}
        //else
        //{
        //    var offset = new Vector3(this.transform.position.x, this.transform.position.y + 8.0f, this.transform.position.z + 7.0f);
        //    offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * OrbitSpeed, Vector3.up) * offset;
        //    MainCamera.transform.position = this.transform.position + offset;
        //    MainCamera.transform.LookAt(this.transform.position);
        //}

    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 45, Screen.width, Screen.height), "MouseX: " + mouseInput.x.ToString() + " - MouseY:" + mouseInput.y);
    }
}