using BeatSaberAP;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using TMPro;
using UnityEngine;

[ViewDefinition("BeatSaberAP.connection-options.bsml")]
[HotReload(RelativePathToLayout = @"connection-options.bsml")]
public class ArchipelagoViewController : BSMLAutomaticViewController {
    string host = "";
    [UIComponent("hostOut")] TextMeshProUGUI hostOut;
    string port = "";
    [UIComponent("portOut")] TextMeshProUGUI portOut;
    string slot = "";
    [UIComponent("slotOut")] TextMeshProUGUI slotOut;
    string pass = "";
    [UIComponent("passOut")] TextMeshProUGUI passOut;
    [UIComponent("connectionStatus")] public TextMeshProUGUI connStatus;

    [UIAction("enteredHost")]
    void EnteredHostValue(string entered) {
        host = entered;
        hostOut.text = host;
    }
    [UIAction("enteredPort")]
    void EnteredPortValue(string entered) {
        port = entered;
        portOut.text = port;
    }
    [UIAction("enteredSlot")]
    void EnteredSlotValue(string entered) {
        slot = entered;
        slotOut.text = slot;
    }
    [UIAction("enteredPass")]
    void EnteredPassValue(string entered) {
        pass = entered;
        passOut.text = pass;
    }

    [UIAction("clickedConnect")]
    void ConnectButtonPress() {
        if (APConnection.ConnectAndGetSlotData(host, Convert.ToInt32(port), slot, pass))
            connStatus.text = "Connected Successfully";
        else
            connStatus.text = "Connection failed";
    }
}
