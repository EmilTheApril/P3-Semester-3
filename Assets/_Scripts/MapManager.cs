using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] GameObject error, ground, spike;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else Destroy(this);
    }

    void Start()
    {
        
    }

    public void ReadJson()
    {
        string filepath = Application.dataPath + "/data.json";
        try
        {
            string mapdata = File.ReadAllText(filepath);

            // Parse the JSON data into the mapData object
            mapdata = JsonHelper.fixJson(mapdata);
            MapData[] mapData = JsonHelper.FromJson<MapData>(mapdata);

            // Check if mapData is not null before accessing its properties
            foreach(MapData _mapData in mapData)
            {
                if (_mapData != null)
                {
                    SpawnPlatform(_mapData.name, new Vector2(_mapData.pos[0], 1080f - _mapData.pos[1]), new Vector2(_mapData.scale[0], _mapData.scale[1]), new Vector3(0, 0, _mapData.rotation[2]));
                }
                else
                {
                    Debug.LogError("mapData is null!");
                }
            }

            File.Delete(filepath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading JSON file: {e.Message}");
        }
    }

    public void SpawnPlatform(string _object, Vector2 _spawnPoint, Vector2 _size, Vector3 _rotation)
    {
        GameObject _prefab = FindPrefab(_object);
        GameObject _platform = Instantiate(_prefab, _spawnPoint, Quaternion.identity, this.gameObject.transform);
        _platform.transform.localScale += new Vector3(_size.x, _size.y, 0);
        _platform.transform.rotation = Quaternion.Euler(0, 0, _rotation.z);
    }

    public GameObject FindPrefab(string _name)
    {
        switch(_name) 
        {
            case "Ground":
                return ground;
            case "Spike":
                return spike;
            default:
                Debug.Log("Object not found");
                return error;
        }
    }

    public void ClearMap()
    {
        foreach (Transform child in this.transform)
        {
	        GameObject.Destroy(child.gameObject);
        }
    }
}