using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("地图相关文件")]
    public TextAsset mapCollision;
    public TextAsset mapStartAndEndPoint;

    [Header("相关文件路径")]
    public string mapCollisionPath;
    public string mapStartAndEndPointPath;
    public string mapSpritesPath;

    [Header("地图和人物预制件脚本")]
    public MapTile mapTile;
    public LeftHero leftHero;
    public RightHero rightHero;

    [Header("人物相当于Anchor的出生点与起始点位置")]
    public Vector2 startLocalPoint;
    public Vector2 endLocalPoint;

    [Header("地图碰撞信息")]
    //将地图碰撞二维信息压缩成一维
    public string mapCollisionLine;

    [Header("地图块信息")]
    //存储所有地图块上的SmallMap脚本
    public MapTile[,] mapTiles;

    [Header("其它地图信息")]
    public int width;
    public int height;

    //地图左上角基准点
    private Vector2 leftAndUpLocalPoint;

    void Start()
    {
        if (this.gameObject.name == "LeftMap")
        {
            mapCollisionPath = "Documents/Level" + SceneController.instance.level.ToString() + "/MapCollisions1";
            mapStartAndEndPointPath = "Documents/Level" + SceneController.instance.level.ToString() + "/StartAndEndPos1";
            mapSpritesPath = "Textures/PuzzleGame/Level" + SceneController.instance.level.ToString() + "/Map1";
            if (mapCollision == null)
            {
                mapCollision = Resources.Load<TextAsset>(mapCollisionPath);
            }
            if (mapStartAndEndPoint == null)
            {
                mapStartAndEndPoint = Resources.Load<TextAsset>(mapStartAndEndPointPath);
            }
        }
        if (this.gameObject.name == "RightMap")
        {
            mapCollisionPath = "Documents/Level" + SceneController.instance.level.ToString() + "/MapCollisions2";
            mapStartAndEndPointPath = "Documents/Level" + SceneController.instance.level.ToString() + "/StartAndEndPos2";
            mapSpritesPath = "Textures/PuzzleGame/Level" + SceneController.instance.level.ToString() + "/Map2";
            if (mapCollision == null)
            {
                mapCollision = Resources.Load<TextAsset>(mapCollisionPath);
            }
            if (mapStartAndEndPoint == null)
            {
                mapStartAndEndPoint = Resources.Load<TextAsset>(mapStartAndEndPointPath);
            }
        }
        ReadMapCollision();
        ReadMapStartAndEndPoint();
        LoadMap();
        LoadHero();
    }

    private void ReadMapCollision()
    {
        string mapCollsionText = mapCollision.text;
        string[] lines = mapCollsionText.Split('\n');
        string[] tileNums = lines[0].Split(' ');

        height = lines.Length;
        width = tileNums.Length;

        using (StringReader sr = new StringReader(mapCollsionText))
        {
            string line;
            for (int j = 0; j < height; j++)
            {
                line = sr.ReadLine();
                for (int i = 0; i < width; i++)
                {
                    tileNums = line.Split(' ');
                    mapCollisionLine += tileNums[i];
                }
            }
        }
    }

    private void ReadMapStartAndEndPoint()
    {
        using (StringReader sr = new StringReader(mapStartAndEndPoint.text))
        {
            string line;
            string [] param;
            line = sr.ReadLine();
            param = line.Split(' ');

            startLocalPoint = new Vector2(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));

            line = sr.ReadLine();
            param = line.Split(' ');
            endLocalPoint = new Vector2(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
        }
    }

    private void LoadMap()
    {
        //地图贴图信息
        Sprite [] mapSprites = Resources.LoadAll<Sprite>(mapSpritesPath);
        //地图位置信息
        Vector2 [,] mapLocalPostions = new Vector2[width, height];
        //地图左上角LocalPosition
        leftAndUpLocalPoint = new Vector2((int)-width / 2, (int)height / 2);
        mapTiles = new MapTile[width, height];

        //填充人物起始点和地图终点
        startLocalPoint.x = leftAndUpLocalPoint.x + (startLocalPoint.x - 1);
        startLocalPoint.y = leftAndUpLocalPoint.y - (startLocalPoint.y - 1);
        endLocalPoint.x = leftAndUpLocalPoint.x + (endLocalPoint.x - 1);
        endLocalPoint.y = leftAndUpLocalPoint.y - (endLocalPoint.y - 1);

        //填充贴图相对于Anchor的位置的同时生成地图的同时存储小地图脚本信息
        Vector2 temp = leftAndUpLocalPoint; 
        MapTile tMapTile;
        for (int j = 0; j < height; j++)
        {
            temp.x = leftAndUpLocalPoint.x;
            for (int i = 0; i < width; i++)
            {
                //填充位置
                mapLocalPostions[i, j] = temp;
                //生成地图
                tMapTile = Instantiate<MapTile>(mapTile);
                tMapTile.transform.SetParent(this.transform);
                //让子地图脚本完善自身信息与位置（信息包括x, y, 贴图, 碰撞体字符）
                tMapTile.SetMapTile((int)temp.x, (int)temp.y, ref mapSprites[j * width + i], mapCollisionLine[j * width + i]);
                //存储小地图脚本信息
                mapTiles[i, j] = tMapTile;

                temp.x++;
            }
            temp.y--;
        }
    }

    private void LoadHero()
    {
        if (this.gameObject.name == "LeftMap")
        {
            LeftHero tHero = Instantiate<LeftHero>(leftHero);
            tHero.transform.SetParent(this.transform);
            tHero.SetHero(startLocalPoint, endLocalPoint, this);
        }
        if (this.gameObject.name == "RightMap")
        {
            RightHero tHero = Instantiate<RightHero>(rightHero);
            tHero.transform.SetParent(this.transform);
            tHero.SetHero(startLocalPoint, endLocalPoint, this);
        }  
    }

    public bool WillAgainstTheWall(Direction direction, Vector2 heroLocalPositon)
    {
        int index = 0;
        switch (direction)
        {
            case Direction.Up:
                index = Convert.ToInt32((heroLocalPositon.x - leftAndUpLocalPoint.x) + (leftAndUpLocalPoint.y - heroLocalPositon.y) * width - width);
                break;
            case Direction.Down:
                index = Convert.ToInt32((heroLocalPositon.x - leftAndUpLocalPoint.x) + (leftAndUpLocalPoint.y - heroLocalPositon.y) * width + width);
                break;
            case Direction.Left:
                index = Convert.ToInt32((heroLocalPositon.x - leftAndUpLocalPoint.x) + (leftAndUpLocalPoint.y - heroLocalPositon.y) * width - 1);
                break;
            case Direction.Right:
                index = Convert.ToInt32((heroLocalPositon.x - leftAndUpLocalPoint.x) + (leftAndUpLocalPoint.y - heroLocalPositon.y) * width + 1);
                break;
        }
        
        if (mapCollisionLine[index] == 'H')
        {
            return true;
        }
        if (mapCollisionLine[index] == '_')
        {
            return false;
        }
        return false;
    }
}
