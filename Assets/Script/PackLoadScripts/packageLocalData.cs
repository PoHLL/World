using UnityEngine;
using System.Collections.Generic;

public class packageLocalData
{
    private static packageLocalData _instance;

    public static packageLocalData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new packageLocalData();
            }
            return _instance;
        }
    }

    public List<PackageLocalItem> items;

    public void SavePackage()
    {
        string inventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("packageLocalData", inventoryJson);
        PlayerPrefs.Save();
    }

    public List<PackageLocalItem> LoadPackage()
    {
        if (items != null)
        {
            return items;
        }
        if (PlayerPrefs.HasKey("packageLocalData"))
        {
            string inventoryJson = PlayerPrefs.GetString("packageLocalData");
            packageLocalData PackageLocalData = JsonUtility.FromJson<packageLocalData>(inventoryJson);
            items = PackageLocalData.items;
            return items;
        }
        else
        {
            items = new List<PackageLocalItem>();
            return items;
        }
    }
}


    [System.Serializable]
    public class PackageLocalItem
    {
        public string uid;

        public int id;

        public int num;

        public int level;

        public bool isNew;

        public override string ToString()
        {
            return string.Format("[id]:{0} [num]:{1}", id, num);
        }
    }