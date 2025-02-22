using System.Collections.Generic;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using BeatSaberAP;
using CustomCampaigns.UI.FlowCoordinators;
using Newtonsoft.Json;

public static class APConnection {

    private static ArchipelagoSession session = null;
    private static DeathLinkService dlservice = null;
    private static Dictionary<uint,uint> NodeToID = null;
    private static Dictionary<uint,uint> IDToNode = null;
    private static readonly List<uint> song_unlocks = [];
    public static string CampaignName { get; private set; }

    public static bool ConnectAndGetSlotData(string ip, int port, string slot, string password) {
        session = ArchipelagoSessionFactory.CreateSession(ip, port);
        LoginResult result = session.TryConnectAndLogin("Beat Saber", slot, ItemsHandlingFlags.AllItems, null, null, null, password);
        if (!result.Successful) {
            Plugin.Log.Error("Could not connect to Archipelago!");
            return false;
        }
        Plugin.Log.Info("Connected to Archipelago");
        var success = (LoginSuccessful)result;
        if ((long)success.SlotData["DeathLink"] == 1) {
            dlservice = session.CreateDeathLinkService();
            dlservice.EnableDeathLink();
        }
        CampaignName = (string)success.SlotData["campaign_name"];
        NodeToID = JsonConvert.DeserializeObject<Dictionary<uint,uint>>(success.SlotData["node_to_mapid"].ToString());
        IDToNode = JsonConvert.DeserializeObject<Dictionary<uint,uint>>(success.SlotData["mapid_to_node"].ToString());
        song_unlocks.Add(JsonConvert.DeserializeObject<uint>(success.SlotData["start_song"].ToString()));
        session.Items.ItemReceived += RecvItem;
        return true;
    }

    #warning TODO receive deathlink
    public static void SendDeathLink(string cause) {
        if (dlservice == null) return;
        DeathLink dl = new(session.Players.ActivePlayer.Alias, cause);
        dlservice.SendDeathLink(dl);
    }

    public static void CheckLocation(uint mapid) {
        session.Locations.CompleteLocationChecks(IDToNode[mapid]);
    }

    private static void RecvItem(ReceivedItemsHelper helper) {
        ItemInfo item = helper.DequeueItem();
        song_unlocks.Add(NodeToID[(uint)item.ItemId]);
    }

    public static bool HaveSong(uint mapid) {
        return song_unlocks.Contains(mapid);
    }

    public enum CampaignValidity {
        NotAP,
        WrongCampaign,
        Correct
    }
    public static CampaignValidity CheckCampaignValid(string name) {
        string selected = CustomCampaignFlowCoordinator.CustomCampaignManager.Campaign.info.name;
        if (!selected.StartsWith("AP Campaign, ")) return CampaignValidity.NotAP; // None of our business
        if (selected != APConnection.CampaignName) {
            // User selected wrong campaign, so mismatch, or CampaignName not initialized, so not connected
            return CampaignValidity.WrongCampaign;
        }
        // Campaign is correct
        return CampaignValidity.Correct;
    }
}
