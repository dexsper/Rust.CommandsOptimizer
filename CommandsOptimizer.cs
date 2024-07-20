using System;
using System.Collections.Generic;
using HarmonyLib;
using Network;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Ext.ZString;
using UnityEngine;

namespace Oxide.Plugins.CommandsOptimizer
{
    public static class CommandBuilder
    {
        private static Utf8ValueStringBuilder _sb = ZString.CreateUtf8StringBuilder();

        public static ReadOnlySpan<byte> BuildCommand(ReadOnlySpan<char> command, params object[]? args)
        {
            _sb.Clear();
            _sb.Append(command);

            if (args == null || args.Length == 0)
                return _sb.AsSpan();

            foreach (object obj in args)
            {
                switch (obj)
                {
                    case null:
                        _sb.Append(" \"\"");
                        break;
                    case Color color:
                        _sb.AppendFormat(" \"{0},{1},{2},{3}\"", color.r, color.g, color.b, color.a);
                        break;
                    case Vector3 vector:
                        _sb.AppendFormat(" \"{0},{1},{2}\"", vector.x, vector.y, vector.z);
                        break;
                    case IEnumerable<string> strings:
                    {
                        foreach (var value in strings)
                        {
                            _sb.Append(" ");
                            QuoteSafe(ref _sb, value);
                        }

                        break;
                    }
                    default:
                        _sb.Append(" ");
                        QuoteSafe(ref _sb, obj.ToString());
                        break;
                }
            }

            return _sb.AsSpan();
        }

        private static void QuoteSafe(ref Utf8ValueStringBuilder builder, string value)
        {
            builder.Append('"');
            int startIndex = 0;

            for (int index = 0; index < value.Length; ++index)
            {
                if (value[index] != '"')
                    continue;

                int count = index - startIndex;
                if (count > 0)
                    builder.Append(value.AsSpan(startIndex, count));

                builder.Append("\\\"");
                startIndex = index + 1;
            }

            if (startIndex < value.Length)
                builder.Append(value.AsSpan(startIndex, value.Length - startIndex));

            builder.Append('"');
        }
    }

    [Info("Commands Optimizer", "Dexsper", "1.0.0")]
    public class CommandsOptimizer : RustPlugin
    {
        [AutoPatch]
        [HarmonyPatch(typeof(ConsoleNetwork))]
        static class ConsoleNetwork_ClientCommand_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(ConsoleNetwork.SendClientCommand))]
            [HarmonyPatch(new[] { typeof(Connection), typeof(string), typeof(object[]) })]
            public static bool SendClientCommandPrefix(Connection cn, ref string strCommand, ref object[] args)
            {
                if (!Net.sv.IsConnected() || Interface.CallHook("OnSendCommand", cn, strCommand, args) != null)
                    return false;

                NetWrite netWrite = CreateCommand(ref strCommand, ref args);
                netWrite.Send(new SendInfo(cn));

                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(ConsoleNetwork.SendClientCommand))]
            [HarmonyPatch(new[] { typeof(List<Connection>), typeof(string), typeof(object[]) })]
            public static bool SendClientCommandPrefix2(List<Connection> cn, ref string strCommand, ref object[] args)
            {
                if (!Net.sv.IsConnected() || Interface.CallHook("OnSendCommand", cn, strCommand, args) != null)
                    return false;

                NetWrite netWrite = CreateCommand(ref strCommand, ref args);
                netWrite.Send(new SendInfo(cn));

                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(ConsoleNetwork.BroadcastToAllClients))]
            [HarmonyPatch(new[] { typeof(string), typeof(object[]) })]
            public static bool BroadcastToAllClientsPrefix(ref string strCommand, ref object[] args)
            {
                if (!Net.sv.IsConnected() || Interface.CallHook("OnBroadcastCommand", strCommand, args) != null)
                    return false;

                NetWrite netWrite = CreateCommand(ref strCommand, ref args);
                netWrite.Send(new SendInfo(Net.sv.connections));

                return false;
            }

            public static NetWrite CreateCommand(ref string strCommand, ref object[] args)
            {
                NetWrite netWrite = Net.sv.StartWrite();
                netWrite.PacketID(Message.Type.ConsoleCommand);

                ReadOnlySpan<byte> commandBytes = CommandBuilder.BuildCommand(strCommand, args);
                switch (commandBytes.Length)
                {
                    case 0:
                        netWrite.WriteUInt32(0U, false);
                        break;
                    default:
                        netWrite.WriteUInt32((uint)commandBytes.Length, false);
                        netWrite.Write(commandBytes);
                        break;
                }

                return netWrite;
            }
        }
    }
}
