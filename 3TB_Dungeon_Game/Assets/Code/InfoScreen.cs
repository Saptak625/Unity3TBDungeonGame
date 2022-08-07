using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{
    public int currentIndex = 0;
    public int currentSection = 1;
    public int currentPage = 0;
    public bool wrapAction = false;
    public bool tabs = true;
    public List<int> categories = new List<int>();
    public Room currentRoom = null;
    public GameObject player;
    public GameObject roomLoaderSpawner;

    public Sprite[] sprites;
    public Dictionary<int, List<Sprite>> pageImages = new Dictionary<int, List<Sprite>>();
    public Dictionary<int, List<string[]>> pageInfo = new Dictionary<int, List<string[]>>()
    {
        {1, new List<string[]> { new string[] {"Firework Spearman", null, null}, new string[] { "Firework Launcher", null, null }, new string[] { "Firework Mage", null, null }, new string[] { "Gunpowder Minions", null, null } } },
        {2, new List<string[]> { new string[] {"Chainsaw Guy", null, null}, new string[] { "Lawnmower", null, null }, new string[] { "Leaf Blower Mage", null, null }, new string[] { "Chainsaw Minions", null, null } } },
        {3, new List<string[]> { new string[] {"Hand Mixer", null, null}, new string[] { "Range Hood", null, null }, new string[] { "Blender Wizard", null, null } } },
        {4, new List<string[]> { new string[] {"Earphone Snake", null, null}, new string[] { "Soundwave 3000", null, null }, new string[] { "Headphone Wizard", null, null } } },
        {5, new List<string[]> { new string[] {"Little Timmy", null, null}, new string[] { "Mom Van", null, null }, new string[] { "Convertable Mage", null, null }, new string[] { "Music Note Minions", null, null } } },
        {6, new List<string[]> { new string[] {"Monster Taxi", null, null}, new string[] { "Motorcycle", null, null }, new string[] { "Jetski Mage", null, null }, new string[] { "Jetski Minions", null, null } } },
        {7, new List<string[]> { new string[] {"Jukebox", null, null}, new string[] { "Party DJ", null, null }, new string[] { "Speaker Wizard", null, null } } },
        {8, new List<string[]> { new string[] {"Fan Stampede", null, null}, new string[] { "Intercom", null, null }, new string[] { "Football Mage", null, null }, new string[] { "Fan Minions", null, null } } }
    };

    // Start is called before the first frame update
    void Start()
    {
        int index = 1;

        List<Sprite> newArr = new List<Sprite>();
        for(int i=0; i<sprites.Length; i++)
        {
            newArr.Add(sprites[i]);
            if (newArr.Count == pageInfo[index].Count)
            {
                pageImages.Add(index, newArr);
                newArr = new List<Sprite>();
                index++;
            }
        }

        currentSection = categories[currentIndex];

        if (tabs)
        {
            gameObject.transform.GetChild((currentSection * 2) + 7).position -= new Vector3(0f, 15f, 0f);
            gameObject.transform.GetChild((currentSection * 2) + 7).gameObject.GetComponent<Image>().color = new Color(100.0f / 255.0f, 100.0f / 255.0f, 100.0f / 255.0f, 150.0f / 255.0f);
            gameObject.transform.GetChild((currentSection * 2) + 8).position -= new Vector3(0f, 15f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Sprite s = pageImages[currentSection][currentPage];
        string data1 = pageInfo[currentSection][currentPage][0];
        string data2 = pageInfo[currentSection][currentPage][1];
        string data3 = pageInfo[currentSection][currentPage][2];

        gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = s;
        gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = (data1 == null ? "Enemy Name": data1);
        gameObject.transform.GetChild(4).gameObject.GetComponent<Text>().text = (data2 == null ? "Enemy Description Here" : data2);
        gameObject.transform.GetChild(5).gameObject.GetComponent<Text>().text = (data3 == null ? "How to Protect Yourself in Real Life" : data3);
    }

    public void Right()
    {
        int originalSection = currentSection;
        currentPage += 1;
        if(currentPage >= pageInfo[currentSection].Count)
        {
            currentIndex += 1;
            if(currentIndex >= categories.Count)
            {
                if (!wrapAction)
                {
                    currentIndex = 0;
                    currentSection = categories[currentIndex];
                    currentPage = 0;
                }
                else
                {
                    currentIndex -= 1;
                    currentPage -= 1;
                    startDungeon();
                }
            }
            else
            {
                currentSection = categories[currentIndex];
                currentPage = 0;
            }
        }
        changeTab(originalSection, currentSection);
    }

    public void Left()
    {
        int originalSection = currentSection;
        currentPage -= 1;
        if (currentPage < 0)
        {
            currentIndex -= 1;
            if (currentIndex < 0)
            {
                if (!wrapAction)
                {
                    currentIndex = categories.Count - 1;
                    currentSection = categories[currentIndex];
                    currentPage = pageInfo[currentSection].Count - 1;
                }
                else
                {
                    currentSection = 0;
                    currentPage = 0;
                    startDungeon();
                }
            }
            else
            {
                currentSection = categories[currentIndex];
                currentPage = pageInfo[currentSection].Count - 1;
            }
        }
        changeTab(originalSection, currentSection);
    }

    void changeTab(int originalSection, int newSection)
    {
        if(originalSection != newSection && tabs)
        {
            gameObject.transform.GetChild((originalSection * 2) + 7).position += new Vector3(0f, 15f, 0f);
            gameObject.transform.GetChild((originalSection * 2) + 7).gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 150.0f / 255.0f);
            gameObject.transform.GetChild((originalSection * 2) + 8).position += new Vector3(0f, 15f, 0f);

            gameObject.transform.GetChild((newSection * 2) + 7).position -= new Vector3(0f, 15f, 0f);
            gameObject.transform.GetChild((newSection * 2) + 7).gameObject.GetComponent<Image>().color = new Color(100.0f / 255.0f, 100.0f / 255.0f, 100.0f / 255.0f, 150.0f / 255.0f);
            gameObject.transform.GetChild((newSection * 2) + 8).position -= new Vector3(0f, 15f, 0f);
        }
    }

    public void changeSection(int newSection)
    {
        changeTab(currentSection, newSection);
        currentSection = newSection;
        currentPage = 0;
    }

    void startDungeon()
    {
        player.GetComponent<PlayerController>().isPaused = false;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        roomLoaderSpawner.GetComponent<RoomLoaderSpawner>().startDungeon(currentRoom);
    }

    public void setupScreen(Room r, GameObject p, GameObject rls, List<int> c)
    {
        currentRoom = r;
        player = p;
        roomLoaderSpawner = rls;
        categories = c;
    }
}
