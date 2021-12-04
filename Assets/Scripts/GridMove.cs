// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GridMove : MonoBehaviour
// {
//     private Hero hero;

//     void Awake() 
//     {
//         hero = GetComponent<Hero>();
//     }

//     void FixedUpdate() {
//         if(hero.dirHeld == -1) return;
//         int facing = hero.GetFacing();

//         //如果在一个方向移动，分配到网格
//         //首先，获取网格位置
//         Vector2 rPos = hero.roomPos;
//         Vector2 rPosGrid = hero.GetRoomPosOnGrid();
        
//         //移动到网格行
//         float delta = 0;
//         if(facing == 0 || facing == 2)
//         {
//             //水平移动，分配到y网格
//             delta = rPosGrid.y - rPos.y;
//         }
//         else
//         {
//             //垂直移动，分配到x网格
//             delta = rPosGrid.x - rPos.x;
//         }
//         if(delta == 0) return;

//         float move = hero.speed * Time.fixedDeltaTime;
//         move = Mathf.Min(move, Mathf.Abs(delta));
//         if(delta < 0) move = -move;

//         if(facing == 0 || facing == 2)
//         {
//             rPos.y += move;
//         }
//         else
//         {
//             rPos.x += move;
//         }

//         mover.roomPos = rPos;
//     }
// }
