using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{

    public Rigidbody rb;
    public float wingSpan = 13.56f;
    public float wingArea = 78.04f;

    private float aspectRatio;
    public float EnginePower;

    public float pitchSpeed;
    public float rollSpeed;

    private void Awake()
    {
        rb.drag = Mathf.Epsilon;
        aspectRatio = (wingSpan * wingSpan) / wingArea;
    }
    private void FixedUpdate()
    {
        var pitch = Input.GetAxis("Vertical");
        var roll = -Input.GetAxis("Horizontal");
        transform.Rotate(new Vector3(pitch*Time.deltaTime*pitchSpeed, 0, roll*Time.deltaTime*rollSpeed),Space.Self);
        calculateForces();
    }
    private void calculateForces()
    {
        // *flip sign(s) if necessary*
        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        var angleOfAttack = Mathf.Atan2(-localVelocity.y, localVelocity.z);

        // ? * 2 * PI * (AR / AR + 2)
        var inducedLift = angleOfAttack * (aspectRatio / (aspectRatio + 2f)) * 2f * Mathf.PI;

        // CL ^ 2 / (AR * PI)
        var inducedDrag = (inducedLift * inducedLift) / (aspectRatio * Mathf.PI);

        // V ^ 2 * R * 0.5 * A
        var pressure = rb.velocity.sqrMagnitude * 1.2754f * 0.5f * wingArea;

        var lift = inducedLift * pressure;
        var drag = (0.021f + inducedDrag) * pressure;

        // *flip sign(s) if necessary*
        var dragDirection = rb.velocity.normalized;
        var liftDirection = Vector3.Cross(dragDirection, transform.right);

        // Lift + Drag = Total Force
        rb.AddForce(liftDirection * lift - dragDirection * drag);
        rb.AddForce(transform.forward * EnginePower);
    }
}
