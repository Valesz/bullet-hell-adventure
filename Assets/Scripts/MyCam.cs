using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCam : MonoBehaviour
{
    [Header("Follow")]
    public Transform target;
    public Vector3 offset;
    public float followSpeed;
    public bool onPlayer = true;
    [SerializeField] float volume;
    [Header("Zoom")]
    [SerializeField] float zoomMin;
    [SerializeField] float zoomMax;
    public float zoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        if (target)
        {
            FollowTarget();
        }
    }

    void FollowTarget()
    {
        if (onPlayer)
        {
            Vector3 offsetted = target.position + offset;
            Vector3 lookAhead = offsetted +( Camera.main.ScreenToWorldPoint(Input.mousePosition) - target.position).normalized * volume;
            lookAhead.z = offset.z;
            Vector3 anchorPos = transform.position - lookAhead;
            transform.position -= anchorPos * (followSpeed) * Time.deltaTime;
            
        }
    }

    void Zoom()
    {
        Camera.main.orthographicSize -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        Camera.main.orthographicSize = Mathf.Min(zoomMax, Mathf.Max(zoomMin, Camera.main.orthographicSize));
    }
}
