using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using BeatSaberAP;

public static class APConnection {

    private static ArchipelagoSession session = null;
    private static DeathLinkService dlservice = null;

    public static void ConnectAndGetSlotData(string ip, int port, string slot, string password) {
        session = ArchipelagoSessionFactory.CreateSession(ip, port);
        LoginResult result = session.TryConnectAndLogin("Beat Saber", slot, ItemsHandlingFlags.NoItems, null, null, null, password);
        if (!result.Successful) {
            Plugin.Log.Error("Could not connect to Archipelago!");
            return;
        }
        Plugin.Log.Info("Connected to Archipelago");
        var success = (LoginSuccessful)result;
        if ((long)success.SlotData["DeathLink"] == 1) {
            dlservice = session.CreateDeathLinkService();
            dlservice.EnableDeathLink();
        }
    }

    #warning TODO receive deathlink
    public static void SendDeathLink(string cause) {
        if (dlservice == null) return;
        DeathLink dl = new(session.Players.ActivePlayer.Alias, cause);
        dlservice.SendDeathLink(dl);
    }
}
