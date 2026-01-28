// Decompiled with JetBrains decompiler
// Type: Bots.Quest.Objectives.DecoratorCanMoveToGrindArea
// Assembly: Honorbuddy, Version=2.0.0.5999, Culture=neutral, PublicKeyToken=50a565ab5c01ae50
// MVID: FB7FEB85-27C0-4D17-B8DE-615FDFDA7752
// Assembly location: C:\Users\Texy6\Desktop\Honorbuddy-cleaned.exe

using Styx.Logic;
using Styx.Logic.AreaManagement;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

#nullable disable
namespace Bots.Quest.Objectives;

public class DecoratorCanMoveToGrindArea : Decorator
{
    private readonly GrindArea grindArea;

    public DecoratorCanMoveToGrindArea(GrindArea area, Composite decorated)
        : base(decorated)
    {
        this.grindArea = area;
    }

    protected override bool CanRun(object context)
    {
        return (!((Area)this.grindArea != (Area)null) || this.grindArea.Hotspots.Count != 0) && 
               (Area)this.grindArea != (Area)null && 
               LootTargeting.Instance.FirstObject == (WoWObject)null && 
               this.grindArea.HotspotChanged;
    }
}
