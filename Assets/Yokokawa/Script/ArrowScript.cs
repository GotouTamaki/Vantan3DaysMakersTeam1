using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject player;
    private Player_Scripts Player_Scripts;
    [SerializeField] float _adjustY = 0;

    SpriteRenderer spriteRenderer;

    private Vector3 player_Position;

    private Vector3 drag_Velocity;

    private Vector3 arrow_scale = new(1, 1, 1);
    [SerializeField] float drag_magnitude = 0.5f;
    private float pull_power;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        Player_Scripts = player.GetComponent<Player_Scripts>();
    }


    void Update()
    {

        if (!Player_Scripts.GetIsShooted && Input.GetMouseButton(0))
        {
            spriteRenderer.enabled = true;
            player_Position = Player_Scripts.GetPlayerPosition;
            this.transform.position = player_Position;

            Quaternion x = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
            Quaternion y = Quaternion.AngleAxis(-90, new Vector3(0, 1, 0));

            drag_Velocity = Player_Scripts.GetDragVelocityPosition;
            // drag_Velocity = drag_Velocity.normalized;
            Quaternion q = Quaternion.LookRotation(drag_Velocity) * y * x;

            this.transform.localRotation = q;

            pull_power = Player_Scripts.GetPullPower;
            this.transform.localScale = arrow_scale * drag_magnitude * pull_power;

            this.transform.position = new Vector3(this.transform.position.x, _adjustY, this.transform.position.z);
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

}


