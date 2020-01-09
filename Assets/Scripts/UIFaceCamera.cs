using UnityEngine;
using System.Collections;
 
public class UIFaceCamera : MonoBehaviour
{
    private Camera _camera;
    

    private void  Awake()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        if(_camera == null){
            _camera = Camera.main;
            return;
        }
        
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
            _camera.transform.rotation * Vector3.up);
    }
}