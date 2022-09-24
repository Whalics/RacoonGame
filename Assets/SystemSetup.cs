using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SystemSetup : MonoBehaviour
{
    private static string _animationsFolder = "Animations";
    private static string _spritesFolder = "Sprites";
    private static string _materialsFolder = "Materials";
    private static string _meshesFolder = "Meshes";
    private static string _texturesFolder = "Textures";
    private static string _audioFolder = "Audio";
    private static string _scriptableObjectsFolder = "Scriptable Objects";
    private static string _scenesFolder = "Scenes";
    private static string _scriptsFolder = "Scripts";
    private static string _fontsfolder = "Fonts";
    private static string _prefabsFolder = "Prefabs";
    private static string _inputsFolder = "Input Action Assets";

    [MenuItem("Game Setup/Create Folder")]

    static void CreateFolder(){
        
        string animationsPath = $"{Application.dataPath}/{_animationsFolder}";
        if(!Directory.Exists(animationsPath)){
            Directory.CreateDirectory(animationsPath);
        }

        string spritesPath = $"{Application.dataPath}/{_spritesFolder}";
        if(!Directory.Exists(spritesPath)){
            Directory.CreateDirectory(spritesPath);
        }

        string materialsPath = $"{Application.dataPath}/{_materialsFolder}";
        if(!Directory.Exists(materialsPath)){
            Directory.CreateDirectory(materialsPath);
        }
        
        string meshespath = $"{Application.dataPath}/{_meshesFolder}";
        if(!Directory.Exists(_meshesFolder)){
            Directory.CreateDirectory(_meshesFolder);
        }

        string texturesPath = $"{Application.dataPath}/{_texturesFolder}";
        if(!Directory.Exists(texturesPath)){
            Directory.CreateDirectory(texturesPath);
        }

        string audioPath = $"{Application.dataPath}/{_audioFolder}";
        if(!Directory.Exists(audioPath)){
            Directory.CreateDirectory(audioPath);
        }

        string scriptableobjectpath = $"{Application.dataPath}/{_scriptableObjectsFolder}";
        if(!Directory.Exists(scriptableobjectpath)){
            Directory.CreateDirectory(scriptableobjectpath);
        }

        string scenespath = $"{Application.dataPath}/{_scenesFolder}";
        if(!Directory.Exists(scenespath)){
            Directory.CreateDirectory(scenespath);
        }

        string scriptspath = $"{Application.dataPath}/{_scriptsFolder}";
        if(!Directory.Exists(scriptspath)){
            Directory.CreateDirectory(scriptspath);
        }

        string fontspath = $"{Application.dataPath}/{_fontsfolder}";
        if(!Directory.Exists(fontspath)){
            Directory.CreateDirectory(fontspath);
        }

        string prefabsfolder = $"{Application.dataPath}/{_prefabsFolder}";
        if(!Directory.Exists(prefabsfolder)){
            Directory.CreateDirectory(prefabsfolder);
        }

        string inputactionsfolder = $"{Application.dataPath}/{_inputsFolder}";
        if(!Directory.Exists(inputactionsfolder)){
            Directory.CreateDirectory(inputactionsfolder);
        }
    }
    [MenuItem("Game Setup/Create Materials")]
    static void MakeMaterials(){
        MatDictionary colorsDict = new MatDictionary();
        colorsDict.PopulateDictionary();
        Dictionary<string, Color> colors = colorsDict.GetDictionary();

        foreach(KeyValuePair<string, Color> matColor in colors){
            Material mat = new Material(Shader.Find("Standard"));
            AssetDatabase.CreateAsset(mat, $"Assets/Materials/{matColor.Key}.mat");
            mat.color = matColor.Value;
        }
    }
}

public class MatDictionary{

    private Dictionary<string, Color> colors = new Dictionary<string, Color>();

    public void PopulateDictionary(){
        colors.Add("Red_M", new Color(1f,0.2f,0.2f,1f));
        colors.Add("Blue_M", new Color(0.2f,0.2f,1f,1f));
        colors.Add("Green_M", new Color(0.2f,1f,0.2f,1f));
        colors.Add("Yellow_M", new Color(1f,1f,0.2f,1f));
    }
    
    public Dictionary<string, Color> GetDictionary(){
        return colors;
    }
}
