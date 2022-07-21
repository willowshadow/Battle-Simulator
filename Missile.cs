using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Missile : MonoBehaviour
{
    public Transform target;
    public Rigidbody rb;

    public float targetFollowAccuracy=1f;

    public float angleChangingSpeed;
    public float acceleration = 1f;
    public float maxVelocity =1f;
    public float velocity =1f;
    public float initialLaunchForce;

    public float activationDelay;
    public float boostDelay;
    public float boosterDuration;

    //Fins
    public Transform[] fins;
    //Target Stats
    private Vector3 targetMovementDir;
    private Vector3 targetMovementDirPrev;
    //Visuals
    public ParticleSystem trail;
    public ParticleSystem explosion;
  

    private void Update()
    {
       // targetMovementDirPrev = target.position;
       // targetMovementDir = target.position - targetMovementDirPrev;
    }

    public void Start()
    {
        InitialLaunch();
    }

    public void InitialLaunch()
    {
        rb.AddForce(transform.forward*initialLaunchForce);
        foreach (var fin in fins)
        {
            fin.DOLocalRotate(new Vector3(0f, 0f, fin.rotation.eulerAngles.y - 90f), 1f, RotateMode.FastBeyond360);
        }
        Invoke(nameof(InitiateTracking), activationDelay);
    }
    public void InitiateTracking()
    {
        trail.Play();
        StartCoroutine(nameof(TrackingSequence));
    }
    private IEnumerator TrackingSequence()
    {
        var launchTime = Time.time;
        
        while (launchTime+boostDelay>Time.time)
        {
            if (target != null)
            {
                //Look at target - rotate to target
                Vector3 relativePos = (target.position + targetMovementDir)- transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos,Vector3.up);
                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, toRotation, Time.deltaTime);
            }
            yield return null;
        }
        
        StartCoroutine(nameof(BoostSequence));
        while (true)
        {
            if (target != null)
            {
                //Look at target - rotate to target
                Vector3 relativePos = target.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos,Vector3.up);
                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, toRotation, targetFollowAccuracy * Time.deltaTime);
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator BoostSequence()
    {
        var accRate = 0f;
        while (boosterDuration>0)
        {
            if (target != null)
            {
                //Move to target
                velocity = Mathf.Lerp(velocity, maxVelocity, Time.deltaTime * accRate);
                rb.velocity = transform.forward * velocity;
                if (accRate < acceleration)
                    accRate += Time.deltaTime;
            
            }
            boosterDuration -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Target"))
        {
            trail.transform.parent = null;
            trail.Stop();
            Destroy(gameObject);
            Instantiate(explosion,collision.transform.position,Quaternion.identity,null);
        }
    }
}
