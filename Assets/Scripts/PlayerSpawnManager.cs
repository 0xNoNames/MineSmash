using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform[] spawnLocations;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.GetComponent<PlayerDetails>().Initialize(playerInput.playerIndex + 1, spawnLocations[playerInput.playerIndex].position);

        UIManager.Instance.getPlayerUI(playerInput.playerIndex).gameObject.SetActive(true);

        GameManager.Instance.playerList.Add(playerInput.GetComponent<PlayerDetails>());
    }
}
