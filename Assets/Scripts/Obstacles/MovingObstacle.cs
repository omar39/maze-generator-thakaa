using System.Collections;
using UnityEngine;

namespace MazeGeneratorAndSolverDemo.Obstacles
{
    public enum MovementDirection
    {
        Vertical,
        Horizontal
    }
    public class MovingObstacle : Obstacle
    {
        [SerializeField] private float speed = 5f, rotationalSpeed = 10f;
        [SerializeField] private float stoppingDistance = 0.1f;
        private Vector3 forwardConstraint, backwardConstraint, leftConstraint, rightConstraint;
        private MovementDirection movementDirection = MovementDirection.Horizontal;
        private void Start()
        {
            // get the moving directions by casting rays
            GetConstraints();

            if(Vector3.Distance(forwardConstraint, backwardConstraint) > Vector3.Distance(leftConstraint, rightConstraint))
            {
                movementDirection = MovementDirection.Vertical;
            }
            else
            {
                movementDirection = MovementDirection.Horizontal;
            }
            StartCoroutine( Move() );
        }
        private void GetConstraints()
        {
            Ray forwardRay = new Ray(transform.position, transform.forward * 100);
            Ray backwardRay = new Ray(transform.position, -transform.forward * 100);
            Ray rightRay = new Ray(transform.position, transform.right * 100);
            Ray leftRay = new Ray(transform.position, -transform.right * 100);

            // visualize rays
            // Debug.DrawRay(transform.position, transform.forward * 10, Color.red, 5);
            // Debug.DrawRay(transform.position, -transform.forward * 10, Color.red, 5);
            // Debug.DrawRay(transform.position, transform.right * 10, Color.red, 5);
            // Debug.DrawRay(transform.position, -transform.right * 10, Color.red, 5);

            // check if ray hits something
            if(Physics.Raycast(forwardRay, out RaycastHit forwardHit))
            {
                forwardConstraint = forwardHit.point;
            }
            if(Physics.Raycast(backwardRay, out RaycastHit backwardHit))
            {
                backwardConstraint = backwardHit.point;
            }
            if(Physics.Raycast(rightRay, out RaycastHit rightHit))
            {
                rightConstraint = rightHit.point;
            }
            if(Physics.Raycast(leftRay, out RaycastHit leftHit))
            {
                leftConstraint = leftHit.point;
            }
        }
        private IEnumerator Move()
        {
            while(true)
            {
                if(movementDirection == MovementDirection.Vertical)
                {
                    transform.LookAt(forwardConstraint);
                    yield return new WaitUntil(() => MoveObstacleVertical(1));

                    transform.LookAt(backwardConstraint);
                    yield return new WaitUntil(() => MoveObstacleVertical(-1));
                }
                else
                {
                    transform.LookAt(rightConstraint);
                    yield return new WaitUntil(() => MoveObstacleHorizontal(1));

                    transform.LookAt(leftConstraint);
                    yield return new WaitUntil(() => MoveObstacleHorizontal(-1));
                }
            }
        }
        private bool MoveObstacleVertical(int direction)
        {
            transform.Rotate(Vector3.right * rotationalSpeed * Time.deltaTime);

            if(direction >= 1)   
            {
                transform.position = Vector3.MoveTowards(transform.position, forwardConstraint, Time.deltaTime * speed);
                return Vector3.Distance(transform.position, forwardConstraint) < stoppingDistance;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, backwardConstraint, Time.deltaTime * speed);
                return Vector3.Distance(transform.position, backwardConstraint) < stoppingDistance;
            }
        }
        private bool MoveObstacleHorizontal(int direction)
        {
            transform.Rotate(Vector3.right * rotationalSpeed * Time.deltaTime);
            if(direction >= 1)   
            {
                transform.position = Vector3.MoveTowards(transform.position, rightConstraint, Time.deltaTime * speed);
                return Vector3.Distance(transform.position, rightConstraint) < stoppingDistance;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, leftConstraint, Time.deltaTime * speed);
                return Vector3.Distance(transform.position, leftConstraint) < stoppingDistance;
            }
        }
        protected override void OnPlayerCollision()
        {
            print("Moving Obstacle");
        }
    }
}
