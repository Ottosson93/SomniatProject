using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CNode
{
    private int corridorWidth;
    public Vector2 bottomLeft, topRight;
    private RNode room1, room2;
    public bool vertical;

    public int id = 0;

    public float width, height; 

    public CNode(Vector2 bottomLeft, Vector2 topRight, int width, int id)
    {
        corridorWidth = width;
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        this.id = id;
     }

    public void updateWH()
    {

        if (vertical == true)
        {
            width = 5;

            if(bottomLeft.y < 0)
            {
                
                if (topRight.y < 0)  // -20,-10 // 20 - 10 = 10
                {
                    height = bottomLeft.y * -1 - topRight.y * -1; 
                }
                else // -20,40 //20 + 40 = 60
                {
                    height = bottomLeft.y * -1 + topRight.y; 
                }
            }
            else  // 40,80 // 80 - 40 = 40
            {
                height = topRight.y - bottomLeft.y;
            }
            
        }
        else
        {
            height = 5;

            if (bottomLeft.x < 0)
            {

                if (topRight.x < 0)   
                {
                    width = bottomLeft.x * -1 - topRight.x * -1;
                }
                else 
                {
                    width = bottomLeft.x * -1 + topRight.x;
                }
            }
            else
            {
                width = topRight.x - bottomLeft.x;
            }
        }
    }

}