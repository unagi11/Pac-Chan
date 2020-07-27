using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public static MapManager instance;

    public GameObject [] MapObject;

    public enum MapObjectCategory {
        Empty = 0,
        Wall = 1,
        Coin = 2,
        Power = 3,
        Portal = 4,
        Player = 5,
        Enemy = 6
    };

    /* 0 : null
     * 1 : Wall
     * 2 : Coin
     * 3 : Power
     * 4 : Portal
     * 5 : Player
     * 6 : Enemy
     * 
     * 
     */

    Vector3 StartPosition = new Vector3(0, 0, 0);

    public Vector3 RightUp, RightDown, LeftUp, LeftDown;

    public static int [,] Map1 = new int[,]{
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 0
        {1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1 }, // 1
        {1, 3, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 3, 1 }, // 2
        {1, 2, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 2, 1 }, // 3
        {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 }, // 4
        {1, 2, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 2, 1 }, // 5
        {1, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2, 1 }, // 6
        {1, 1, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 1, 1 }, // 7
        {1, 1, 1, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 1, 1, 1 }, // 8
        {1, 1, 1, 1, 2, 1, 2, 1, 1, 0, 1, 1, 2, 1, 2, 1, 1, 1, 1 }, // 9
        {1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 1 }, // 10
        {1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1 }, // 11
        {1, 1, 1, 1, 2, 1, 2, 2, 2, 5, 2, 2, 2, 1, 2, 1, 1, 1, 1 }, // 12
        {1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1 }, // 13
        {1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1 }, // 14
        {1, 2, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 2, 1 }, // 15
        {1, 3, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 3, 1 }, // 16
        {1, 1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1 }, // 17
        {1, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2, 1 }, // 18
        {1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1 }, // 19
        {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 }, // 20
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 21
    }; //0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18

    public static int[,] currentMap;

    private void Awake()
    {
        instance = this;
        currentMap = Map1;

        LeftDown = new Vector3(0, 0);
        RightDown = new Vector3(currentMap.GetLength(0), 0);
        LeftUp = new Vector3(0, currentMap.GetLength(1));
        RightUp = new Vector3(currentMap.GetLength(0), currentMap.GetLength(1));

        for (int y = 0; y < currentMap.GetLength(0); y++)
        {
            for (int x = 0; x < currentMap.GetLength(1); x++)
            {
                switch (currentMap[y, x])
                {
                    case 0:
                        continue;
                    case 5:
                        if (GameObject.FindGameObjectWithTag("Player") != null)
                            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(x, 0, currentMap.GetLength(0) - y);
                        break;
//                    case 6:
//                        GameObject.FindGameObjectWithTag("Enemy").transform.position = new Vector3(x, 0, currentMap.GetLength(0) - y);
//                        break;
                    default:
                        Instantiate(MapObject[currentMap[y, x]], new Vector3(x, MapObject[currentMap[y, x]].transform.position.y, currentMap.GetLength(0) - y), MapObject[currentMap[y, x]].transform.rotation, transform);
                        break;
                }
            }
        }
    }
}

