using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameboardSetup : MonoBehaviour
{
    public GameRoundSetupData[] levels;
    [SerializeField]
    OptionsTray optionsPrefab;

    [SerializeField]
    DropTray dropTrayPrefab;

    OptionsTray optionsTrayInstance;
    DropTray dropTrayInstance;
    List<int> currentRoundChoices;
    
    enum GameState
    {
        CreateNewRound,
        NormalGameplay,
        AnimateItemsBackToOrigin,
        CelebrationTime, 

    }
    GameState gameState = GameState.NormalGameplay;
    float timeGameStateChange;
    //----------------------------------------------------
    void Start()
    {
        gameState = GameState.CreateNewRound;
    }
  /*  int[,] progression = new int[,] 
        { 
            { 3, 2 },
            { 3, 3 },
            { 4, 3 }, 
            { 4, 2 }, 
            { 5, 4 },
            { 5, 3 },
            { 5, 2 }
        };*/
    int progressionIndex = 0;

    void InitializeGame(int numOptions, int numDropTrays)
    {
        Debug.Assert(numOptions >= numDropTrays);
        if(optionsTrayInstance)
        {
            Destroy(optionsTrayInstance.gameObject);
        }
        if(dropTrayInstance)
        {
            Destroy(dropTrayInstance.gameObject);
        }
        GameObject placeholder = Utils.FindChild(this.gameObject, "NonSizingChild");

        Vector3 position = Vector3.zero;
        optionsTrayInstance = Instantiate<OptionsTray>(optionsPrefab, position, optionsPrefab.transform.rotation, placeholder.transform);
        optionsTrayInstance.gameObject.SetActive(true);
        optionsTrayInstance.Init(numOptions);
        optionsTrayInstance.gameboard = this;

        position.z += 0.5f;

        dropTrayInstance = Instantiate<DropTray>(dropTrayPrefab, position, optionsPrefab.transform.rotation, placeholder.transform);
        dropTrayInstance.gameObject.SetActive(true);
        dropTrayInstance.gameboard = this;
        optionsTrayInstance.dropTray = dropTrayInstance;
        dropTrayInstance.Init(numDropTrays);

        HidePrefabs();

        currentRoundChoices = MakeColorChoices();
        dropTrayInstance.FillInChoices(currentRoundChoices);
        gameState = GameState.NormalGameplay;
    }



    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.CreateNewRound:
                GameRoundSetupData level = levels[progressionIndex];
                InitializeGame(level.supply, level.numTargets);
                break;
            case GameState.NormalGameplay:
                if (dropTrayInstance.AreAllDishesFilled() == true)// todo, convert to event driven
                {
                    List<Dish> listOfCorrectDishes = dropTrayInstance.SortMatches();
                    foreach (var dish in listOfCorrectDishes)
                    {
                        Token token = dish.droppedToken;
                        optionsTrayInstance.RemoveToken(token);
                        currentRoundChoices.Remove(token.choiceIndex);
                        dish.gameObject.SetActive(false);
                    }

                    dropTrayInstance.SendActiveTrayTokensHome();
                    bool isComplete = dropTrayInstance.AreAnyTraysStillActive() == false;
                    if (isComplete)
                    {
                        gameState = GameState.AnimateItemsBackToOrigin;
                        timeGameStateChange = Time.time + 2;
                    }
                }
                break;
            case GameState.AnimateItemsBackToOrigin:
                if(timeGameStateChange < Time.time)
                {
                    gameState = GameState.CelebrationTime;
                    timeGameStateChange = Time.time + 2;
                    // todo, start celebration
                }
                break;
            case GameState.CelebrationTime:
                if (timeGameStateChange < Time.time)
                {
                    gameState = GameState.CreateNewRound;
                    timeGameStateChange = 0;
                    progressionIndex++;
                    if (progressionIndex >= levels.Length)
                        progressionIndex = levels.Length -1;
                }
                break;
        }
    }

    public void HidePrefabs()
    {
        optionsPrefab.gameObject.SetActive(false);
        dropTrayPrefab.gameObject.SetActive(false);
    }

    List<int> MakeColorChoices()
    {
        var listOptions = optionsTrayInstance.GetSelectedTokenIndices();
        int numTargets = dropTrayInstance.NumOptions;

        List<int> myChoices = new List<int>();
        for(int i=0; i<numTargets; i++)
        {
            int choice = Random.Range(0, listOptions.Count);
            myChoices.Add(listOptions[choice]);
            listOptions.RemoveAt(choice);
        }
        return myChoices;
    }


}
