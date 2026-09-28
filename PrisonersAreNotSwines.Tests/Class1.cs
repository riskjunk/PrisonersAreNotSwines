using System.Collections;
using System.Linq;
using DevTools.Testing;
using DevTools.Testing.Utils;
using RimWorld;
using UnityEngine;
using Verse;

namespace PrisonersAreNotSwines.Tests;
[TestFixture(TestType.Playing)]
public class Class1
{
    [Test]
    public IEnumerator Test1()
    {
        var pawn = PawnKindDefOf.Colonist
            .AsBuilder()
            .WithName("ColonistTest")
            .WithWorkMaxed(WorkTypeDefOf.Cleaning)
            .WithPostSpawn((pawn, i) => pawn.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner))
            .Spawn();

        pawn.Position = new IntVec3(75, 0, 79);
        var location = pawn.Position + new IntVec3(-2, 0, -2);

        // GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Wall, GenStuff.DefaultStuffFor(ThingDefOf.Wall)), location,
        //     pawn.Map);

        for (var i = 0; i < 6; i++)
        {
            var PrisonWalls = location + new IntVec3(i, 0, -1);

            GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Wall, GenStuff.DefaultStuffFor(ThingDefOf.Wall)),
                PrisonWalls,
                pawn.Map);

            var PrisonWalls1 = location + new IntVec3(i, 0, 5);

            GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Wall, GenStuff.DefaultStuffFor(ThingDefOf.Wall)),
                PrisonWalls1,
                pawn.Map);
        }

        for (var i = 0; i < 6; i++)
        {
            var PrisonWalls = location + new IntVec3(-1, 0, i);

            GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Wall, GenStuff.DefaultStuffFor(ThingDefOf.Wall)),
                PrisonWalls,
                pawn.Map);

            var PrisonWalls1 = location + new IntVec3(5, 0, i);

            GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Wall, GenStuff.DefaultStuffFor(ThingDefOf.Wall)),
                PrisonWalls1,
                pawn.Map);
        }

        yield return new WaitTicks(10);
        var bed = (Building_Bed)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Bed, GenStuff.DefaultStuffFor(ThingDefOf.Bed)), pawn.Position - new IntVec3(1, 0, 0), pawn.Map);
        bed.SetFaction(Faction.OfPlayer);
        bed.ForOwnerType = BedOwnerType.Prisoner;


        var filthCells = GenRadial.RadialCellsAround(pawn.Position, 0f, 2f).ToArray();
        foreach (var intVec3 in filthCells)
        {
            FilthMaker.TryMakeFilth(intVec3, pawn.Map, ThingDefOf.Filth_Blood);
        }

        Tweaker.SuperFast = true;
        yield return new WaitUntilTimeout(() => filthCells.All(x => !x.GetThingList(pawn.Map).Any(y => y is Filth)));
    }
}