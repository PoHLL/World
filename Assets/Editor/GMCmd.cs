using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static packageLocalData;
using System;
using log4net.Core;

public class GMCmd
{
    [MenuItem("CMCmd/读取表格")]

    public static void ReadTable()
    {
        packageTable PackageTable = Resources.Load<packageTable>("TableData/PackageTable");
        foreach (PackageTableitem PackageItem in PackageTable.DateList)
        {
            Debug.Log(string.Format("【id】:{0} , 【name】:{1}", PackageItem.id, PackageItem.name));
        }
    }

    [MenuItem("CMCmd/创建背包测试数据")]

    public static void CreateLocaPackageData()
    {
        //保存数据
        packageLocalData.Instance.items = new List<PackageLocalItem>();
        for(int i = 1; i < 9; i++)
        {
            PackageLocalItem packageLocalItem = new()
            {
                uid = Guid.NewGuid().ToString(),
                id = i,
                num = i,
                level = i,
                isNew = i % 2 == 1
            };
            packageLocalData.Instance.items.Add(packageLocalItem);
        }
        packageLocalData.Instance.SavePackage();
    }

    [MenuItem("CMCmd/读取背包测试数据")]
    public static void ReadLocaPackageData()
    {
        //读取数据
        List<PackageLocalItem> readIrems = packageLocalData.Instance.LoadPackage();
        foreach (PackageLocalItem item in readIrems)
        {
            Debug.Log(item);
        }
    }

    [MenuItem("CMCmd/打开背包主界面")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);
    }
}
