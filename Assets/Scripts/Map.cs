using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("提前导入：")]
    public Texture2D mapTile; 
    public static Sprite[] SPRITES;
    public static SmallMap[,] SMALLMAP;
    public SmallMap smallMapPrefab;

    [Header("非提前导入：")]
    public static string COLLISIONS;
    static private int W, H; //地图长和宽，从COLLISION读取
    static private Vector2[,] MAP; //地图位置信息，通过LoadMapTrans()生成

    void Start() 
    {
        if(SceneController.instance.map1 != null)
        {
            ReadCOLLISIONS("Text/Level"+SceneController.instance.level.ToString()+"Scene1/mapCollisions1");
            LoadMap(1);
        }
        else
        {
            ReadCOLLISIONS("Text/Level"+SceneController.instance.level.ToString()+"Scene1/mapCollisions2");
            LoadMap(2);
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
                sm.SetSmallMap(i, j, tNum);
                SMALLMAP[i, j] = sm;

                tNum++;
            }
        }
    }

    void ReadCOLLISIONS(string path) //读取并存储地图碰撞体数据COLLISIONS和地图W和H
    {
        string [] lines = Resources.Load<TextAsset>(path).text.Split('\n');
        H = lines.Length;
        string [] tileNums = lines[0].Split(' ');
        W = tileNums.Length;

        for(int j=0; j<H; j++)
        {
            tileNums = lines[j].Split(' ');
            for(int i=0; i<W; i++)
            {
                COLLISIONS += tileNums[i];
            }
        }
    }

    void LoadMapSprite(int number)
    {
        SPRITES = Resources.LoadAll<Sprite>("Textures/Level"+SceneController.instance.level.ToString()+"Map"+number.ToString());
    }

    void LoadMapTrans(int number)
    {
        MAP = new Vector2[W, H];
        Vector2 leftAndDown = new Vector2(-W/2, -H/2);
        Vector2 temp = leftAndDown;
        for(int j=0; j<H; j++)
        {
            for(int i=0; i<W; i++)
            {
                MAP[i, j] = temp;
                temp.x++;
            }
            temp.y++;
        }
    }
}


