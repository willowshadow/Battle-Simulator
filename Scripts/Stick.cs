using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [Range(100,10000)]
    public float force = 1000f;
    [SerializeField] LayerMask otherballLayer;
    [SerializeField] Ball ball;
    [SerializeField] Rigidbody rb;


    ///////////////////////////////

    public Vector3 startTouch;
    public Vector3 endTouch;
    public void Update()
    {
       
        CalculateRotation();
        

        /*var mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePositionWorld.x,otherballLayer,mousePositionWorld.z);
        var direction = ball.transform.position - transform.position;

        var hit = Physics.Raycast(transform.position,transform.forward,out RaycastHit hitInfo,100, otherballLayer);
          
        if(hit)
        {
            Debug.DrawLine(transform.position, hitInfo.point);
            Vector3 deflection = Vector3.Reflect(direction,hitInfo.normal);
            Debug.DrawLine(hitInfo.transform.position, deflection);
        }*/
        Ray a = new Ray(transform.position, transform.forward);
       
        PlayShot(a.direction);
        if (Physics.SphereCast(a,0.5f,out RaycastHit hitInfo,1000))
        {

            Debug.DrawLine(transform.position, transform.position + a.direction);
            Vector3 deflection = Vector3.Reflect(a.direction, hitInfo.normal);
            var deflected = new Ray(hitInfo.point, deflection);
            Debug.DrawLine(hitInfo.point, deflected.direction);
            //Debug.DrawLine(hitInfo.transform.position,deflection)
        }

        void CalculateRotation()
        {

            if (Input.GetMouseButtonDown(0))
            {
                startTouch = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                endTouch = Input.mousePosition;
            }
            if(Input.GetMouseButtonUp(0))
            {
                startTouch = Vector3.zero;
                endTouch = Vector3.zero;
            }
            var changeDelta = endTouch - startTouch;
           
            if (changeDelta.magnitude > 2f)
            {
                transform.Rotate(Vector3.up*Time.deltaTime*changeDelta.magnitude);
             
            }
        }
        void PlayShot(Vector3 direction)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            { 
                rb.AddForce(direction* force);
                Invoke(nameof(ResetRotation), 4f);
               
            }
          
        }
       
    }
    void ResetRotation()
    {
        rb.isKinematic = true;
      
        rb.rotation = Quaternion.Euler(Vector3.zero);

        rb.isKinematic = false;
    }


}
