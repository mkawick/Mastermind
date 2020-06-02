using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboardSetup : MonoBehaviour
{
    [SerializeField]
    OptionsTray optionsPrefab;

    [SerializeField]
    DropTray dropTrayPrefab;

    OptionsTray optionsTrayInstance;
    DropTray dropTrayInstance;
    List<int> currentRoundChoices;
    void Start()
    {
        GameObject placeholder = Utils.FindChild(this.gameObject, "NonSizingChild");

        Vector3 position = Vector3.zero;
        optionsTrayInstance = Instantiate<OptionsTray>(optionsPrefab, position, optionsPrefab.transform.rotation, placeholder.transform);
        optionsTrayInstance.Init(3);
        optionsTrayInstance.gameboard = this;


        position.z += 0.5f;

        dropTrayInstance = Instantiate<DropTray>(dropTrayPrefab, position, optionsPrefab.transform.rotation, placeholder.transform);
        dropTrayInstance.gameboard = this;
        optionsTrayInstance.dropTray = dropTrayInstance;
        dropTrayInstance.Init(2);
        //drop.tra
        HidePrefabs();

        currentRoundChoices = MakeColorChoices();
        dropTrayInstance.FillInChoices(currentRoundChoices);
    }

    

    // Update is called once per frame
    void Update()
    {
        if(dropTrayInstance.AreAllDishesFilled() == true)// todo, convert to event driven
        {
            // match choices to selected
            //currentRoundChoices;
            List<Dish> listOfCorrectDishes = dropTrayInstance.SortMatches();
            foreach(var dish in listOfCorrectDishes)
            {
                Token token = dish.droppedToken;
                optionsTrayInstance.RemoveToken(token);
                currentRoundChoices.Remove(token.choiceIndex);
                dish.gameObject.SetActive(false);
            }
            // walk list of dishes, grab the token and then remove it from the options tray
            // also, remove it from the current round choices

            dropTrayInstance.SendActiveTrayTokensHome();
            // now we send the non-matching back to their original locations


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
