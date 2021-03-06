using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using System;

namespace MMO
{

    [ApiVersion(2, 1)]
    public class MMO : TerrariaPlugin
    {
        private MPPacketWrapperPlugin plugin;
        public override string Author => "647";
        public override string Description =>
            "Multiplayer Packet Wrapper for tShock Servers using custom Endpoints";
        public override string Name => "MP Packet Wrapper";

        public MMO(Main game) : base(game) { }
        internal delegate bool GetDataHandlerDelegate(GetDataHandlerArgs args);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                HookManager serverHooks = ServerApi.Hooks;

                serverHooks.NetGetData.Deregister(this, OnGetData);

                plugin = null;
            }
        }

        public override void Initialize()
        {
            HookManager serverHooks = ServerApi.Hooks;

            serverHooks.NetGetData.Register(this, OnGetData);

            GetDataHandlers.InitGetDataHandler();

            plugin = new MPPacketWrapperPlugin();
        }

        private void OnGetData(GetDataEventArgs args)
        {
            if (args.Handled) return;

            plugin.HandleData(args);
        }

    }
}
