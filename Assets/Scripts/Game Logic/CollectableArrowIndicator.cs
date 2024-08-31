using UnityEngine;
using MazeGeneratorAndSolverDemo.Collectables;
using System.Collections;

namespace MazeGeneratorAndSolverDemo
{
    public class CollectableArrowIndicator : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        public void SetArrowPointsAt(Collectable collectable)
        {
            StopAllCoroutines();
            transform.position = collectable.transform.position + Vector3.up * 2f;
            // transform.LookAt(collectable.transform.position + collectable.transform.forward);

             StartCoroutine(Indicator());
        }
        private IEnumerator Indicator()
        {
            while(true)
            {
                Vector3 target = transform.position + Vector3.up;
                yield return new WaitUntil(() => MoveTo(target));

                target = transform.position + Vector3.down;
                yield return new WaitUntil(() => MoveTo(target));
            }
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        private bool MoveTo(Vector3 target)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);

            return Vector3.Distance(transform.position, target) <= .1f;
        }
    }
}