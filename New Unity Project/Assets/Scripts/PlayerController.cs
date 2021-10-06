using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    private Quaternion startingRotation;
    private float Vertical, Horizontal;
    private float RotationVertical, RotationHorizontal;
    private bool OnGround = false;
    public float Stamina = 100;

    public float speed = 2;
    public float jumpSpeed = 500;
    public GameObject Camera;
    public float sensivity = 1;

    void Start()
    {
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        MoveCamera();
        if (Stamina < 100)
            Stamina += 0.05f;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            OnGround = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            OnGround = false;
    }

    private void MoveCharacter()
    {
        if (OnGround)
        {
            Vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            Horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            if (Input.GetKeyDown(KeyCode.Space) && Stamina > 50) 
            {
                GetComponent<Rigidbody>().AddForce(0, jumpSpeed, 0);
                Stamina -= 30;
            }
        }
        transform.Translate(new Vector3(Horizontal, 0, Vertical));
    }

    private void MoveCamera()
    {
        (var rotationX, var rotationY) = GetRotation();
        Camera.transform.rotation = startingRotation * transform.rotation * rotationX;
        transform.rotation = startingRotation * rotationY;
    }

    private Tuple<Quaternion, Quaternion> GetRotation()
    {
        RotationHorizontal += Mathf.Clamp(Input.GetAxis("Mouse X"), -60, 60) * sensivity;
        RotationVertical += Mathf.Clamp(Input.GetAxis("Mouse Y"), -60, 60) * sensivity;
        var rotationY = Quaternion.AngleAxis(RotationHorizontal, Vector3.up);
        var rotationX = Quaternion.AngleAxis(-RotationVertical, Vector3.right);
        return new Tuple<Quaternion, Quaternion>(rotationX, rotationY);
    }


}
