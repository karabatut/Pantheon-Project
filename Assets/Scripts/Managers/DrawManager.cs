using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{

    Texture2D texture;
    Color color;
    int pixelCount;
    GameObject gameManager;
    GameManager gameManagerScript;
    bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the gamemanager instance
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = (GameManager)gameManager.GetComponent(typeof(GameManager));

        //A Texture2D is applied to the End Wall mesh.
        texture = new Texture2D(128, 128);
        MeshRenderer mesh = GetComponent<MeshRenderer>();

        mesh.material.mainTexture = texture;
        texture.filterMode = FilterMode.Trilinear;
        color = Color.red;
        pixelCount = 0; // For counting the painted pixels for the percentage info.
    }

    // Update is called once per frame
    void Update()
    {
        finished = gameManagerScript.isFinished();
        //If finished start painting.
        if (finished)
        {
            if (Input.GetMouseButton(0))
            {
                var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // The pixel which has been hit by the mouse click and all the pixels surrounding it are painted red.
                if (Physics.Raycast(Ray, out hit))
                {
                    //If the pixel that is about to be painted is already painted, don't paint it.
                    //If a pixel is painted, total pixel count is incremented.
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128), (int)(hit.textureCoord.y * 128)) == Color.red)) 
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128), (int)(hit.textureCoord.y * 128), color);
                        pixelCount++;
                    }
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128 + 1), (int)(hit.textureCoord.y * 128)) == Color.red))
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128 + 1), (int)(hit.textureCoord.y * 128), color);
                        pixelCount++;
                    }
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128 - 1), (int)(hit.textureCoord.y * 128)) == Color.red))
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128 - 1), (int)(hit.textureCoord.y * 128), color);
                        pixelCount++;
                    }
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128), (int)(hit.textureCoord.y * 128 + 1)) == Color.red))
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128), (int)(hit.textureCoord.y * 128 + 1), color);
                        pixelCount++;
                    }
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128), (int)(hit.textureCoord.y * 128 - 1)) == Color.red))
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128), (int)(hit.textureCoord.y * 128 - 1), color);
                        pixelCount++;
                    }
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128 + 1), (int)(hit.textureCoord.y * 128 + 1)) == Color.red))
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128 + 1), (int)(hit.textureCoord.y * 128 + 1), color);
                        pixelCount++;
                    }
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128 + 1), (int)(hit.textureCoord.y * 128 - 1)) == Color.red))
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128 + 1), (int)(hit.textureCoord.y * 128 - 1), color);
                        pixelCount++;
                    }
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128 - 1), (int)(hit.textureCoord.y * 128 + 1)) == Color.red))
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128 - 1), (int)(hit.textureCoord.y * 128 + 1), color);
                        pixelCount++;
                    }
                    if (!(texture.GetPixel((int)(hit.textureCoord.x * 128 - 1), (int)(hit.textureCoord.y * 128 - 1)) == Color.red))
                    {
                        texture.SetPixel((int)(hit.textureCoord.x * 128 - 1), (int)(hit.textureCoord.y * 128 - 1), color);
                        pixelCount++;
                    }
                }
                texture.Apply();
            }
        }
        

    }

    //Texture is 128*128 so total pixel count is divided by 128*128 to get their ratio.
    public float getPercentage()
    {
        int size = 128 * 128;
        float percentage;

        percentage = (float)pixelCount / (float)size;

        return percentage * 100;
    }
}
