using Verse;
using System;
using UnityEngine;
using RimWorld.Planet;

namespace GetTogethers
{
    public static class DialogMaker
    {
        public static Window MakeRequestFromGuestDialog(Pawn host, Pawn guest, Pawn negotiator, Comm_LetsGetTogether_Contact comm)
        {
            DiaNode rootNode = new DiaNode(string.Empty);
            string dialogTitle = null;
            bool hostIsWhoAnswered = negotiator == host;
            bool callingFromCaravan = Find.WorldPawns.GetSituation(guest) == WorldPawnSituation.CaravanMember;

            string caravanSubString = callingFromCaravan ? ".Caravan" : string.Empty;
            string hostIsWhoAnsweredString = hostIsWhoAnswered ? string.Empty
                    : ".NotHost" + (comm.timesNegotiatorWasNotHost++).ToString();

            dialogTitle = ("GT_RequestFromGuest" + caravanSubString + ".Title").Translate(host, guest, negotiator);
            rootNode.text = ("GT_RequestFromGuest" + caravanSubString + hostIsWhoAnsweredString 
                                       + ".Text").Translate(host, guest, negotiator);
            if(hostIsWhoAnswered) {
                rootNode.options.Add(new DiaOption(("GT_RequestFromGuest" + caravanSubString + ".Accept")
                                                        .Translate(host, guest, negotiator)) {
                    resolveTree = true,
                    action = () => GetTogethersUtility.StartPreparationsForGetTogether(host, guest)
                });
                rootNode.options.Add(new DiaOption(("GT_RequestFromGuest" + caravanSubString + ".Decline")
                                                        .Translate(host, guest, negotiator)) {
                    resolveTree = true,
                    action = comm.Remove
                });
            }
            else {
                rootNode.options.Add(new DiaOption(("GT_RequestFromGuest" + caravanSubString
                                          + hostIsWhoAnsweredString + ".Accept").Translate(host, guest, negotiator)) {
                    resolveTree = true,
                    action = (comm.timesNegotiatorWasNotHost > 4) ? comm.Remove : (Action) null
                });
                rootNode.options.Add(new DiaOption(("GT_RequestFromGuest" + caravanSubString
                                          + hostIsWhoAnsweredString + ".Decline").Translate(host, guest, negotiator)) {
                    resolveTree = true,
                    action = comm.Remove
                });
            }

            return new Dialog_NodeTree(rootNode, false, false, dialogTitle);
        }
    }
}
