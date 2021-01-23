

using System.Data;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MMOPlugin
{

    [ApiVersion(2, 1)]
    public class MMO : TerrariaPlugin
    {
        public static SqlTableEditor SQLEditor;
        public static SqlTableCreator SQLWriter;

        public override string Author => "647";

        public override string Description => "Template for TShock Plugins for Terraria Server with Terraria Version 1.3.5.3";

        public override string Name => "Terraria Plugin Template";


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Methods to perform when the Plugin is disposed i.e. unhooks
                ServerApi.Hooks.ServerChat.Deregister(this, OnServerChat);
                GetDataHandlers.TogglePvp -= OnTogglePvp;
            }
        }

        public MMO(Main game) : base(game)
        {
            // Load priority. smaller numbers loads earlier
            Order = 1;
            Console.WriteLine(game.ToString());
        }

        public override void Initialize()
        {
            InitTable();
            ServerApi.Hooks.ServerChat.Register(this, OnServerChat);
            GetDataHandlers.TogglePvp += OnTogglePvp;

        }

        // This is the ServerChat's callback function; this function is called
        // whenever the ServerChat hook is fired, which is upon receiving a message
        // from the client.
        // This example acts as a debug and outputs the message to the console.
        void OnServerChat(ServerChatEventArgs args)
        {
            Console.WriteLine("DEBUG: {0}", args.Text);
        }

        // This is the TogglePvp handler. This function is called whenever the server
        // receives packet #30 (TogglePvP)
        void OnTogglePvp(object sender, TShockAPI.GetDataHandlers.TogglePvpEventArgs args)
        {
            Console.WriteLine("{0} has just {1} their PvP Status.",
              TShock.Players[args.PlayerId].Name, args.Pvp ? "ENABLED" : "DISABLED");
        }

        private void InitTable()
        {
            // Methods to perform when plugin is initzialising i.e. hookings
            SQLEditor = new SqlTableEditor(TShock.DB, TShock.DB.GetSqlType() == SqlType.Sqlite ? (IQueryBuilder)new SqliteQueryCreator() : new MysqlQueryCreator());
            SQLWriter = new SqlTableCreator(TShock.DB, TShock.DB.GetSqlType() == SqlType.Sqlite ? (IQueryBuilder)new SqliteQueryCreator() : new MysqlQueryCreator());

            var columnID = new SqlColumn("ID", MySqlDbType.Int32) { Unique = true };
            var columnXp = new SqlColumn("XP", MySqlDbType.Int32) { DefaultValue = "0" };

            var table = new SqlTable("MMO", columnID, columnXp);

            // Creates MMO table in Database if does not exist
            var ensure = SQLWriter.EnsureTableStructure(table);
            if (!ensure)
            {

                List<SqlValue> users = new List<SqlValue>();

                List<object> objects = SQLEditor.ReadColumn("Users", "ID", users);

                // Copy all server existing users ID to MMO
                //
                // With initial 0 experience
                for (int i = 0; i < objects.Count; i++)
                {
                    try
                    {
                        int id = Int32.Parse(SQLEditor.ReadColumn("Users", "ID", new List<SqlValue>())[i].ToString());

                        List<SqlValue> columns = new List<SqlValue>();

                        columns.Add(new SqlValue("ID", id));
                        columns.Add(new SqlValue("XP", "0"));

                        SQLEditor.InsertValues("MMO", columns);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }
    }
}
