using System;
using Verse;
using RimWorld;

namespace GetTogethers
{
    public abstract class Communication : ICommunicable, IExposable, ILoadReferenceable
    {
        private int id;

        private NotificationLetter letter;
        protected DynamicCommManager manager; 

        public Communication() : this(0) {}

        public Communication(int id)
        {
            this.id = id;
        }

        public abstract string GetCallLabel ();

        public virtual string GetInfoText()
        {
            return string.Empty;
        }

        public string GetUniqueLoadID()
        {
            return this.GetType ().Name + id.ToString ();
        }

        public bool IsAllowedFor(Pawn pawn)
        {
            return IsAllowedFor (pawn, out string tmp);
        }

        public void Remove()
        {
            this.manager?.RemoveCommunication(this);
            this.manager = null;
        }

        public void SetManager(DynamicCommManager manager) => this.manager = manager;

        //Called by the letter in the PostLoadInit phase of loading
        public void SetNotificationLetter(NotificationLetter letter)
        {
            this.letter = letter;
        }

        public void TryRemoveNotificationLetter()
        {
            if(this.letter == null) 
                return;
    
            Find.LetterStack.RemoveLetter (this.letter);
            this.letter = null;
        }

        public void TryOpenComms (Pawn negotiator)
        {
            OpenCommunications (negotiator);
        }

        public abstract void OpenCommunications (Pawn negotiator);

        public virtual void ExposeData ()
        {
            Scribe_Values.Look<int> (ref this.id, "ID", 0);
        }

        public virtual bool IsAllowedFor(Pawn pawn, out string disabledReason)
        {
            disabledReason = null;
            return true;
        }

        public virtual bool PotentiallyFor (Pawn pawn)
        {
            return true;
        }
    }
}
