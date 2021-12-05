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
    public static bool isPlayerCrouch ;
    public static int timeOfContact = 0;

    private float NormalSpeed = 3, FastedSpeed = 5, currentSpeed = 3;
    private float jumpSpeed = 150;
    public GameObject Camera;
    public float sensivity = 1;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        MoveCharacter();
        MoveCamera();
        if (Stamina < 100 && Stamina > 10)
            Stamina += 0.5f;
        else if (Stamina < 10)
            Stamina += 0.25f;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            OnGround = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            OnGround = true;
    }
    
    private IEnumerable<Vector3> GetSmooth(int koef)
    {
        for (var i = 0.5f; i <= 1; i += 0.1f)
            yield return new Vector3(1, i, 1);
    }

    private void GetSpeed()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = 2;
            GetComponent<CapsuleCollider>().transform.localScale = new Vector3(0.5f, Mathf.Lerp(0.5f, 0.05f, 10 * Time.deltaTime), 0.5f);
        }
        else
        {
            GetComponent<CapsuleCollider>().transform.localScale = new Vector3(0.5f, Mathf.Lerp(GetComponent<CapsuleCollider>().transform.localScale.y, 0.5f, 10 * Time.deltaTime), 0.5f);
            if (Stamina < 5)
                currentSpeed = 2;
            if (Stamina > 2)
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Stamina < 5)
                        currentSpeed = 2;
                    else
                        currentSpeed = FastedSpeed;
                    if (Stamina > 5)
                        Stamina -= 0.7f;
                }
                else
                    currentSpeed = NormalSpeed;
        }
    }

    private void MoveCharacter()
    {
        GetSpeed();

        if (!OnGround)
        {
            Vertical = Input.GetAxis("Vertical") * Time.deltaTime * currentSpeed;
            Horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * currentSpeed;
            if (Input.GetKey(KeyCode.Space) && Stamina > 50) 
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
