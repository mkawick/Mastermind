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
        GameObject placeholder = Utils.FindChild(this.gameObject, "NonSizingChild");

        //OptionsTray
        Vector3 position = Vector3.zero;
        OptionsTray optionsTray = Instantiate<OptionsTray>(optionsPrefab, position, optionsPrefab.transform.rotation, placeholder.transform);
        optionsTray.Init(3);
        optionsTray.gameboard = this;
        

        position.z += 0.5f;

        DropTray dropTray = Instantiate<DropTray>(dropTrayPrefab, position, optionsPrefab.transform.rotation, placeholder.transform);
        dropTray.gameboard = this;
        optionsTray.dropTray = dropTray;
        dropTray.Init(2);
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
