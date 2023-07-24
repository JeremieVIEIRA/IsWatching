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

    [SerializeField] GameObject lampe;

    [SerializeField][Range(0.0f, 0.5f)] float moveSmothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmothTime = 0.3f;

    [SerializeField] public bool isMoving;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            speed = 0;
        }
        else
        {
            isMoving = true;
            speed = 10;
        }
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelo, moveSmothTime);

        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;
        UpdateSprint();

        //Vector3 vitessePreChange = velo;

        velo = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;

/*        if (velo.x < vitessePreChange.x && Math.Abs(velo.x) < 0.5f)
        {
            velo.x = 0;
        }
*/

/*        if (Math.Abs(velo.x) > 0)
        {
            isMoving = true;
        }
        else
        {
            if (velo.x == 0)
            {
                isMoving = false;
            }
        }
*/        controller.Move(velo * Time.deltaTime);

    }

    void UpdateLamp()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isActive)
            {
                lampe.gameObject.SetActive(false);
                isActive = false;
            }
            else
            {
                lampe.gameObject.SetActive(true);
                isActive = true;
            }
        }
    }

    void UpdateSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            speed = 15;
            if (CameraParametres.fieldOfView < 70 && isMoving)
            {
                CameraParametres.fieldOfView += 0.5f;
            }
            else if (!isMoving)
            {
                isSprinting = false;
                speed = 10;
                if (CameraParametres.fieldOfView > 60)
                {
                    CameraParametres.fieldOfView -= 1;
                }
            }
        }
        else
        {
            isSprinting = false;
            speed = 10;
            if (CameraParametres.fieldOfView > 60)
            {
                CameraParametres.fieldOfView -= 1;
            }
        }
    }
}
