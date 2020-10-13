using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class MoveTarget : MonoBehaviour
    {
        public LayerMask hitLayers;
        public Grid grid;
        public Node[,] wholeGrid;
        public List<Node> finalPath;
        public Transform testGO;

        private void Start()
        {
            grid = FindObjectOfType<Grid>();
            wholeGrid = grid.grid;
            finalPath = grid.finalPath;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;

                // if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
                // {
                //     this.transform.position = hit.point;
                // }

                MoveObject(testGO);
            }
        }

        public void MoveObject(Transform _object)
        {
            StartCoroutine(MoveLoop(_object));
        }

        private IEnumerator MoveLoop(Transform _object)
        {
            if (finalPath == null)
                yield return null;

            for (int i = 0; i < finalPath.Count - 1; i++)
            {
                yield return Turn(finalPath[i].pos, _object);
                yield return MoveTo(finalPath[i].pos, finalPath[i + 1].pos, _object);

                if (i == finalPath.Count - 1)
                {
                    //? reset algorithm (instead of after this for loop??)
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

        IEnumerator Turn(Vector3 _lookatPos, Transform _object)
        {
            Quaternion _srcRotation = _object.transform.rotation;
            Quaternion _dstRotation = Quaternion.LookRotation(_lookatPos);
            float _turnPct = 0.1f;

            while (_turnPct <= 1.0f)
            {
                Quaternion _newRotation = Quaternion.Lerp(_srcRotation, _dstRotation, _turnPct);
                _object.transform.rotation = _newRotation;
                _turnPct += .01f;
                yield return null;
            }
        }
    }
}