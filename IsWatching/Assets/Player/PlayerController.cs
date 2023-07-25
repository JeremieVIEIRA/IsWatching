using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] Transform playerCamera = null;
    [SerializeField] Camera CameraParametres;
    [SerializeField] float mouseSens = 3.5f;
    [SerializeField] float speed = 10f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] public Vector3 velo;


    [SerializeField][Range(0.0f, 0.5f)] float moveSmothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmothTime = 0.3f;

    [SerializeField] public bool isMoving;

    private float fovTargetSprint = 70f;
    private float fovTargetNormal = 60f;

    [SerializeField] float Stamina = 100.0f;
    float MaxStamina = 100.0f;


    float velocityY = 0.0f;

    CharacterController controller = null;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelo = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }



    // Update is called once per frame
    void Update()
    {
        UpdateMouvement();
        updateStamina();

        if (Stamina < 0.0f)
            Stamina = 0.0f;

        if (Stamina > 10.0f)
        {
            UpdateSprint();
        }
        else if (Stamina <= 0.0f)
        {
            speed = 10f;
            CameraParametres.fieldOfView = Mathf.Lerp(CameraParametres.fieldOfView, fovTargetNormal, 10 * Time.deltaTime);
        }


    }


    void DecreaseStamina()
    {
        if (Stamina != 0)
            Stamina -= 30 * Time.deltaTime;
    }

    void IncreaseStamina()
    {
        Stamina += 30 * Time.deltaTime;
    }


    void UpdateMouvement()
    {

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        if (targetDir.x == 0 && targetDir.y == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelo, moveSmothTime);

        if (Stamina > 8.0f)
            UpdateSprint();


        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        velo = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;


        controller.Move(velo * Time.deltaTime);

    }




    void UpdateSprint()
    {


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 15f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 10f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {

            if (CameraParametres.fieldOfView < fovTargetSprint)
            {
                CameraParametres.fieldOfView = Mathf.Lerp(CameraParametres.fieldOfView, fovTargetSprint, 10 * Time.deltaTime);
            }
        }
        else
        {
            if (CameraParametres.fieldOfView > fovTargetNormal)
            {
                CameraParametres.fieldOfView = Mathf.Lerp(CameraParametres.fieldOfView, fovTargetNormal, 10 * Time.deltaTime);
            }
        }
    }


    void updateStamina()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            DecreaseStamina();
        else if (Stamina <= MaxStamina)
            IncreaseStamina();
    }
}
