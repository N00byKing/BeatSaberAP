using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using UnityEngine;

[ViewDefinition("BeatSaberAP.connection-options.bsml")]
[HotReload(RelativePathToLayout = @"connection-options.bsml")]
public class ArchipelagoViewController : BSMLAutomaticViewController {
    [UIAction("clickedConnect")]
    void ConnectButtonPress() {
        APConnection.ConnectAndGetSlotData("localhost", 38281, "blok", "");
    }
}
