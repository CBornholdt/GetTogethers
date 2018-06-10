using System;
using Verse;

namespace GetTogethers
{
    public class Comm_LetsGetTogether_Contact : Communication 
    {
        public Pawn host = null;
        public Pawn guest = null;
        public int timesNegotiatorWasNotHost = 0;

        public Comm_LetsGetTogether_Contact(Pawn host, Pawn guest)
        {
            this.host = host;
            this.guest = guest;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Pawn>(ref this.host, "Host");
            Scribe_References.Look<Pawn>(ref this.guest, "Guest");
            Scribe_Values.Look<int>(ref this.timesNegotiatorWasNotHost, "TimesNegotiatorWasNotHost", 0);
        }

        public override string GetCallLabel()
        {
            return "GT_IncomingMessage.CommLabel".Translate();
        }

        public override void OpenCommunications(Pawn negotiator)
        {
            Find.WindowStack.Add (DialogMaker.MakeRequestFromGuestDialog(host, guest, negotiator, this));
        }
    }
}
