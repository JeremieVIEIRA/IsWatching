using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public AudioPlayerScript audioScript;

    [SerializeField] Transform playerCamera = null;
    [SerializeField] Camera CameraParametres;
    [SerializeField] float mouseSens = 3.5f;
    [SerializeField] float speed = 10f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] public Vector3 velo;

    [SerializeField] GameObject lampe;
    HeadBobing headbobing;

    [SerializeField][Range(0.0f, 0.5f)] float moveSmothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmothTime = 0.3f;

    [SerializeField] public bool isMoving;

    private float fovTargetSprint = 70f;
    private float fovTargetNormal = 60f;

    bool isSprinting = false;
    bool isActive = true;
    float cameraPitch = 0.0f;
    float velocityY = 0.0f;

    CharacterController controller = null;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelo = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelo = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        headbobing = GetComponent<HeadBobing>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void ResetBobing()
    {
        headbobing.amplitude = 0.0144f;
        headbobing.frequency = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouse();
        UpdateMouvement();
        UpdateLamp();
        UpdateSprint();
    }

    void UpdateMouse()
    {

        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelo, mouseSmothTime);

        cameraPitch -= targetMouseDelta.y * mouseSens;


        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * targetMouseDelta.x * mouseSens);

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

        UpdateSprint();

        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        velo = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;


        controller.Move(velo * Time.deltaTime);

    }

    void UpdateLamp()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isActive)
            {
                lampe.gameObject.SetActive(false);
                isActive = false;
                audioScript.FlashOff();
            }
            else
            {
                lampe.gameObject.SetActive(true);
                isActive = true;
                audioScript.FlashOn();
            }
        }
    }


    void UpdateSprint()
    {

        if(Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            speed = 15;
            headbobing.frequency = 16f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 10;
            ResetBobing();
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
}
