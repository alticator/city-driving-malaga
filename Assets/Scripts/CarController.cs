using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public Rigidbody carController;
    public float forwardAcceleration = 8f, reverseAcceleration  = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 10f, groundRayLength = .5f;
    public LayerMask groundObjects;
    public Transform groundRayPoint;

    private bool grounded;
    private float speedInput, turnInput, dragOnGround;
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
            }
        } else
        {
            carController.drag = 0.1f;
            carController.AddForce(Vector3.down * gravityForce * 100f);
        }
        
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        transform.position = carController.transform.position;
    }
}
