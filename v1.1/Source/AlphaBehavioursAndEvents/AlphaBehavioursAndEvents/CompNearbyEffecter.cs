﻿using RimWorld;
using Verse;
using System;
using System.Collections.Generic;


namespace AlphaBehavioursAndEvents
{
    class CompNearbyEffecter : ThingComp
    {

        private int tickProgress = 0;
        
        public CompProperties_NearbyEffecter Props
        {
            get
            {
                return (CompProperties_NearbyEffecter)this.props;
            }
        }
        public override void CompTick()
        {
            this.tickProgress += 1;
            if (this.tickProgress > Props.ticksConversionRate)
            {
                Pawn pawn = this.parent as Pawn;
                if ((!pawn.Downed)&&(pawn.Map != null)) {
                    CellRect rect = GenAdj.OccupiedRect(pawn.Position, pawn.Rotation, IntVec2.One);
                    rect = rect.ExpandedBy(2);

                    foreach (IntVec3 current in rect.Cells)
                    {
                        if (current.InBounds(pawn.Map))
                        {
                            HashSet<Thing> hashSet = new HashSet<Thing>(current.GetThingList(pawn.Map));
                            if (hashSet != null)
                            {
                                Thing current2 = hashSet.FirstOrFallback();
                                if (current2 != null)
                                {
                                    if (current2.def.defName == Props.thingToAffect)
                                    {
                                        Thing thing = GenSpawn.Spawn(ThingDef.Named(Props.thingToTurnTo), current, pawn.Map, WipeMode.Vanish);
                                        current2.Destroy();
                                        break;
                                    } else if (current2.def.defName == Props.secondaryThingToAffect)
                                    {
                                        Thing thing = GenSpawn.Spawn(ThingDef.Named(Props.thingToTurnTo), current, pawn.Map, WipeMode.Vanish);
                                        current2.Destroy();
                                        break;
                                    }
                                }
                            }

                        }

                    }
                }
                
                // FilthMaker.MakeFilth(this.parent.PositionHeld, this.parent.MapHeld, ThingDef.Named("GR_FilthMucus"), 1);
                this.tickProgress = 0;
            }
        }
    }
}