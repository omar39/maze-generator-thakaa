using System.Collections.Generic;
using MazeGeneratorAndSolverDemo.Maze;
using UnityEngine;

namespace MazeGeneratorAndSolverDemo.CameraBehaviour
{
    public class CameraMazeLook : MonoBehaviour
    {
        [SerializeField] private MazeGenerator mazeTarget;
        [SerializeField] private Vector3 offset;

        private Vector3 initialPosition;
        private bool mazeCompleted = false;
        private List<MazeCell> mazeCells = new List<MazeCell>();

        private void Start()
        {
            initialPosition = transform.position;
            mazeTarget.OnMazeGenerationComplete += CollectMazeInfo;
            mazeTarget.OnMazeGenerationReset += () => mazeCompleted = false;
        }
        private void LateUpdate()
        {
            if(mazeCompleted)
            {
                ViewCameraToMaze();
            }
        }
        private void CollectMazeInfo()
        {
            mazeCells = new List<MazeCell>();
            MazeCell cell = null;
            for(int i = 0;i < mazeTarget.transform.childCount;++i)
            {
                if(mazeTarget.transform.GetChild(i).TryGetComponent(out cell))
                {
                    mazeCells.Add(cell);
                }
            }
            mazeCompleted = true;
        }
        private void ViewCameraToMaze()
        {
            Vector3 lookingPoint = Vector3.zero;
            lookingPoint.y = mazeTarget.MazeWidth * .5f;

            foreach(var cell in mazeCells)
            {
                lookingPoint += cell.transform.position;
            }

            lookingPoint /= mazeCells.Count;

            transform.position = Vector3.Lerp(transform.position, lookingPoint + initialPosition + offset, Time.deltaTime * 5f);
        }
    }
}
