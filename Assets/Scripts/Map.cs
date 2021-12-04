using System.Net.Mail;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("提前导入：")]
    public SmallMap smallMapPrefab;

    [Header("非提前导入：")]
    public Texture2D mapTile; 
    public Sprite[] SPRITES;
    public SmallMap[,] SMALLMAP;
    public string COLLISIONS;
    public Vector2[,] MAPTRANS; //地图位置信息，通过LoadMapTrans()生成
    private int W, H; //地图长和宽，从COLLISION读取
    

    void Start() 
    {
        if(this.gameObject.name == "Map1Anchor")
        {
            ReadCOLLISIONS("Text/Level"+SceneController.instance.level.ToString()+"Scene1/mapCollisions1");
            LoadMap(1);
            SceneController.instance.map1 = this;
        }
        else
        {
            ReadCOLLISIONS("Text/Level"+SceneController.instance.level.ToString()+"Scene1/mapCollisions2");
            LoadMap(2);
            SceneController.instance.map2 = this;
        }
    }

    public void LoadMap(int number)
    {
        LoadMapSprite(number);
        LoadMapTrans(number);
        ShowMap();
    }

    void ShowMap()
    {
        SMALLMAP = new SmallMap[W, H];
        int tNum = 0;

        //运行整个地图并实例化Tiles
        for(int j=0; j<H; j++)
        {
            for(int i=0; i<W; i++)
            {
                SmallMap sm = Instantiate<SmallMap>(smallMapPrefab);
                sm.transform.SetParent(this.transform);
                sm.SetSmallMap(this, i, j, tNum);
                SMALLMAP[i, j] = sm;

                tNum++;
            }
        }
    }

    // void ReadCOLLISIONS(string path) //读取并存储地图碰撞体数据COLLISIONS和地图W和H
    // {
    //     print(path);
    //     string [] lines = Resources.Load<TextAsset>(path).text.Split('\n');
    //     H = lines.Length;
    //     string [] tileNums = lines[0].Split(' ');
    //     W = tileNums.Length;
    //     print("W = " + W);

    //     for(int j=0; j<H; j++)
    //     {
    //         tileNums = lines[j].Split(' ');
    //         for(int i=0; i<W; i++)
    //         {
    //             COLLISIONS += tileNums[i];
    //         }
    //     }
    //     print(COLLISIONS);
    // }

    void ReadCOLLISIONS(string path) //读取并存储地图碰撞体数据COLLISIONS和地图W和H
    {
        string [] lines = Resources.Load<TextAsset>(path).text.Split('\n'); //如何直接两个Split会有bug，一行最后一个字符串会有两个元素
        H = lines.Length;
        string [] tileNums = lines[0].Split(' ');
        W = tileNums.Length;
        using(StringReader sr = new StringReader(Resources.Load<TextAsset>(path).text))
        {
            string line;
            for(int j=0; j<H; j++)
            {
                line = sr.ReadLine();
                tileNums = line.Split(' ');

                for(int i=0; i<W; i++)
                {
                    COLLISIONS += tileNums[i];
                }
            }
        }
    }

    void LoadMapSprite(int number)
    {
        SPRITES = Resources.LoadAll<Sprite>("Textures/Level"+SceneController.instance.level.ToString()+"Scene1/Map"+number.ToString());
    }

    void LoadMapTrans(int number)
    {
        MAPTRANS = new Vector2[W, H];
        Vector2 leftAndUp = new Vector2((int)-W/2, (int)H/2);
        Vector2 temp = leftAndUp;
        for(int j=0; j<H; j++)
        {
            temp.x = (int)-W/2;
            for(int i=0; i<W; i++)
            {
                MAPTRANS[i, j] = temp;
                temp.x++;
            }
            temp.y--;
        }
    }
}


