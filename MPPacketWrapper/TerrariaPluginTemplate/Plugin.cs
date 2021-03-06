using TShockAPI;
using System;
using TerrariaApi.Server;
using System.Net;
using System.Collections.Specialized;

public class MPPacketWrapperPlugin
{

    public MPPacketWrapperPlugin() { }

    private static string ByteToHexBitFiddle(byte[] bytes)
    {
        char[] c = new char[bytes.Length * 2];
        int b;
        for (int i = 0; i < bytes.Length; i++)
        {
            b = bytes[i] >> 4;
            c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
            b = bytes[i] & 0xF;
            c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
        }
        return new string(c);
    }

    // Encode buffer to HEX and send to Endpoint
    public void HandleData(GetDataEventArgs args)
    {
        int playerId = args.Msg.whoAmI;

        byte[] buffer = args.Msg.readBuffer;
        int index = args.Index;
        int length = args.Length;

        string result = "";

        string packetData = ByteToHexBitFiddle(buffer);

        try
        {
            using (var client = new WebClient())
            {

                int id = (int)args.MsgID;

                var values = new NameValueCollection
                {
                    ["whoAmI"] = playerId.ToString(),
                    ["packetId"] = id.ToString(),
                    ["index"] = index.ToString(),
                    ["length"] = length.ToString(),
                    ["packetData"] = packetData,
                };

                client.UploadValues("http://localhost:10000/", values);
            }
        }
        catch (Exception e)
        {

        }


        if (result.Length > 0)
            TShock.Log.ConsoleInfo("[OnGetData]: {0}", result);
    }
}