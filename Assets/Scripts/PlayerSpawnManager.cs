using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform[] spawnLocations;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerDetails pd = playerInput.GetComponent<PlayerDetails>();

        // Initialisation du joueur (pour le faire spawn au bon endroit)
        pd.Initialize(playerInput.playerIndex, spawnLocations[playerInput.playerIndex].position);

        // Affichage de son UI
        UIManager.Instance.GetPlayerUI(playerInput.playerIndex).gameObject.SetActive(true);
        UIManager.Instance.GetPlayerUI(playerInput.playerIndex).SetPlayerName(pd.playerName);

        // Ajout du joueur au GameManager
        GameManager.Instance.playerList.Add(playerInput.GetComponent<PlayerDetails>());
    }
}
