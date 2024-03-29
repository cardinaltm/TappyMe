using UnityEngine;
using UnityEngine.UI;

public class Theme : MonoBehaviour
{
    public static Theme Instance;

    enum Themes
    {
        Summer,
        Lava,
        Ocean,
        Desert,
    }

    private Themes currentTheme;

    public Image backgroundImage;

    public GameObject ground1;
    public GameObject ground2;

    private GameObject currentPipePrefab;

    public GameObject pipeBlue;
    public GameObject pipeGreen;
    public GameObject pipeRed;
    public GameObject pipeYellow;

    public Sprite groundGrassSmall;
    public Sprite groundGrassLong;
    public Sprite groundStone;
    public Sprite groundWater;
    public Sprite groundLava;

    public Sprite backgroundSummer;
    public Sprite backgroundDesert;
    public Sprite backgroundLava;
    public Sprite backgroundOcean;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadTheme()
    {
        string savedTheme = PlayerPrefs.GetString("Theme");

        if(!savedTheme.Equals(""))
        {
            currentTheme = (Themes)System.Enum.Parse(typeof(Themes), PlayerPrefs.GetString("Theme"));
        }
        else
        {
            PlayerPrefs.SetString("Theme", Themes.Summer.ToString());
        }
        
        UpdateTheme();
    }

    public GameObject GetPipeThemePrefab()
    {
        return currentPipePrefab;
    }

    public void OnClickSummerButton()
    {
        currentTheme = Themes.Summer;
        PlayerPrefs.SetString("Theme", Themes.Summer.ToString());
        UpdateTheme();
    }

    public void OnClickDesertButton()
    {
        currentTheme = Themes.Desert;
        PlayerPrefs.SetString("Theme", Themes.Desert.ToString());
        UpdateTheme();
    }

    public void OnClickLavaButton()
    {
        currentTheme = Themes.Lava;
        PlayerPrefs.SetString("Theme", Themes.Lava.ToString());
        UpdateTheme();
    }

    public void OnClickOceanButton()
    {
        currentTheme = Themes.Ocean;
        PlayerPrefs.SetString("Theme", Themes.Ocean.ToString());
        UpdateTheme();
    }

    private void UpdateTheme()
    {
        string imageBackgroundPath = PlayerPrefs.GetString("ImageBackgroundPath");

        if (currentTheme == Themes.Ocean)
        {
            ground1.GetComponent<SpriteRenderer>().sprite = groundWater;
            ground2.GetComponent<SpriteRenderer>().sprite = groundWater;
            currentPipePrefab = pipeBlue;
            if (imageBackgroundPath.Equals("")) backgroundImage.sprite = backgroundOcean;
        }
        else if (currentTheme == Themes.Lava)
        {
            ground1.GetComponent<SpriteRenderer>().sprite = groundLava;
            ground2.GetComponent<SpriteRenderer>().sprite = groundLava;
            currentPipePrefab = pipeRed;
            if (imageBackgroundPath.Equals("")) backgroundImage.sprite = backgroundLava;
        }
        else if (currentTheme == Themes.Summer)
        {
            ground1.GetComponent<SpriteRenderer>().sprite = groundGrassSmall;
            ground2.GetComponent<SpriteRenderer>().sprite = groundGrassSmall;
            currentPipePrefab = pipeGreen;
            if (imageBackgroundPath.Equals("")) backgroundImage.sprite = backgroundSummer;
        }
        else if (currentTheme == Themes.Desert)
        {
            ground1.GetComponent<SpriteRenderer>().sprite = groundStone;
            ground2.GetComponent<SpriteRenderer>().sprite = groundStone;
            currentPipePrefab = pipeYellow;
            if (imageBackgroundPath.Equals("")) backgroundImage.sprite = backgroundDesert;
        }
    }
}
