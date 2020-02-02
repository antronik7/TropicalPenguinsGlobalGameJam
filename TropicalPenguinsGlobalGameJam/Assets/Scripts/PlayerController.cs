using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Instances
	public PickUpToolController pickUpController;

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
	float dashDelay = 3f;

	//Sounds
	[SerializeField]
	private AK.Wwise.RTPC RPM;
	[SerializeField]
	private AK.Wwise.Event StartEngine;
	[SerializeField]
	private AK.Wwise.Event PlayerImpact;
	[SerializeField]
	private AK.Wwise.Event BeaverShout;
	[SerializeField]
	private AK.Wwise.Event BlockBreak;
	[SerializeField]
	private AK.Wwise.Event Boost;
	[SerializeField]
	private AK.Wwise.Event WallHit;

	//FX
	[SerializeField]
	private ParticleSystem ImpactParticles;

	//Components
	private Rigidbody myRigidbody;

	//Variables
	public int playerId { get; private set; }
	private bool isDashing = false;
	private bool canDestroyBlock = true;
	private bool areControlsEnable = true;
	[SerializeField]
	private float currentVelocity = 0f;
	private Vector3 velocityBeforeDash;
	Vector3 lastPosition = Vector3.zero;
	float dashTimeStamp = 0f;

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
		UpdatePlayerRPM();
	}

	void OnCollisionEnter(Collision collision)
	{
		DestroyBlock(collision);

		if (collision.gameObject.CompareTag("Wall"))
		{
			WallHit.Post(gameObject);
		}
	}

	void OnCollisionStay(Collision collision)
	{
		DestroyBlock(collision);
	}

	void DestroyBlock(Collision collision)
	{
		if (isDashing)
		{
			if (canDestroyBlock)
			{
				PlayerController controller;
				if (collision.transform.GetComponent<Shape>() != null)
				{
					collision.transform.GetComponent<Shape>().Crumble();
					BlockBreak.Post(gameObject);
				}
				else if (controller = collision.transform.GetComponent<PlayerController>())
				{
					PlayerImpact.Post(gameObject);
					Debug.Log("Collision");
					ImpactParticles.Play();
					collision.gameObject.GetComponent<PlayerController>().ImpactParticles.Play();
					Shape shapeToCrumble = controller.pickUpController.GetHoldedShape();

					if (shapeToCrumble != null) {
						shapeToCrumble.Crumble(controller);
						BeaverShout.Post(gameObject);
					}
				}

				canDestroyBlock = false;
			}
		}
	}

	public void Move(float input)
	{
		//float signVelocity = Mathf.Sign(Vector3.Dot(myRigidbody.velocity.normalized, transform.forward));
		//myRigidbody.velocity = transform.forward * myRigidbody.velocity.magnitude * signVelocity;

		//myRigidbody.AddForce(transform.forward * axisRightTrigger * 40f);
		//myRigidbody.AddForce(transform.forward * axisLeftTrigger * 25f * -1f);

		//if (Mathf.Abs(myRigidbody.velocity.magnitude) >= maxSpeedPlayer)
		//{
		//    myRigidbody.velocity = maxSpeedPlayer * transform.forward * signVelocity;
		//}

		//return;
		if (isDashing || !areControlsEnable)
			return;

		float speed = (transform.position - lastPosition).magnitude;
		if (speed <= 0.001f)
		{
			currentVelocity = 0f;
		}

		lastPosition = transform.position;

		float movementVelocity = input * speedPlayer * Time.deltaTime;
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

		myRigidbody.velocity = new Vector3(velocityPlayer.x, myRigidbody.velocity.y, velocityPlayer.z);
	}

	public void Rotate(float input)
	{
		if (isDashing || !areControlsEnable)
			return;

		float turn = input * turnSpeedPlayer * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
		myRigidbody.MoveRotation(myRigidbody.rotation * turnRotation);
	}

	public void Dash(bool input)
	{
		if (Time.timeSinceLevelLoad < dashTimeStamp || input == false)
			return;

		dashTimeStamp = Time.timeSinceLevelLoad + dashDelay;
		isDashing = true;
		EnableControls(false);
		velocityBeforeDash = myRigidbody.velocity;
		Vector3 velocityPlayer = transform.forward * forceDash;
		myRigidbody.velocity = new Vector3(velocityPlayer.x, myRigidbody.velocity.y, velocityPlayer.z);
		Boost.Post(gameObject);
		Invoke("StopDash", dashLenghtInMS);
	}

	private void StopDash()
	{
		isDashing = false;
		EnableControls(true);
		canDestroyBlock = true;
		//currentVelocity = myRigidbody.velocity.magnitude;
	}

	public void EnableControls(bool value)
	{
		areControlsEnable = value;
		myRigidbody.velocity = Vector3.zero;
	}

	public void UpdatePlayerRPM()
	{
		RPM.SetValue(gameObject, Mathf.Abs(currentVelocity) * 100 / maxSpeedPlayer);
	}

	public void SetId(int id)
	{
		playerId = id;
	}
}
