using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class GridMovement : MonoBehaviour
    {
        public GridBehaviour behaviour;
        public GameObject obj;
        public bool move = false;

        private List<GameObject> invertedPath = new List<GameObject>();

        private void Update()
        {
            if (move)
            {
                Move(obj.transform);
                move = false;
            }
        }

        public void Move(Transform _controlled)
        {
            invertedPath = behaviour.path;
            StartCoroutine(MoveLoop(_controlled));
        }

        private IEnumerator MoveLoop(Transform _controlled)
        {
            if (invertedPath == null)
                yield return null;

            for (int i = 0; i < invertedPath.Count - 1; i++)
            {
                yield return MoveTo(invertedPath[i].transform.position, invertedPath[i + 1].transform.position, _controlled);

                if (i == invertedPath.Count - 1)
                {
                    // behaviour.startX = 
                }
            }
        }

        private IEnumerator MoveTo(Vector3 _start, Vector3 _end, Transform _controlled)
        {
            float time = 0;
            float rate = 1;

            while (time < rate)
            {
                float factor = Mathf.Clamp01(time / rate);

                _controlled.position = Vector3.Lerp(_start, _end, factor);

                yield return null;

                time += Time.deltaTime;
            }

            _controlled.position = _end;
        }
    }
}