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
    [SerializeField]
    float forceDash;
    [SerializeField]
    float dashLenghtInMS;

    [SerializeField]
    private AK.Wwise.RTPC RPM;

    [SerializeField]
    private AK.Wwise.Event StartEngine;

    //Components
    private Rigidbody myRigidbody;

    //Variables
    private bool areControlsEnable = true;
    private float axisLeftTrigger;
    private float axisRightTrigger;
    private float axisHorizontal;
    [SerializeField]
    private float currentVelocity = 0f;
    private Vector3 velocityBeforeDash;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartEngine.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(areControlsEnable)
        {
            if (Input.GetButtonDown("ButtonX"))
                Dash();

            axisLeftTrigger = Input.GetAxis("LeftTrigger");
            axisRightTrigger = Input.GetAxis("RightTrigger");
            axisHorizontal = Input.GetAxis("Horizontal");
        }

        UpdatePlayerRPM();
    }

    private void FixedUpdate()
    {
        if (areControlsEnable)
        {
            Move();
            Rotate();
        }
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

    private void Dash()
    {
        EnableControls(false);
        velocityBeforeDash = myRigidbody.velocity;
        Vector3 velocityPlayer = transform.forward * forceDash;
        myRigidbody.velocity = new Vector3(velocityPlayer.x, myRigidbody.velocity.y, velocityPlayer.z);
        Invoke("StopDash", dashLenghtInMS);
    }

    private void StopDash()
    {
        EnableControls(true);
        //currentVelocity = myRigidbody.velocity.magnitude;
    }

    public void EnableControls(bool value)
    {
        areControlsEnable = value;
    }

    public void UpdatePlayerRPM()
    {
        RPM.SetValue(gameObject, currentVelocity*100 / maxSpeedPlayer);
    }

}
