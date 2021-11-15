using System;
using System.Collections;
using System.Collections.Generic;
using Grid_Components;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector]public GridObject gridObject;
        public GameController.Directions currentDir;
        public float timeUntilNextMovement = 1;
        public float startDelay = 1;
        public GameObject playerBodyPart;
        private bool _stopPlayer = false;
        [SerializeField]private List<GridObject> playerBody = new List<GridObject>();

        private void Start()
        {
            GameController.Instance.CurrentPlayer = this;
            currentDir = GameController.Directions.Down;
            gridObject = GetComponent<GridObject>();
            playerBody.Add(gridObject);
            StartCoroutine(TimedMovement());
        }

       private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) && currentDir != GameController.Directions.Up)
                currentDir = GameController.Directions.Down;
            if (Input.GetKeyDown(KeyCode.W) && currentDir != GameController.Directions.Down)
                currentDir = GameController.Directions.Up;
            if (Input.GetKeyDown(KeyCode.A) && currentDir != GameController.Directions.Right)
                currentDir = GameController.Directions.Left;
            if (Input.GetKeyDown(KeyCode.D) && currentDir != GameController.Directions.Left)
                currentDir = GameController.Directions.Right;
            
        }


        private IEnumerator TimedMovement()
        {
            yield return new WaitForSeconds(startDelay);
            while (!_stopPlayer)
            {
                var transformPosition = transform.position;
                yield return new WaitForSeconds(timeUntilNextMovement);
                switch (currentDir)
                {
                    case GameController.Directions.Up:
                        transformPosition.y += 1;
                        break;
                    case GameController.Directions.Down:
                        transformPosition.y -= 1;
                        break;
                    case GameController.Directions.Left:
                        transformPosition.x -= 1;
                        break;
                    case GameController.Directions.Right:
                        transformPosition.x += 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (((int)transformPosition.x) >= GameController.Instance.GameGrid.columns || ((int)transformPosition.x) < 0)
                {
                    _stopPlayer = true;
                    GameController.Instance.GameDone();
                    break;
                }
                if (((int)transformPosition.y) >= GameController.Instance.GameGrid.rows || ((int)transformPosition.y) < 0)
                {
                    _stopPlayer = true;
                    GameController.Instance.GameDone();
                    break;
                }
                
                if (!GameController.Instance.GameGrid.GrisSpaces[(int)transformPosition.y,(int)transformPosition.x].freeSpace)
                {
                    _stopPlayer = true;
                    GameController.Instance.GameDone();
                    break;
                }

                gridObject.SetPosition(transformPosition, transform.position);
                MoveBody();
                playerBody[0].lastSavedGrid = transformPosition;

            }
        }

        private void MoveBody()
        {
            if (playerBody.Count <= 1) return;
            for (var i = 1; i < playerBody.Count; i++)
            {
                playerBody[i].SetPosition(playerBody[(i-1)].lastSavedGrid, playerBody[i].gameObject.transform.position);
            }
            for (var i = 1; i < playerBody.Count; i++)
            {
                playerBody[i].lastSavedGrid = playerBody[i].gameObject.transform.position;
            }
           
        }

        private void ClearBody()
        {
            for (var j = 1; j < playerBody.Count; j++)
            {
                Destroy(playerBody[j].gameObject);
            }
            playerBody.Clear();
        }

        public void RestartPlayer()
        {
            ClearBody();
            gridObject.SetInitialPosition();
            currentDir = GameController.Directions.Down;
            playerBody.Add(gridObject);
            _stopPlayer = false;
            StartCoroutine(TimedMovement());
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var collectableObject = other.GetComponent<CollectableSize>();
            if (collectableObject == null) return;
            Destroy(other.gameObject);
            GameController.Instance.CurrentCollectable = null;
            
            var tmp = Instantiate(playerBodyPart, Vector3.zero, Quaternion.identity);
            var gridComponent = tmp.GetComponent<GridObject>();
            tmp.transform.parent = gameObject.transform;
            gridComponent.initialColumn = 1;
            gridComponent.initialRow = 1;
            
            var transformPosition = transform.position;
            switch (currentDir)
            {
                case GameController.Directions.Up:
                    transformPosition.y -= 1;
                    break;
                case GameController.Directions.Down:
                    transformPosition.y += 1;
                    break;
                case GameController.Directions.Left:
                    transformPosition.x += 1;
                    break;
                case GameController.Directions.Right:
                    transformPosition.x -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            gridComponent.SetPosition(transformPosition,gridComponent.gameObject.transform.position);
            playerBody.Add(gridComponent);
            GameController.Instance.SpawnCollectable();
            
        }
    }
}
