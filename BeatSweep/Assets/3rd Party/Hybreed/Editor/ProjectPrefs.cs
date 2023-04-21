using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/*
 * Helper module to store editor-specific pefs on a per-project basis
 */
public class ProjectPrefs
{
    private const string _filename = "ProjectPrefs";

    public static ProjectPrefs _instance;
    public static ProjectPrefs Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ProjectPrefs();
                _instance.Load();
            }

            return _instance;
        }
    }

    /*
     * Retrieve a string value
     */
    public static string GetString(string key, string defaultValue)
    {
        return Instance.GetStringInternal(key, defaultValue);
    }

    /*
     * Store a string value
     */
    public static void SetString(string key, string value)
    {
        Instance.SetStringInternal(key, value);
    }

    // Dictionary for storing all values
    private Dictionary<string, string> _database = new Dictionary<string, string>();

    /*
     * Load current values
     */
    void Load()
    {
        // Read database
        if (File.Exists(_filename))
        {
            string[] lines = File.ReadAllLines(_filename);
            for(int i = 0 ; i < lines.Length ; i += 2)
            {
                _database[lines[i]] = lines[i + 1];
            }
        }
    }

    /*
     * Save current values
     */
    void Save()
    {
        // Store database
        // TODO: This could be much more secure
        FileStream fs = File.Open(_filename, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        foreach(var e in _database)
        {
            sw.WriteLine(e.Key);
            sw.WriteLine(e.Value);
        }
        sw.Close();
        fs.Close();
    }

    /*
     * Retrieve a string value
     */
    string GetStringInternal(string key, string defaultValue)
    {
        if (_database.ContainsKey(key))
            return _database[key];

        return defaultValue;
    }

    /*
     * Store a string value
     */
    void SetStringInternal(string key, string value)
    {
        _database[key] = value;
        Save();
    }
}
