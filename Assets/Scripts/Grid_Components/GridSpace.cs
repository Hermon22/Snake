using System;
using Player;
using UnityEngine;

namespace Grid_Components
{
    public class GridSpace : MonoBehaviour
    {
        public int index;
        public int rowIndex;
        public int columnsIndex;
        [HideInInspector] public Vector3 positionOfThePoint;
        public bool freeSpace = true;


        private void OnTriggerEnter2D(Collider2D other)
        {
            var gridObject = other.GetComponent<GridObject>();
            var collectableObject = other.GetComponent<CollectableSize>();
            if (gridObject == null || collectableObject != null) return;
            freeSpace = false;
            gridObject.rowIndex = rowIndex;
            gridObject.columnIndex = columnsIndex;

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var gridObject = other.GetComponent<GridObject>();
            var collectableObject = other.GetComponent<CollectableSize>();
            if (gridObject == null || collectableObject != null) return;
            freeSpace = true;
        }
    }
}
