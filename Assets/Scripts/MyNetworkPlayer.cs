using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColourRenderer;

    [SyncVar(hook =nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "Missing name";

    [SyncVar(hook = nameof(HandleDisplayColourUpdated))]
    [SerializeField]
    private Color displayColour = Color.black;

    #region Server
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColour(Color newDisplayColour)
    {
        displayColour = newDisplayColour;
    }

    [Command]
    private void CmdSetDisplayname (string newDisplayName)
    {
        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }
    #endregion

    #region Client
    private void HandleDisplayColourUpdated (Color oldColour, Color newColour)
    {
        displayColourRenderer.material.SetColor("_BaseColor", newColour);
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayname("My new name");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }
    #endregion
}
