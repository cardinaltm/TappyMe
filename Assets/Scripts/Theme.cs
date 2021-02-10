using UnityEngine;

public class Theme : MonoBehaviour
{
    public GameObject pipePrefab;
    public GameObject groundPrefab;

    public GameObject ground1;
    public GameObject ground2;


    public Sprite groundLava;

    void Start()
    {
    }

    public void OnClickBtn()
    {
        ground1.GetComponent<SpriteRenderer>().sprite = groundLava;
        ground2.GetComponent<SpriteRenderer>().sprite = groundLava;
    }

    public void OnUpdateCurrentTheme()
    {

    }
}
