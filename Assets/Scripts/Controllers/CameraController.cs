using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private float MoveSpeed;
    [SerializeField]private float MoveTime;
    //private bool isRotating;
    private Vector3 MovementVector  = new Vector3();
    private Quaternion RotationQuaternion = new Quaternion();


    private void Start()
    {
        MovementVector = gameObject.transform.position;
        RotationQuaternion = gameObject.transform.rotation;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            MovementVector += transform.forward * MoveSpeed;
        }

        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            MovementVector += transform.forward * -MoveSpeed;
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MovementVector += transform.right * -MoveSpeed;
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MovementVector += transform.right * MoveSpeed;
        }

        /*if(Input.GetKey(KeyCode.Q)&&!isRotating)
        {
            RotationQuaternion *= Quaternion.Euler(Vector3.up*90);
            isRotating = true;
        }

        if(Input.GetKey(KeyCode.E)&&!isRotating)
        {
            RotationQuaternion *= Quaternion.Euler(Vector3.up*-90);
            isRotating = true;
        }         

        if(RotationQuaternion == transform.rotation)
        {
            isRotating = false;
        }
        RotationHandler();*/
        transform.position = Vector3.Lerp(transform.position, MovementVector,Time.deltaTime* MoveTime);
    }
    
    private void RotationHandler()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation,RotationQuaternion,Time.deltaTime*MoveTime);
    }

}
