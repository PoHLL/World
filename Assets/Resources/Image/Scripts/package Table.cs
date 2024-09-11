using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "XiaoLong/PackageTable", fileName = "PackageTable")]
public class packageTable : ScriptableObject
{
    public List<PackageTableitem> DateList = new List<PackageTableitem>();    
}

[System.Serializable]
public class PackageTableitem
{
    public int id;

    public int type;

    public int star;

    public string name;

    public string description;

    public string skillDescription;

    public string imagePath;
}