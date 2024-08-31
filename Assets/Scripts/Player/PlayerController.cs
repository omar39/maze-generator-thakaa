using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MazeGeneratorAndSolverDemo.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float stepSpeed = 20f;
        [SerializeField] private LayerMask stoppingWall;
        public UnityAction OnPlayerMoved;
        private void Update()
        {
           // Vector2 movementResult = movementAction.ReadValue<Vector2>();
            if(Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(Move(Vector3.right));
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(Move(Vector3.left));
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(Move(Vector3.forward));
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(Move(Vector3.back));
            }
        }
        private IEnumerator Move(Vector3 direction)
        {
            Vector3 targetPosition = transform.position + direction;

            if(!CanMoveInDirection(direction)) yield break;
            OnPlayerMoved?.Invoke();

            yield return new WaitUntil(() =>{

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * stepSpeed);
                return Vector3.Distance(transform.position, targetPosition) == 0;
            });
        }
        private bool CanMoveInDirection(Vector3 direction)
        {
            Ray ray = new Ray(transform.position, direction);
            return !Physics.Raycast(ray, 1, stoppingWall.value);
        }
    }
}

