using System.IO;
using UnityEngine;

public class ScreenShotMaker : MonoBehaviour
{
    public Camera cam;
    //public string ImageName = "DefaultName";
    public Sprite outPutImage;
    public GameObject toRenderObjects;

    private void Start()
    {
        if (toRenderObjects == null)
        {
            toRenderObjects = GameObject.Find("ToRenderObjects");
        }
        foreach (Transform child in toRenderObjects.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in toRenderObjects.transform)
        {
            
            child.gameObject.SetActive(true);
            Texture2D tex = RTImage(cam);
            string name = child.name.Replace(",", "_").Replace(".","-");
            File.WriteAllBytes(Application.dataPath + "/2D_Icons/" + name + ".png", tex.EncodeToPNG());
            child.gameObject.SetActive(false);
        }
    }
    // Take a "screenshot" of a camera's Render Texture.
    Texture2D RTImage(Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }
}