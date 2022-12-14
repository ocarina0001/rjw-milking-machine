using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using rjw;

namespace MilkingMachine
{
    public class PenisMilkingHediff : HediffWithComps
    {
        public static bool Sexperience = ModsConfig.IsActive("rjw.sexperience");

        public static float penisMultiplier = 1;
        public static float traitMultiplier = 1;
        public static float quirkMultiplier = 1;
        public static float horse = 1;
        public static float dragon = 1;
        public static float dog = 1;
        public static float demon = 1;
        public override void Tick()
        {
            Pawn pawn = this.pawn;
            Need sexNeed = pawn.needs.TryGetNeed(VariousDefOf.Sex);
            if (pawn.IsHashIntervalTick(300)) // 60000
            {
                if (pawn.IsColonist || pawn.IsPrisoner || pawn.IsSlave)
                {
                    IEnumerable<Hediff> penises = pawn.GetGenitalsList().Where(genitalHediff => Custom_Genital_Helper.is_penis(genitalHediff));
                    bool hasPenis = !penises.EnumerableNullOrEmpty();
                    if (hasPenis)
                    {
                        foreach (Hediff penis in penises)
                        {
                            if (penis.LabelBase.ToLower().Contains("peg")) // Wood can't cum
                                return;
                            CompHediffBodyPart rjwPenisHediff = penis.TryGetComp<CompHediffBodyPart>();
                            if (rjwPenisHediff != null)
                            {
                                // Humans can produce 1.25ml to 5ml of semen per day which averages at 3.125ml (1)
                                // Horses produce about 50ml which is (16)
                                // Dragons produce 3gal or 11356.2ml which is x3633.984 UsedCondoms (3663)
                                // Dragon semen will be output into half-gallon jars (6)
                                // Dogs produce 1ml to 30ml of semen, average 15ml (4)
                                // Demons probably produce x666 that of a human (666)
                                PartSizeExtension.TryGetLength(penis, out float penisLength);
                                PartSizeExtension.TryGetGirth(penis, out float penisGirth);
                                penisMultiplier = penisLength / penisGirth;
                                // Racial penis checks
                                if (penis.LabelBase.ToLower().Contains("equine"))
                                    horse = 16;
                                if (penis.LabelBase.ToLower().Contains("canine"))
                                    dog = 4;
                                if (penis.LabelBase.ToLower().Contains("demon"))
                                    demon = 6.66f;
                                if (penis.LabelBase.ToLower().Contains("dragon"))
                                {
                                    Thing dragonPenisThing = ThingMaker.MakeThing(VariousDefOf.LM_DragonCum);
                                    dragonPenisThing.stackCount = (int)(6);
                                    GenPlace.TryPlaceThing(dragonPenisThing, pawn.Position, pawn.Map, ThingPlaceMode.Near);
                                    return;
                                }
                                Thing penisThing = ThingMaker.MakeThing(VariousDefOf.UsedCondom);
                                if (Sexperience == true)
                                    penisThing = ThingMaker.MakeThing(VariousDefOf.GatheredCum);
                                // Trait and quirk checks
                                if (pawn.story.traits.HasTrait(VariousDefOf.LM_HighTestosterone) || pawn.story.traits.HasTrait(VariousDefOf.LM_NaturalCow))
                                    traitMultiplier = 3;
                                if (pawn.Has(Quirk.Messy))
                                    quirkMultiplier = 2;
                                penisThing.stackCount = (int)(pawn.BodySize * penisMultiplier * traitMultiplier * quirkMultiplier * horse * dog * demon);
                                if (penisThing.stackCount < 1)
                                    penisThing.stackCount = 1;
                                GenPlace.TryPlaceThing(penisThing, pawn.Position, pawn.Map, ThingPlaceMode.Near);
                                sexNeed.CurLevel += 1;
                            }
                        }
                    }
                }
                else
                    return;
            }
        }
    }
}
