using System.Collections.Generic;
using Grid_Components;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviourSingleton<GameController>
{
    [SerializeField] private GridController gameGrid;
    [SerializeField] private GameObject collectable;

    public enum Directions {Up,Down,Left,Right};
    public GridController GameGrid => gameGrid;

    internal GameObject CurrentCollectable;
    internal PlayerController CurrentPlayer;
    
    public override void InitializeSingleton()
    {
        SetAsPersistentSingleton();
    }
    
    private void Start()
    {
        SpawnCollectable();
    }

    public void GameDone()
    {
        UIGameController.Instance.TurnWindowOn();
    }

    public void RestartScene()
    {
        if (CurrentCollectable != null)
        {
            Destroy(CurrentCollectable);
        }
        CurrentPlayer.RestartPlayer();
        UIGameController.Instance.TurnWindowOff();
        SpawnCollectable();
    }

    public void SpawnCollectable()
    {
        var tmp = Instantiate(collectable, Vector3.zero, Quaternion.identity);
        CurrentCollectable = tmp;
        var gridComponent = tmp.GetComponent<GridObject>();
        gridComponent.initialColumn = 1;
        gridComponent.initialRow = 1;
        var randRow = Random.Range(0, gameGrid.rows);
        var randColl = Random.Range(0, gameGrid.columns);

        while (!gameGrid.GrisSpaces[randRow, randColl].freeSpace)
        {
            randRow = Random.Range(0, gameGrid.rows);
            randColl = Random.Range(0, gameGrid.columns);
        }
        
        var initialGridPosition = gameGrid.GrisSpaces[randRow,randColl].positionOfThePoint;
        gridComponent.SetPosition(initialGridPosition,transform.position);
    }
}
