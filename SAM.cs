using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SAM : MonoBehaviour
{
    public Missile missileToLaunch;
    public Collider[] targets;

    public Transform[] launchPoints;
    public int currentLauncherIndex;

    [Header("Targeting Parameters")]
    public float detectionRange;
    public float firingRange;
    private int selectedTarget=0;
    float timeSincelaunch;

    [Header("Debugs")]
    public Color detectionRangeColor;
    public Color firingRangeColor;
    public LayerMask targetLayer;

    public void Awake()
    {
        currentLauncherIndex = 0;
    }
    private void Start()
    {
        StartCoroutine(nameof(DetectTarget));
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LaunchMissile();
        }
    }
    private IEnumerator DetectTarget()
    {
        
        while (true)
        {
            _ = Physics.OverlapSphereNonAlloc(transform.position, detectionRange, targets, targetLayer);

            yield return new WaitForSecondsRealtime(2f);

            if (targets[selectedTarget]!=null && timeSincelaunch+3f< Time.time)
            {
                LaunchMissile();
                timeSincelaunch = Time.time;
            }
            targets = new Collider[5];
        }
     
    }
    
    private void LaunchMissile()
    {
        if (targets[0] == null) return;

        var missile = Instantiate(missileToLaunch, launchPoints[currentLauncherIndex].position, missileToLaunch.transform.rotation);
        missile.target = targets[selectedTarget].transform;
        currentLauncherIndex++;

        if (currentLauncherIndex >= launchPoints.Length)
            currentLauncherIndex = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectionRangeColor;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = firingRangeColor;
        Gizmos.DrawWireSphere(transform.position, firingRange);
    }
}
