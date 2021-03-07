using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [Header("Car Controller Reference")]
    public Rigidbody carController;

    [Header("Car Settings")]
    public float forwardAcceleration = 8f;
    public float reverseAcceleration  = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 10f, groundRayLength = .5f;

    [Header("Raycast Settings")]
    public LayerMask groundObjects;
    public Transform groundRayPoint;

    [Header("Speedometer Text Reference")]
    public Text speedometer;

    private bool grounded;
    private float speedInput, turnInput, dragOnGround;
    private string carSpeed;
    private int direction = 0;
    // Start is called before the first frame update
    void Start()
    {
        carController.transform.parent = null;
        dragOnGround = carController.drag;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAcceleration * 1000f;
        } else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAcceleration * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");

        // Update the Speedometer
        var kph = carController.velocity.magnitude * 3.6;
        carSpeed = kph.ToString("0");
        speedometer.text = carSpeed + " km/h";
    }

    void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, groundObjects))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        if (grounded)
        {
            carController.drag = dragOnGround;
            if (Mathf.Abs(speedInput) > 0)
            {
                carController.AddForce(transform.forward * speedInput);
                if (transform.InverseTransformDirection(carController.velocity).z > 0)
                {
                    direction = 1;
                }
                else if (transform.InverseTransformDirection(carController.velocity).z < 0)
                {
                    direction = -1;
                }
            }
            if (carSpeed == "0")
            {
                direction = 0;
            }
        } else
        {
            carController.drag = 0.1f;
            carController.AddForce(Vector3.down * gravityForce * 100f);
        }
        
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * direction, 0f));
        transform.position = carController.transform.position;
    }
}
