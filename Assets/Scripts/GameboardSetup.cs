using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboardSetup : MonoBehaviour
{
    [SerializeField]
    OptionsTray optionsPrefab;

    [SerializeField]
    DropTray dropTrayPrefab;
    // Start is called before the first frame update
    void Start()
    {

        //OptionsTray
        Vector3 position = Vector3.zero;
        OptionsTray tray = Instantiate<OptionsTray>(optionsPrefab, position, optionsPrefab.transform.rotation, this.transform);
        tray.Init(3);
        tray.gameboard = this;
        

        position.z += 3;

        DropTray dropTray = Instantiate<DropTray>(dropTrayPrefab, position, optionsPrefab.transform.rotation, this.transform);
        dropTray.gameboard = this;
        tray.dropTray = dropTray;
        //drop.tra
        HidePrefabs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HidePrefabs()
    {
        optionsPrefab.gameObject.SetActive(false);
        dropTrayPrefab.gameObject.SetActive(false);
    }
}
