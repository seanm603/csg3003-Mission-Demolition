using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    // fields set in Unity Inspector
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;

    //fields set dynamically
    [Header("Dynamic")]

    public GameObject launchPoint;
    public Vector3 launchPos;

    public GameObject projectile;
    public bool aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    void OnMouseEnter()
    {
        // print("SS: OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        // print("SS: OnMouseExit()");
        launchPoint.SetActive(false);
    }
    private void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(projectilePrefab);
        projectile.transform.position = launchPos;
    //    projectile.GetComponent<Rigidbody>.isKinematic = true;
    }

    void Update()
    {
        if (aimingMode || projectile != null) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;

        //float maxMagnitude = this.GetComponent<SphereCollider>.radius;
        //if (mouseDelta.magnitude > maxMagnitude)
        //{
        //    mouseDelta.Normalize();
        //    mouseDelta *= maxMagnitude;
        //}
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if(Input.GetMouseButtonDown(0))
        {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;
            projectile = null;

        }
    }

}
