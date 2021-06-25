using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneCange : MonoBehaviour
{
    public CanvasScaler canvas;
    public RectTransform cuberect;
    Image cubeimage;

    public Sprite[] sprites;
    int spnum;

    float limitX;
    float limitY;

    int posx = 1;
    int posy = 1;

    void Start()
    {
        cubeimage = cuberect.GetComponent<Image>();
        spnum = sprites.Length - 1;

        limitX = (canvas.referenceResolution.x - cuberect.rect.width) / 2;
        limitY = (canvas.referenceResolution.y - cuberect.rect.height) / 2;
    }

    void Update()
    {
        cuberect.position += new Vector3(posx, posy, 0);
        if (cuberect.anchoredPosition.x >= limitX || cuberect.anchoredPosition.x <= -limitX ||
            cuberect.anchoredPosition.y >= limitY || cuberect.anchoredPosition.y <= -limitY)
            cubeturn();
    }

    void cubeturn()
    {
        if (cuberect.anchoredPosition.x >= limitX) posx = -1;
        if (cuberect.anchoredPosition.x <= -limitX) posx = 1;
        if (cuberect.anchoredPosition.y >= limitY) posy = -1;
        if (cuberect.anchoredPosition.y <= -limitY) posy = 1;

        if(spnum <= 0) spnum = sprites.Length - 1;
            else spnum--;
        Debug.Log(spnum);

        cubeimage.sprite = sprites[spnum];
    }

    public void SceneCange_main()
    {
        SceneManager.LoadScene("GenerateClassScene");
    }
}
