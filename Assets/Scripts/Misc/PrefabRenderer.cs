using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PrefabRenderer : MonoBehaviour {
    /// <summary>
    /// Dictionary containing the prefab name as key, and rendered texture as value
    /// </summary>
    public static Dictionary<string, RenderTexture> RenderedTextures;

    /// <summary>
    /// The index of the prefab we are currently working with
    /// </summary>
    private int prefabIndex = 0;

    /// <summary>
    /// The width of the rendertextures
    /// </summary>
    public int TextureWidth = 0;

    /// <summary>
    /// The height of the rendertextures
    /// </summary>
    public int TextureHeight = 0;

    /// <summary>
    /// The depth of the rendertextures
    /// </summary>
    public int TextureDepth = 0;

    /// <summary>
    /// The position the prefabs should be placed
    /// </summary>
    public Vector3 TargetPosition;

    /// <summary>
    /// The rotation of instantiated objects
    /// </summary>
    public Vector3 TargetRotation;

    /// <summary>
    /// The directories to find prefabs in
    /// Relative to Assets/Resources
    /// </summary>
    public string[] Directories;

    /// <summary>
    /// The prefabs we found on load
    /// </summary>
    private List<GameObject> prefabs;

    /// <summary>
    /// Used to determine if we should instantiate an object or store the texture this round
    /// </summary>
    private bool IsInstantiateRound = true;

    /// <summary>
    /// The instantiated gameobject we're working with
    /// </summary>
    private GameObject currObject = null;

    /// <summary>
    /// The rendertexture we're working with
    /// </summary>
    private RenderTexture currTexture = null;

	/// <summary>
	/// Used for initialization
	/// </summary>
	public void Start () {
        PrefabRenderer.RenderedTextures = new Dictionary<string, RenderTexture>();
	}
	
	/// <summary>
	/// Update every frame
	/// </summary>
	public void Update () {
        if (prefabs == null)
        {
            prefabs = new List<GameObject>();
            InitializePrefabs();
            return;
        }
        if (prefabs.Count > 0 && prefabIndex < prefabs.Count)
        {
            CreatePrefabTextures();
        }
        else
        {
            gameObject.SetActive(false);
        }
	}

    /// <summary>
    /// Creates a RenderTexture for each prefab
    /// The currently rendered prefab is prefabs[prefabIndex]
    /// If IsInstantiateRound is true, we instantiate a prefab, otherwise we store the rendertexture and clean up the object
    /// </summary>
    private void CreatePrefabTextures()
    {
        //We're in an instantiateround, instantiate the object
        if (IsInstantiateRound)
        {
            
            currObject = (GameObject)GameObject.Instantiate(prefabs[prefabIndex], TargetPosition, Quaternion.Euler(TargetRotation));
            currTexture = new RenderTexture(TextureWidth, TextureHeight, TextureDepth, RenderTextureFormat.Default);
            gameObject.camera.targetTexture = currTexture;
            IsInstantiateRound = false;
        }
        //The object was instianted and an update round was passed, we can now store the rendertexture
        else if (currObject != null)
        {
            PrefabRenderer.RenderedTextures.Add(prefabs[prefabIndex].name, currTexture);
            gameObject.camera.targetTexture = null;
            Destroy(currObject);
            IsInstantiateRound = true;
            prefabIndex++;
        }
    }

    /// <summary>
    /// Pulls all prefabs in specified folders, relative to Assets/Resources
    /// </summary>
    private void InitializePrefabs()
    {
        foreach (string dirName in Directories)
        {
            if (!Directory.Exists("Assets/Resources/" + dirName))
            {
                continue;
            }
            DirectoryInfo dir = new DirectoryInfo("Assets/Resources/" + dirName);
            FileInfo[] info = dir.GetFiles("*.prefab");
            foreach (FileInfo f in info)
            {
                Debug.Log(f.Name.Replace(".prefab", ""));
                GameObject currPrefab = Resources.Load<GameObject>(f.Name.Replace(".prefab", ""));
                if (currPrefab != null)
                {
                    prefabs.Add(currPrefab);
                }

            }
        }
    }
}
