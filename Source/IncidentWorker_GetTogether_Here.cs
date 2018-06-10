using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using RimWorld;
using RimWorld.Planet;

namespace GetTogethers
{
    public class IncidentWorker_GetTogether_Here : IncidentWorker
    {
        protected override bool CanFireNowSub(IIncidentTarget target)
        {
            Map map = target as Map;
            if(map == null)
                return false;

            var availColonists = target.FreeColonistsForStoryteller;
            if(!availColonists.Any())
                return false;
            
            //Approaching this from the WorldPawn's perspective as most won't have relations
            
            //Should select any outsider that is more important than an Ex-Spouse
            //      That includes cousins, step-family, 
            Func<Pawn, bool> pawnIsAvailAndRelatedToAvailColonist = outsider => {
                if(!(outsider.relations?.RelatedToAnyoneOrAnyoneRelatedToMe ?? false))
                    return false;
                var situation = Find.WorldPawns.GetSituation(outsider);
                if(situation != WorldPawnSituation.CaravanMember && situation != WorldPawnSituation.Free)
                    return false;
                return availColonists.Any(colonist => (colonist.GetMostImportantRelation(outsider)
                                            ?.importance ?? 0) > PawnRelationDefOf.ExSpouse.importance);
            };

            return Find.WorldPawns.AllPawnsAlive.Any(pawnIsAvailAndRelatedToAvailColonist);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;
            if(map == null)
                return false;
        
            var availColonists = parms.target.FreeColonistsForStoryteller;
            if(!availColonists.Any())
                return false;
        
            var potentialGetTogethers = Find.WorldPawns.AllPawnsAlive.SelectMany
                                            (outsider => GetTogetherersWithOutsider(outsider, availColonists));
            if(!potentialGetTogethers.Any())
                return false;
        
            //Using the square of the relation importance
            potentialGetTogethers.TryRandomElementByWeight(gt => {
                float importance = gt.Item1.GetMostImportantRelation(gt.Item2).importance;
                return importance * importance;
                }, out var chosenGetTogether);
        
            if(chosenGetTogether == null)
                return false;

            DCMTools.DynamicCommManagerFor(map).AddCommunicationWithLetter(new Comm_LetsGetTogether_Contact
                                                                (chosenGetTogether.Item1, chosenGetTogether.Item2)
                                                            , "GT_IncomingMessage.Title".Translate(), "GT_IncomingMessage.Title".Translate()
                                                            , LetterDefOf.NeutralEvent, lookAtComms: true
                                                            , Constants.InitialGuestContactTimeout, removeWhenDisabled: true);
            return true;
        }
        
        protected IEnumerable<Tuple<Pawn, Pawn>> GetTogetherersWithOutsider(Pawn outsider, IEnumerable<Pawn> availColonists)
        {
            if(!(outsider.relations?.RelatedToAnyoneOrAnyoneRelatedToMe ?? false))
                yield break;
            var situation = Find.WorldPawns.GetSituation(outsider);
            if(situation != WorldPawnSituation.CaravanMember && situation != WorldPawnSituation.Free)
                yield break;
        
            foreach(var colonist in availColonists)
                if((outsider.GetMostImportantRelation(colonist)?.importance ?? 0)
                    > PawnRelationDefOf.ExSpouse.importance)
                    yield return Tuple.Create(colonist, outsider);
        }
    }
}
