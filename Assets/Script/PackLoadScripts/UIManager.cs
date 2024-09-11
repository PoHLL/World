using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class UIManager
{
    private static UIManager _intance;
    private Transform _uiRoot;
    //·�������ֵ�
    private Dictionary<string, string> pathDict;
    //Ԥ�Ƽ������ֵ�
    private Dictionary<string, GameObject> prefabDict;
    //�Ѵ򿪽���Ļ����ֵ�
    public Dictionary<string , BasePanel> panelDict;

    public static UIManager Instance
    {
        get
        {
            if(_intance == null)
            {
                _intance = new UIManager();
            }
            return _intance;
        }
    }

    public Transform UIRoot
    {
        get
        {
            if(_uiRoot == null)
            {
                if (GameObject.Find("Canvas"))
                {
                    _uiRoot = GameObject.Find("Canvas").transform;
                }
                else
                {
                    _uiRoot = new GameObject("Canvas").transform;
                }
            };
            return _uiRoot;
        }
    }

    private UIManager()
    {
        InitDicts();
    }

    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();

        pathDict = new Dictionary<string, string>()
        {
            {UIConst.PackagePanel, "Package/PackagePanel"},
            {UIConst.LotteryPanel, "Lottery/LotteryPanel"},
            {UIConst.MainPanel, "MainPanel"},
        };
    }

    public BasePanel GetPanel(string name)
    {
        BasePanel panel = null;
        //����Ƿ��Ѵ�
        if(panelDict.TryGetValue(name, out panel))
        {
            return panel;
        }
        return null;
    }

    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null ;
        //����Ƿ��Ѵ�
        if(panelDict.TryGetValue(name,out panel))
        {
            Debug.Log("�����Ѵ򿪣�" + name);
            return null;
        }

        //���·���Ƿ�����
        string path = "";
        if(!pathDict.TryGetValue(name , out path))
        {
            Debug.Log("���������󣬻�δ����·����" + name);
            return null;  
        }

        // ʹ�û���Ԥ�Ƽ�
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefab/Panel/" + path;

            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }

        // �򿪽���
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("����δ��: " + name);
            return false;
        }

        panel.ClosePanel();
        // panelDict.Remove(name);
        return true;
    }

}

public class UIConst
{
    // menu panels
    public const string PackagePanel = "PackagePanel";
    public const string LotteryPanel = "LotteryPanel";
    public const string MainPanel = "MainPanel";
}

