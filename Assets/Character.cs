using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public int age; // Changed to int based on your JSON structure
    public string descShort;
    public string descLong;
    public string quirk;
    public List<string> props;
}
