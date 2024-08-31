using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MazeGeneratorAndSolverDemo.Obstacles
{
    public enum AffectedTransform
    {
        Position,
        Rotation, 
        Scale
    }
    public class PeriodicObstacle : Obstacle
    {
        [SerializeField] private float interval = 1f;
        [SerializeField] private float speed = 1f;
        [SerializeField] private Transform movingPart;
        [Tooltip("Direction value between 0 and 1, it will be normalized automatically")]
        [SerializeField] private Vector3 movingDirection;
        [SerializeField] private Vector3 delta;
        [SerializeField] private AffectedTransform affectedTransform;

        [Space]
        public UnityEvent OnObstacleActivated;
        [Space]
        public UnityEvent OnObstacleDeactivated;

        private Vector3 initialTransform;
        private void Start()
        {
            movingDirection = movingDirection.normalized;

            switch(affectedTransform)
            {
                case AffectedTransform.Position:
                    initialTransform = movingPart.localPosition;
                    break;
                case AffectedTransform.Rotation:
                    initialTransform = movingPart.rotation.eulerAngles;
                    break;
                case AffectedTransform.Scale:
                    initialTransform = movingPart.localScale;
                    break;
            }

            StartCoroutine( AlternateObstacle() );
        }
        private IEnumerator AlternateObstacle()
        {
            while(true)
            {
                OnObstacleDeactivated?.Invoke();
                
                yield return new WaitForSeconds( interval );

                yield return new WaitUntil(() => PerformTranslation(1));

                OnObstacleActivated?.Invoke();
                
                yield return new WaitForSeconds( interval );

                yield return new WaitUntil(() => PerformTranslation(-1));

            }
        }
        private bool PerformTranslation(int direction)
        {
             Vector3 target = 
                    direction == 1 ? new Vector3(
                        delta.x * movingDirection.x, 
                        delta.y * movingDirection.y,
                        delta.z * movingDirection.z) : initialTransform;

            bool isPerformed = false;

            switch( affectedTransform )
            {
                case AffectedTransform.Position:
                    movingPart.localPosition = Vector3.Lerp(movingPart.localPosition, target, Time.deltaTime * speed);
                    isPerformed = movingPart.localPosition == target;
                    break;

                case AffectedTransform.Rotation:
                    movingPart.rotation = Quaternion.Slerp(movingPart.rotation, Quaternion.Euler(target), Time.deltaTime * speed);
                    isPerformed = movingPart.rotation.eulerAngles == target;
                    break;
                
                case AffectedTransform.Scale:
                    movingPart.localScale = Vector3.Lerp(movingPart.localScale, target, Time.deltaTime * speed);
                    isPerformed = movingPart.localScale == target;
                    break;
            }

            return isPerformed;
        }
        protected override void OnPlayerCollision()
        {
            Debug.Log("Periodic Obstacle");
        }
    }
}

