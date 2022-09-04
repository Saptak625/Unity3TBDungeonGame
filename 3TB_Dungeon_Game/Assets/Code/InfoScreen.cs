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
        {1, new List<string[]> { new string[] {"Firework Spearman", "A fiery melee warrior that pokes you with a spear.", "When watching fireworks, make sure to wear hearing protection and sit far away."}, new string[] { "Firework Launcher", "An evil cannon that launches heat-seeking fireworks at you.", "When watching fireworks, make sure to wear hearing protection and sit far away." }, new string[] { "Firework Mage", "A firework mage capable of summoning gunpowder gremlins.", "When watching fireworks, make sure to wear hearing protection and sit far away." }, new string[] { "Gunpowder Minions", "Gunpowder gremlins that run after you and ignite on contact.", "When watching fireworks, make sure to wear hearing protection and sit far away." } } },
        {2, new List<string[]> { new string[] {"Chainsaw Guy", "A vengeful thug, specializing in the art of chainsaw-weilding.", "When using lawn tools, make sure to wear hearing protection."}, new string[] { "Lawnmower", "A ninja lawnmower that throws its blade at you.", "When using lawn tools, make sure to wear hearing protection." }, new string[] { "Leaf Blower Mage", "A magical leafblower that summons mini-chainsaws.", "When using lawn tools, make sure to wear hearing protection." }, new string[] { "Chainsaw Minions", "Mini-chainsaws that slice and dice.", "When using lawn tools, make sure to wear hearing protection." } } },
        {3, new List<string[]> { new string[] {"Hand Mixer", "A nasty hand mixer that tries to blend you up.", "When using loud kitchen appliances for extended periods of time, make sure to wear hearing protection."}, new string[] { "Range Hood", "A rangehood with attachment issues.", "When using loud kitchen appliances for extended periods of time, make sure to wear hearing protection." }, new string[] { "Blender Wizard", "A blender that emits a harsh whirr while summoning smoothie blenders from the sky.", "When using loud kitchen appliances for extended periods of time, make sure to wear hearing protection." } } },
        {4, new List<string[]> { new string[] {"Earphone Snake", "A viscious pair of earbuds that tries to strangle you.", "When using speakers, either stationary or portable, make sure to keep the volume at a reasonable level."}, new string[] { "Soundwave 3000", "A speaker that emits dangerous sonic waves.", "When using speakers, either stationary or portable, make sure to keep the volume at a reasonable level." }, new string[] { "Headphone Wizard", "A pair of headphones that summons a tensile cable from the sky.", "When using speakers, either stationary or portable, make sure to keep the volume at a reasonable level." } } },
        {5, new List<string[]> { new string[] {"Little Timmy", "A small child who obliviously wanders around with a boombox.", "When around people blasting music at ludicrous volumes, try to keep a bit of distance if possible, and wear hearing protection."}, new string[] { "Mom Van", "A party van with the music at max volume. You don't want to get too close.", "When around people blasting music at ludicrous volumes, try to keep a bit of distance if possible, and wear hearing protection." }, new string[] { "Convertable Mage", "An obnoxious driver who blasts horrible music at you.", "When around people blasting music at ludicrous volumes, try to keep a bit of distance if possible, and wear hearing protection." }, new string[] { "Music Note Minions", "Horrible music that hurts your ears as well as your soul.", "When around people blasting music at ludicrous volumes, try to keep a bit of distance if possible, and wear hearing protection." } } },
        {6, new List<string[]> { new string[] {"Monster Taxi", "An annoyed taxi driver who tries to run you over.", "When around or using loud vehicles, make sure to wear hearing protection and keep your distance if possible."}, new string[] { "Motorcycle", "A fast vehicle that drives around with its deafening engine.", "When around or using loud vehicles, make sure to wear hearing protection and keep your distance if possible." }, new string[] { "Jetski Mage", "A roaring jetski that summons more jetskis to hunt you down.", "When around or using loud vehicles, make sure to wear hearing protection and keep your distance if possible." }, new string[] { "Jetski Minions", "Minions of the jetski that chase you down.", "When around or using loud vehicles, make sure to wear hearing protection and keep your distance if possible." } } },
        {7, new List<string[]> { new string[] {"Jukebox", "A jukebox that blasts off-key music into your ears when it's up close.", "When at parties at other similarly loud events, make sure to wear hearing protection and occasionally leave louder areas."}, new string[] { "Party DJ", "A fed-up DJ who chucks CDs at you.", "When at parties at other similarly loud events, make sure to wear hearing protection and occasionally leave louder areas." }, new string[] { "Speaker Wizard", "A powerful speaker that summons speakers from the sky.", "When at parties at other similarly loud events, make sure to wear hearing protection and occasionally leave louder areas." } } },
        {8, new List<string[]> { new string[] {"Fan Stampede", "A horde of fans ready to trample you to get an autograph.", "When at sporting events, make sure to wear hearing protection and keep a bit of distance from more boisterous fans."}, new string[] { "Intercom", "A crackling intercom that craves to announce the death of your hearing.", "When at sporting events, make sure to wear hearing protection and keep a bit of distance from more boisterous fans." }, new string[] { "Football Mage", "A popular soccer player who sends their fans after you.", "When at sporting events, make sure to wear hearing protection and keep a bit of distance from more boisterous fans." }, new string[] { "Fan Minions", "A swarm of diehard fans determined to assert their team's dominance.", "When at sporting events, make sure to wear hearing protection and keep a bit of distance from more boisterous fans." } } }
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
