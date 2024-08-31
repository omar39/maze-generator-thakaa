using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneratorAndSolverDemo.CameraBehaviour
{
    public class CameraTargetLook : MonoBehaviour
    {
        [SerializeField] private Transform lookTarget;
        [SerializeField] private Vector3 offset;
        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.position;    
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position + offset + initialPosition, lookTarget.position, Time.deltaTime) ;

        }
    }
}

