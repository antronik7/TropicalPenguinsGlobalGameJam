using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Values
    [SerializeField]
    float speedPlayer;
    [SerializeField]
    float turnSpeedPlayer;
    [SerializeField]
    float maxSpeedPlayer;
    [SerializeField]
    float speedDecreasePlayer;

    //Components
    private Rigidbody myRigidbody;

    //Variables
    private float axisLeftTrigger;
    private float axisRightTrigger;
    private float axisHorizontal;
    private float currentVelocity = 0f;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        axisLeftTrigger = Input.GetAxis("LeftTrigger");
        axisRightTrigger = Input.GetAxis("RightTrigger");
        axisHorizontal = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float movementVelocity = (axisRightTrigger * speedPlayer * Time.deltaTime) + (axisLeftTrigger * speedPlayer * Time.deltaTime * -1f);
        currentVelocity += movementVelocity;

        if (Mathf.Abs(movementVelocity) == 0f || Mathf.Sign(currentVelocity) != Mathf.Sign(movementVelocity))
        {
            float currentVelocityAbsolute = Mathf.Abs(currentVelocity) - speedDecreasePlayer;
            

            if (currentVelocityAbsolute < 0f)
                currentVelocityAbsolute = 0f;

            currentVelocity = currentVelocityAbsolute * Mathf.Sign(currentVelocity);
        }

        if (Mathf.Abs(currentVelocity) >= maxSpeedPlayer)
        {
            currentVelocity = maxSpeedPlayer * Mathf.Sign(movementVelocity);
        }

        Vector3 velocityPlayer = transform.forward * currentVelocity;

        myRigidbody.velocity = new Vector3 (velocityPlayer.x, myRigidbody.velocity.y, velocityPlayer.z);
    }

    private void Rotate()
    {
        float turn = axisHorizontal * turnSpeedPlayer * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        myRigidbody.MoveRotation(myRigidbody.rotation * turnRotation);
    }

}
