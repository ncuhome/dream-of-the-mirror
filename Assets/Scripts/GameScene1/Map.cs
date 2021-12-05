using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("提前导入：")]
    public SmallMap smallMapPrefab; //小贴图预制件脚本
    public Hero hero1Prefab;
    public Hero hero2Prefab;

    [Header("非提前导入：")]
    //这是第一张地图还是第二张地图
    public int mapNum;
    //存储该大贴图的全部小贴图
    public Sprite[] SPRITES;
    //存储所有小贴图上的SmallMap脚本
    public SmallMap[,] SMALLMAP;
    //存储这张地图的碰撞体信息（具体表示见SmallMap脚本的SetCollider方法的switch）
    public string COLLISIONS;
    //地图位置信息，通过LoadMapTrans()生成
    public Vector2[,] MAPTRANS; 
    //地图起始点，从文件导入
    public Vector2 StartPos;
    //地图终点，从文件导入，和StartPos在一个文件夹
    public Vector2 EndPos;
    //地图长和宽，从COLLISION读取
    private int W, H; 
    

    void Start() 
    {
        if(this.gameObject.name == "Map1Anchor")
        {
            ReadCOLLISIONS("Text/Level"+SceneController.instance.level.ToString()+"Scene1/mapCollisions1");
            ReadStartAndEndPos("Text/Level"+SceneController.instance.level.ToString()+"Scene1/StartAndEndPos1");
            mapNum = 1;
            LoadMap();
            SceneController.instance.map1 = this;
        }
        else
        {
            ReadCOLLISIONS("Text/Level"+SceneController.instance.level.ToString()+"Scene1/mapCollisions2");
            ReadStartAndEndPos("Text/Level"+SceneController.instance.level.ToString()+"Scene1/StartAndEndPos2");
            mapNum = 2;
            LoadMap();
            SceneController.instance.map2 = this;
        }
    }

    public void LoadMap()
    {
        LoadMapSprite(mapNum);
        LoadMapTrans(mapNum);
        ShowMap();
        ShowStartAndEnd();
        ChangeMapScale(); //用于测试地图缩放比例
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

    void ShowStartAndEnd()
    {
        Hero tHero;
        if(mapNum == 1)
        {
            tHero = Instantiate<Hero>(hero1Prefab);
            tHero.mapNum = mapNum;
            SceneController.instance.hero1 = tHero;
        }
        else
        {
            tHero = Instantiate<Hero>(hero2Prefab);
            tHero.mapNum = mapNum;
            SceneController.instance.hero2 = tHero;
        }
        tHero.transform.SetParent(this.transform);
        tHero.SetHero((int)StartPos.x, (int)StartPos.y);

        if(mapNum == 1)
            SceneController.instance.mapFinish1 = new Vector2(EndPos.x, EndPos.y);
        else
            SceneController.instance.mapFinish2 = new Vector2(EndPos.x, EndPos.y);
    }

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

    void ReadStartAndEndPos(string path)
    {
        using(StringReader sr = new StringReader(Resources.Load<TextAsset>(path).text))
        {

            string line;
            String[] vec;
            line = sr.ReadLine();
            vec = line.Split(' ');
            StartPos = new Vector2(Convert.ToInt32(vec[0]), Convert.ToInt32(vec[1]));

            line = sr.ReadLine();
            vec = line.Split(' ');
            EndPos = new Vector2(Convert.ToInt32(vec[0]), Convert.ToInt32(vec[1]));
        }
    }

    void LoadMapSprite(int number)
    {
        SPRITES = Resources.LoadAll<Sprite>("Textures/Level"+SceneController.instance.level.ToString()+"Scene1/Map"+number.ToString());
    }

    void LoadMapTrans(int number) //同时修改起始点位置
    {
        MAPTRANS = new Vector2[W, H];
        Vector2 leftAndUp = new Vector2((int)-W/2, (int)H/2);
        StartPos.x = leftAndUp.x+(StartPos.x-1);
        StartPos.y = leftAndUp.y-(StartPos.y-1);
        EndPos.x = leftAndUp.x+(EndPos.x-1);
        EndPos.y = leftAndUp.y-(EndPos.y-1);

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

    void ChangeMapScale()
    {
        this.transform.localScale = SceneController.instance.scaling;
    }
}


