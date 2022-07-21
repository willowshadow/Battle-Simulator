using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public Vector3 direction;

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime);
    }
}
