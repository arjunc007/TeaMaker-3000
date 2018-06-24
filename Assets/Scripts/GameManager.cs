using System.Collections;
using System.Collections.Generic;
using Academy.HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.SpatialMapping;

public class GameManager : Singleton<GameManager> {

    public GameObject SelectedGameObject { get; private set; }

    private GameObject oldSelectedGameObject = null;

    public string AvatarText = "For the first step, Add Water!";

    //GameObject states
    bool kettleHasWater;
    bool kettleIsOn;
    bool cupHasTeabag;
    bool cupHasSugar;
    bool cupHasMilk;
    bool cupHasWater;

    // Use this for initialization
    void Start () {
        SelectedGameObject = null;
        kettleHasWater = false;
        kettleIsOn = false;
        cupHasTeabag = false;
        cupHasSugar = false;
        cupHasMilk = false;
        cupHasWater = false;
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SelectObject(GameObject obj)
    {
        oldSelectedGameObject = SelectedGameObject;
        SelectedGameObject = obj;

        if (SelectedGameObject != null && oldSelectedGameObject != null)
            MakeMove();
    }

    private void MakeMove()
    {
        if (oldSelectedGameObject.tag == "Water" && SelectedGameObject.tag == "Kettle")
        {
            AddWaterToKettle();
        }
        else if (oldSelectedGameObject.tag == "Kettle" && SelectedGameObject.tag == "Kettle")
        {
            TurnOnKettle();
        }
        else if (oldSelectedGameObject.tag == "Teabag" && SelectedGameObject.tag == "Cup")
        {
            AddTeabag();
        }
        else if (oldSelectedGameObject.tag == "Sugar" && SelectedGameObject.tag == "Cup")
        {
            AddSugar();
        }
        else if (oldSelectedGameObject.tag == "Milk" && SelectedGameObject.tag == "Cup")
        {
            AddMilk();
        }
        else if (oldSelectedGameObject.tag == "Kettle" && SelectedGameObject.tag == "Cup")
        {
            AddHotWater();
        }
        else if (oldSelectedGameObject.tag == "Cup" && SelectedGameObject.tag == "UsedTeabag")
        {
            RemoveTeabag();
        }
        else if (oldSelectedGameObject.tag == "Spoon" && SelectedGameObject.tag == "Cup")
        {
            MixTea();
        }
    }

    public void AddWaterToKettle()
    {
        if (!kettleHasWater && !kettleIsOn)
        {
            kettleHasWater = true;
            //Set UI 1 color to green
            GameObject.Find("1").GetComponent<Image>().color = new Color(0, 255, 0);

            //Animate water jug
            oldSelectedGameObject.SendMessage("StartAnimation");

            AvatarText = "Good! Now Switch On Kettle!";

        }
    }

    public void TurnOnKettle()
    {
        if (kettleHasWater && !kettleIsOn)
        {
            kettleIsOn = true;

            GameObject.Find("2").GetComponent<Image>().color = new Color(0, 255, 0);

            //Animate Kettle on/off
            oldSelectedGameObject.GetComponent<SteamController>().TurnSteamOn();

            AvatarText = "Great Job! Now Add Teabag!";
        }
    }

    public void AddTeabag()
    {
        if (!cupHasTeabag)
        {
            cupHasTeabag = true;

            GameObject.Find("3").GetComponent<Image>().color = new Color(0, 255, 0);

            //Animate teabag into cup
            AvatarText = "Awesome! Let's Add Sugar!";
        }
    }

    public void AddSugar()
    {
        if (!cupHasSugar)
        {
            cupHasSugar = true;
            GameObject.Find("4").GetComponent<Image>().color = new Color(0, 255, 0);
            //Animate sugar

            AvatarText = "You're doing great! Now Add Milk!";
        }
    }

    public void AddMilk()
    {
        if (!cupHasMilk)
        {
            cupHasMilk = true;
            GameObject.Find("5").GetComponent<Image>().color = new Color(0, 255, 0);
            //Animate milk jug
            oldSelectedGameObject.SendMessage("StartAnimation");

            AvatarText = "Nice! Now Add Hot Water!";
        }
    }

    public void AddHotWater()
    {
        if (cupHasSugar && cupHasTeabag && cupHasMilk && kettleHasWater && kettleIsOn)
        {
            cupHasWater = true;
            GameObject.Find("6").GetComponent<Image>().color = new Color(0, 255, 0);
            //Animate kettle to cup
            oldSelectedGameObject.SendMessage("StartAnimation");

            AvatarText = "Good Job! Now Remove Teabag!";
        }
    }

    public void RemoveTeabag()
    {
        if (cupHasTeabag)
        {
            cupHasTeabag = false;
            GameObject.Find("7").GetComponent<Image>().color = new Color(0, 255, 0);
            //Animate used teabag
            AvatarText = "Almost Done! Just Mix Tea!";
        }
    }

    public void MixTea()
    {
        if (cupHasMilk && cupHasSugar && cupHasWater && !cupHasTeabag)
        {
            GameObject.Find("8").GetComponent<Image>().color = new Color(0, 255, 0);
            //Play stirring;
            AvatarText = "Amazing! You Completed the Game!";
        }
    }

    public void OnReset()
    {
        oldSelectedGameObject.GetComponent<SteamController>().TurnSteamOff();
        SelectedGameObject = null;
        oldSelectedGameObject = null;
        kettleHasWater = false;
        kettleIsOn = false;
        cupHasTeabag = false;
        cupHasSugar = false;
        cupHasMilk = false;
        cupHasWater = false;

        GameObject.Find("1").GetComponent<Image>().color = new Color(255, 255, 255);
        GameObject.Find("2").GetComponent<Image>().color = new Color(255, 255, 255);
        GameObject.Find("3").GetComponent<Image>().color = new Color(255, 255, 255);
        GameObject.Find("4").GetComponent<Image>().color = new Color(255, 255, 255);
        GameObject.Find("5").GetComponent<Image>().color = new Color(255, 255, 255);
        GameObject.Find("6").GetComponent<Image>().color = new Color(255, 255, 255);
        GameObject.Find("7").GetComponent<Image>().color = new Color(255, 255, 255);
        GameObject.Find("8").GetComponent<Image>().color = new Color(255, 255, 255);

        AvatarText = "TAP to interact or HOLD to place";

        //Destroy all children of TeaCollection and place again
        foreach (Transform child in GameObject.Find("TeaCollection").transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Collection of floor and table planes that we can use to set horizontal items on.
        List<GameObject> horizontal = new List<GameObject>();

        // Collection of wall planes that we can use to set vertical items on.
        List<GameObject> vertical = new List<GameObject>();

        // 3.a: Get all floor and table planes by calling
        // SurfaceMeshesToPlanes.Instance.GetActivePlanes().
        // Assign the result to the 'horizontal' list.
        horizontal = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Table | PlaneTypes.Floor);

        // 3.a: Get all wall planes by calling
        // SurfaceMeshesToPlanes.Instance.GetActivePlanes().
        // Assign the result to the 'vertical' list.
        vertical = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Wall);

        TeaCollectionManager.Instance.GenerateItemsInWorld(horizontal, vertical);
    }
}
