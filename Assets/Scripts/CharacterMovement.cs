using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";

    void Update()
    {
        if (m_inpul_left.GetComponent<MobileInputController>().Vertical != 0 || m_inpul_left.GetComponent<MobileInputController>().Horizontal != 0)
        {
            controller.Move(transform.forward * m_inpul_left.GetComponent<MobileInputController>().Vertical * Time.deltaTime * speed + transform.right * m_inpul_left.GetComponent<MobileInputController>().Horizontal * Time.deltaTime * speed);
        }

        if (m_inpul_Right.GetComponent<MobileInputController>().Vertical != 0 || m_inpul_Right.GetComponent<MobileInputController>().Horizontal != 0)
        {
            rotation.x += m_inpul_Right.GetComponent<MobileInputController>().Horizontal * sensitivity;
            rotation.y += m_inpul_Right.GetComponent<MobileInputController>().Vertical * sensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

            transform.localRotation = xQuat * yQuat; //Quaternions seem to rotate more consistently than EulerAngles. Sensitivity seemed to change slightly at certain degrees using Euler. transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        }

    }
    [Header("Movement Attribute")]
    public float speed = 5;
    public float rotAngle = 0;
    public float turnSmoothTime = 0.2f;
    public Vector3 forward;
    float turnnSmoothVelocity;
    [Header("Reference")]
    CharacterController controller;
    public float inputHorizontal;
    public float inputVertical;
   
    [Header("Body parts reference")]

    //public GameObject upperBody;
    //public GameObject lowerBody;
    public GameObject m_inpul_left;
    public GameObject m_inpul_Right;

    [Header("Platform")]
    public bool PC = true;
	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        
	}
	
	//// Update is called once per frame
	//void Update () {
 //       inputHorizontal = Input.GetAxisRaw("Vertical");

 //       if(!PC)
 //           controller.Move(Vector3.forward * m_inpul_left.GetComponent<MobileInputController>().Vertical * Time.deltaTime * speed+ Vector3.right * m_inpul_left.GetComponent<MobileInputController>().Horizontal * Time.deltaTime * speed);
 //       else
 //           controller.Move(Vector3.forward * Input.GetAxisRaw("Vertical") * Time.deltaTime * speed + Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed);

 //       Vector3 from = new Vector3(0f,0f,1f);
 //       Vector3 to = new Vector3(m_inpul_Right.GetComponent<MobileInputController>().Horizontal,0f, m_inpul_Right.GetComponent<MobileInputController>().Vertical);
 //       //forward = lowerBody.transform.forward;

        

 //       if (m_inpul_Right.GetComponent<MobileInputController>().Horizontal != 0 && m_inpul_Right.GetComponent<MobileInputController>().Vertical != 0)
 //       {
 //           float angle = Vector3.SignedAngle(from, to, Vector3.up);
 //           rotAngle = angle;


 //          // if (angle > 60 || angle <-60)
 //           //{
 //            //   float targetRotation = Mathf.Atan2(m_inpul_Right.GetComponent<MobileInputController>().Horizontal, m_inpul_Right.GetComponent<MobileInputController>().Vertical) * Mathf.Rad2Deg;
 //              transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.z, angle, ref turnnSmoothVelocity, turnSmoothTime);
 //           // }

 //         //  float targetRotationForUpperBody = Mathf.Atan2(m_inpul_Right.GetComponent<MobileInputController>().Horizontal, m_inpul_Right.GetComponent<MobileInputController>().Vertical) * Mathf.Rad2Deg;
 //          // transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(upperBody.transform.eulerAngles.y, targetRotationForUpperBody, ref turnnSmoothVelocity, turnSmoothTime);

 //          // transform.eulerAngles = new Vector3(0f, angle, 0f);

 //       }
        



    //}

}
