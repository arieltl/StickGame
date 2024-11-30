using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class DynamicTargetGroup : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;
    public CinemachineVirtualCamera defaultCamera;
    public float defaultCameraDuration = 3f; // Time to keep the default camera active
    private bool isUsingDefaultCamera = true; // Tracks if we're using the default camera

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Start with the default camera for a specified duration
        StartCoroutine(SwitchToTargetGroupAfterDelay());
    }

    private IEnumerator SwitchToTargetGroupAfterDelay()
    {
        // Keep the default camera active for the specified duration
        defaultCamera.Priority = 20;
        isUsingDefaultCamera = true;

        yield return new WaitForSeconds(defaultCameraDuration);

        // Switch to target group camera
        isUsingDefaultCamera = false;
        UpdateCameraPriority();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-add players to the target group
        var proxies = GameObject.FindGameObjectsWithTag("PlayerProxy");
        foreach (var proxy in proxies)
        {
            AddPlayer(proxy);
        }
    }

    private void Update()
    {
        if (!isUsingDefaultCamera)
        {
            UpdateCameraPriority();
        }
    }

    private void UpdateCameraPriority()
    {
        if (targetGroup.m_Targets.Length == 0)
        {
            // Activate the default camera if no targets in the target group
            defaultCamera.Priority = 20;
        }
        else
        {
            // Deactivate the default camera when there are targets
            defaultCamera.Priority = 0;
        }
    }

    // Add a new player to the Target Group
    public void AddPlayer(GameObject player)
    {
        if (targetGroup == null) return;

        // Create a new array with one more slot for the new player
        var targets = new CinemachineTargetGroup.Target[targetGroup.m_Targets.Length + 1];

        // Copy the old targets to the new array
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            targets[i] = targetGroup.m_Targets[i];
        }

        // Add the new player to the last slot
        targets[targets.Length - 1] = new CinemachineTargetGroup.Target
        {
            target = player.transform, 
            weight = 1f,              
            radius = 4.5f               
        };

        // Replace the Target Group's targets with the new array
        targetGroup.m_Targets = targets;
    }

    // Remove a player from the Target Group
    public void RemovePlayer(GameObject player)
    {
        if (targetGroup == null) return;

        var targets = new List<CinemachineTargetGroup.Target>();

        // Retain only targets that aren't the specified player
        foreach (var target in targetGroup.m_Targets)
        {
            if (target.target != player.transform)
            {
                targets.Add(target);
            }
        }

        // Update the Target Group's targets
        targetGroup.m_Targets = targets.ToArray();
    }
}
