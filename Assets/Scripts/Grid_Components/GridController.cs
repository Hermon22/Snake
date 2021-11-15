using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid_Components
{
    public class GridController : MonoBehaviour
    {
        public float size = 1f;
        public int columns = 5;
        public int rows = 5;

        public GameObject squareSpace;
        [SerializeField] private List<GameObject> createdScares = new List<GameObject>();
        public GridSpace[,] GrisSpaces = null;

        private void Awake()
        {
            GrisSpaces = new GridSpace[rows, columns];
            DrawGrid();
        }

        public Vector3 GetNearestPointOnGrid(Vector3 position, Vector3 originalPosition)
        {
            if (CheckDistanceInRange(position))
            {
                var objPosition = transform.position;
                position -= objPosition;

                var xCount = Mathf.RoundToInt(position.x / size);
                var yCount = Mathf.RoundToInt(position.y / size);
                var zCount = Mathf.RoundToInt(position.z / size);

                var result = new Vector3(
                    (float)xCount * size,
                    (float)yCount * size,
                    (float)zCount * size);

                result += objPosition;

                return result;
            }
            else
            {
                return originalPosition;
            }

        }
    
        private bool CheckDistanceInRange(Vector3 newPosition)
        {
            var position = transform.position;
            return (!((position.x + (size * columns)) <= newPosition.x)) && (!(newPosition.x < position.x)) && (!((position.y + (size * rows)) <= newPosition.y)) && (!(newPosition.y < position.y));
        }
    

        private void DrawGrid()
        {
            var counter = 0;
            var columnCounter = 0;
            for (float x = 0; x < columns; x += size)
            {
                var rowCounter = 0;
                for (float z = 0; z < rows; z += size)
                {
                    var objTrans = transform;
                    var position = objTrans.position;
                    var point = GetNearestPointOnGrid(new Vector3((position.x + x), (position.y + z), 0f), Vector3.zero);
                    var tmp = Instantiate(squareSpace, point, Quaternion.identity);
                    createdScares.Add(tmp);
                    tmp.transform.parent = gameObject.transform;
                    var gridComponent = tmp.GetComponent<GridSpace>();
                    gridComponent.index = counter;
                    gridComponent.rowIndex = rowCounter;
                    gridComponent.columnsIndex = columnCounter;
                    gridComponent.positionOfThePoint = point;
                    GrisSpaces[rowCounter, columnCounter] = gridComponent;
                    counter++;
                    rowCounter++;
                }
                columnCounter++;
            }
        }
        
        /* private void OnDrawGizmos()
      {
          Gizmos.color = Color.red;
          for (float x = 0; x < colums; x += size)
          {
              for (float z = 0; z < rows; z += size)
              {
                  var point = GetNearestPointOnGrid(new Vector3((transform.position.x + x), (transform.position.y + z), 0f), Vector3.zero);
                  Gizmos.DrawSphere(point, 0.1f);
              }

          }
      }*/
    }
}
