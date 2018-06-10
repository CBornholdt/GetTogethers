using System;
using Verse;
using RimWorld;

namespace GetTogethers
{
    public class NotificationLetter : StandardLetter
    {
        public Communication communication;
        public bool persistWithComm = false;

        protected override string PostProcessedLabel ()
        {
            return this.label;
        }

        public override void ExposeData()
        {
            base.ExposeData ();
            Scribe_References.Look<Communication> (ref this.communication, "Communication");
            Scribe_Values.Look<bool>(ref this.persistWithComm, "PersistWithComm");
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
                communication.SetNotificationLetter(this);
        }

        public override void OpenLetter()
        {
            // the +1 is due to LetterStack checking early ...
            if (this.disappearAtTick <= Find.TickManager.TicksGame + 1)
                return;	//Do nothing if opened due to timeout
            base.OpenLetter ();
            if(persistWithComm) //Readd to stack, the Comm is responsible for removing us
                Find.LetterStack.ReceiveLetter(this);
        }
    }
}

