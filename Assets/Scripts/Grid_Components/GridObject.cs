using System.Collections;
using UnityEngine;

namespace Grid_Components
{
    public class GridObject : MonoBehaviour
    {
        private GridController _gridControllerScene;
        public Vector3 lastSavedGrid;

        [HideInInspector]public int rowIndex = 0;
        [HideInInspector] public int columnIndex = 0;
        
        public int initialRow = 0;
        public int initialColumn = 0;
        public bool skipStart = false;

        private void Awake()
        {
            lastSavedGrid = transform.position;
            _gridControllerScene = GameController.Instance.GameGrid;
        }

        private void Start()
        {
            if (skipStart) return;
            SetInitialPosition();
        }

        public void SetInitialPosition()
        {
            var initialGridPosition = _gridControllerScene.GrisSpaces[(initialRow - 1), (initialColumn- 1)].positionOfThePoint;
            rowIndex = initialRow - 1;
            columnIndex = initialColumn - 1;
            SetPosition(initialGridPosition,transform.position);
            lastSavedGrid = initialGridPosition;
        }

        public void SetPosition(Vector3 position, Vector3 originalPosition)
        {
            if (_gridControllerScene == null) return;
            var initialPosition = _gridControllerScene.GetNearestPointOnGrid(position, originalPosition);
            transform.position = initialPosition;
            // _lastSavedGrid = initialPosition;
        }
    }
}
