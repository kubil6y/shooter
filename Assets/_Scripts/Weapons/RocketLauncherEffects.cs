using System;
using UnityEngine;

public class RocketLauncherEffects : MonoBehaviour {
    [SerializeField] private ProjectileWeapon m_rocketLauncher;

    private void Start() {
        m_rocketLauncher.OnFired += RocketLauncher_OnFired;
    }

    private void RocketLauncher_OnFired(object sender, EventArgs e) {
        Debug.Log("RocketLauncher fired!");
    }
}
