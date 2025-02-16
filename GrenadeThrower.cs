using System;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Grenade prefab")]
    [SerializeField] private GameObject grenadePrefab;   

    [Header("Grenade Seting")]
    [SerializeField] private KeyCode throwKey = KeyCode.G;
    [SerializeField] private Transform throwPosition; //postion refrence of throw
    [SerializeField] private Vector3 throwDirection = new Vector3(0, 1, 0);// direction of throw

    [Header("Grenade Force")]
    [SerializeField] private float throwForce = 10f; // force aplide to grenade
    [SerializeField] private float maxForce = 20f; // maximum force apllied to thow grenadea

    [Header("Trajectory settings")]
    [SerializeField] private LineRenderer trajectoryLine; //refrences linerenderer componet
    private bool isCharging = false; // check if charging throw
    private float chargeTime = 0f; //time player has been charging]
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey))
        {
            StartThrowing();
        }
        if (isCharging)
        {
            ChargeThrow();
        }
        if (Input.GetKeyUp(throwKey))
        {
            ReleaseThrow();
        }
    }

    void StartThrowing()
    {
        // FULL PIN SOWND
        isCharging = true;
        chargeTime = 0f;

        trajectoryLine.enabled = true;
    }

    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;

        Vector3 grenadeVelocity = (mainCamera.transform.forward + throwDirection).normalized * Mathf.Min(chargeTime * throwForce, maxForce);
        ShowTrajectory(throwPosition.position + throwPosition.forward,  grenadeVelocity);
    }

    void ReleaseThrow()
    {
        ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
        isCharging = false;

        trajectoryLine.enabled = false;
    }


    void ThrowGrenade(float force)
    {
        Vector3 spawnPosition = throwPosition.position + mainCamera.transform.forward;

        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, mainCamera.transform.rotation);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        Vector3 finalThrowDirection = (mainCamera.transform.forward + throwDirection).normalized;
        rb.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);

        // throwing sound
    }

    void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];
        trajectoryLine.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + 0.5f * Physics.gravity * time * time;
        }
        trajectoryLine.SetPositions(points);
    }
}
