using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject player;
    [SerializeField] Player_Scripts player_Scripts;

    Vector3 drag_Position;
    float drag_magnitude;

    Vector3 player_position;

    Vector3 debug_position;

    void Start()
    {
       

    }

   
    void Update()
    {
       //drag_Position =  player_Scripts.GetDragVelocityPosition;
       //drag_magnitude =  drag_Position.magnitude;

        if (Input.GetMouseButtonDown(0))
        { 
            Quaternion q = Quaternion.Euler(debug_position);
            player_position = player_Scripts.GetPlayerPosition;

            Debug.Log(player_position);
            this.transform.position = player_position;
            this.transform.rotation = q;
            
            
        }
        
    }
}
