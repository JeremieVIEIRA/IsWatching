using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobing : MonoBehaviour
{

    [SerializeField] private bool _enable = true;

    [SerializeField, Range(0, 0.1f)] public float amplitude = 0.0144f; 
    [SerializeField, Range(0, 30)] public float frequency = 10f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform cameraHolder = null;

    private float toogleSpeed = 3.0f;
    private Vector3 startPos;
    private CharacterController controller;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        startPos = _camera.localPosition;
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(controller.velocity.x,0,controller.velocity.z).magnitude;
        if (speed < toogleSpeed) return;
        if (!controller.isGrounded) return;
        PlayMotion(FootStepMotion());   
    }

    private void ResetPosition()
    {
        if(_camera.localPosition == startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(!enabled) return;
        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());

    }
}
