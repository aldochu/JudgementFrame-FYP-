using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    private Vector3 movementVector;
    private float forward;
    private float sideways;
    private CharacterController characterController;
    private float movementSpeed = 8;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float mousesensitivityX = 1F;
    public float mousesensitivityY = 1F;

    public float joysensitivityX = 1F;
    public float joysensitivityY = 1F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    private Rigidbody rigidBody;
    private float jumpPower = 15;
    private float gravity = 40;
    // Use this for initialization
    float ltaxis; 
    float rtaxis; 
    void Start () {
       
        characterController = GetComponent<CharacterController>();
           
    }
	
	// Update is called once per frame
	void Update () {

        ltaxis = Input.GetAxis("Left Trigger");
        rtaxis = Input.GetAxis("Right Trigger");

        var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // Rotate around y - axis

        if (directionVector != Vector3.zero)
        {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            var directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength * directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector * directionLength;

            characterController.Move(transform.rotation * directionVector * Time.deltaTime);
        }
            //movement
            movementVector.x = transform.forward.x*Input.GetAxis("LeftJoystickX") * movementSpeed;
        movementVector.z = Input.GetAxis("LeftJoystickY") * movementSpeed;
       // characterController.Move(movementVector * Time.deltaTime);
        //  forward = Input.GetAxis("LeftJoystickX") * movementSpeed;
        //   sideways=Input.GetAxis("LeftJoystickY") * movementSpeed;
        if (characterController.isGrounded)
        {
            movementVector.y = 0;
            if (Input.GetButtonDown("A"))
            {
                movementVector.y = jumpPower;
            }
           else if (Input.GetButtonDown("B"))
            {
                movementVector.y = jumpPower;

            }
           else if (Input.GetButtonDown("Y"))
            {
                movementVector.y = jumpPower;
            }
           else  if (Input.GetButtonDown("X"))
            {
                movementVector.y = jumpPower;
            }
            //Left Trigger
           else if (ltaxis>0)
            {
                movementVector.y = jumpPower;
            }
            //Right Trigger
           else if (rtaxis>0)
            {
                movementVector.y = jumpPower;
            }
        }
        movementVector.y -= gravity * Time.deltaTime;
        characterController.Move(movementVector * Time.deltaTime);
      


        //camera rotation 
        float Xon = Mathf.Abs(Input.GetAxis("RightJoystickX"));
        float Yon = Mathf.Abs(Input.GetAxis("RightJoystickY"));

        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("RightJoystickX") * mousesensitivityX;

            rotationY += Input.GetAxis("RightJoystickY") * mousesensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            if (Xon > .05)
            {
                transform.Rotate(0, Input.GetAxis("RightJoystickX") * joysensitivityX, 0);
            }
            transform.Rotate(0, Input.GetAxis("RightJoystickX") * mousesensitivityX, 0);
        }
        else
        {
            if (Yon > .05)
            {
                rotationY += Input.GetAxis("RightJoystickY") * joysensitivityY;
            }
            rotationY += Input.GetAxis("RightJoystickY") * mousesensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);

        }
    }
}
