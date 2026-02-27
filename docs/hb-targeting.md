# HB Targeting — DefaultRemoveTargetsFilter & IsNotWithinHotspotRange (HB 4.3.4 / HB 6.2.3)

## HB 4.3.4 — Honorbuddy\Honorbuddy\Styx\Logic\Targeting.cs (extraits)

```
protected virtual void DefaultRemoveTargetsFilter(List<WoWObject> units)
{
	Blacklist.Flush();
	bool flag = StyxWoW.Me.IsInParty || StyxWoW.Me.IsInRaid;
	double num = Targeting.CollectionRange * Targeting.CollectionRange;
	Profile currentProfile = ProfileManager.CurrentProfile;
	HashSet<ulong> hashSet = new HashSet<ulong>();
	HashSet<ulong> hashSet2 = new HashSet<ulong>();
	if (StyxWoW.Me.IsInRaid)
	{
		using (List<WoWPlayer>.Enumerator enumerator = StyxWoW.Me.RaidMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				WoWPlayer woWPlayer = enumerator.Current;
				hashSet2.Add(woWPlayer.Guid);
				foreach (WoWUnit woWUnit in woWPlayer.Minions)
				{
					hashSet.Add(woWUnit.Guid);
				}
			}
			goto IL_016B;
		}
	}
	if (StyxWoW.Me.IsInParty)
	{
		foreach (WoWPlayer woWPlayer2 in StyxWoW.Me.PartyMembers)
		{
			hashSet2.Add(woWPlayer2.Guid);
			foreach (WoWUnit woWUnit2 in woWPlayer2.Minions)
			{
				hashSet.Add(woWUnit2.Guid);
			}
		}
	}
	IL_016B:
	bool mounted = StyxWoW.Me.Mounted;
	bool isInsideBattleground = Battlegrounds.IsInsideBattleground;
	for (int i = units.Count - 1; i >= 0; i--)
	{
		WoWObject woWObject = units[i];
		if (!(woWObject == null) && woWObject is WoWUnit)
		{
			bool flag2 = woWObject is WoWPlayer;
			WoWUnit woWUnit3 = (WoWUnit)woWObject;
			bool combat = woWUnit3.Combat;
			if (flag2 || !combat || !flag || (!hashSet2.Contains(woWUnit3.CurrentTargetGuid) && !hashSet.Contains(woWUnit3.Guid)))
			{
				bool isAlive = woWUnit3.IsAlive;
				bool petAggro = woWUnit3.PetAggro;
				if (flag2 || !combat || !isAlive || (!petAggro && !woWUnit3.Aggro && !woWUnit3.IsTargetingMeOrPet && !woWUnit3.IsTargetingAnyMinion))
				{
					double distanceSqr = woWUnit3.DistanceSqr;
					bool isFriendly = woWUnit3.IsFriendly;
					if (!flag2 || !combat || !isAlive || distanceSqr >= num || isFriendly || (!woWUnit3.IsTargetingMeOrPet && !woWUnit3.IsTargetingAnyMinion))
					{
						uint entry = woWUnit3.Entry;
						WoWPoint location = woWUnit3.Location;
						if (Targeting.hashSet_0.Contains(entry) || Blacklist.Contains(woWUnit3.Guid, false) || woWUnit3.Dead || distanceSqr > num || woWUnit3.OnTaxi || woWUnit3.IsFlightMaster || (woWUnit3.IsFlying && !isInsideBattleground) || (woWUnit3.TaggedByOther && !flag) || (currentProfile != null && (currentProfile.AvoidMobs.Contains(entry) || currentProfile.AvoidMobs.Contains(woWUnit3.Name))) || (currentProfile != null && (Targeting.IsTooNearBlackspot(currentProfile.Blackspots, location) || this.IsNotWithinHotspotRange(location, false))) || (woWUnit3.IsCritter || woWUnit3.IsNonCombatPet || !woWUnit3.Attackable || (mounted && this.IsNotWithinHotspotRange(location, false))) || isFriendly)
						{
							units.RemoveAt(i);
						}
					}
				}
			}
		}
		else
		{
			units.RemoveAt(i);
		}
	}
}

public bool IsNotWithinHotspotRange(WoWPoint point, bool force = false)
{
	bool flag;
	if ((this.KillBetweenHotspots || Battlegrounds.IsInsideBattleground || StyxWoW.Me.IsInInstance) && !force)
	{
		flag = false;
	}
	else
	{
		bool flag2;
		try
		{
			if (ProfileManager.CurrentProfile != null && ProfileManager.CurrentProfile.GrindArea != null && ProfileManager.CurrentProfile.GrindArea.CurrentHotSpot != null)
			{
				float num = ProfileManager.CurrentProfile.GrindArea.CurrentHotSpot.Position.Distance2D(point);
				flag2 = (double)num >= Targeting.CollectionRange;
			}
			else
			{
				flag2 = false;
			}
		}
		catch
		{
			flag2 = false;
		}
		flag = flag2;
	}
	return flag;
}
```

## HB 6.2.3 — Honorbuddy\Styx\CommonBot\Targeting.cs (extraits)

```
protected virtual void DefaultRemoveTargetsFilter(List<WoWObject> units)
{
	Targeting.Class142 @class = new Targeting.Class142();
	@class.double_0 = Targeting.CollectionRange * Targeting.CollectionRange;
	@class.profile_0 = ProfileManager.CurrentProfile;
	@class.bool_0 = StyxWoW.Me.Combat;
	@class.woWPoint_0 = StyxWoW.Me.Location;
	@class.woWGuid_0 = BotPoi.Current.Guid;
	List<Quest.QuestObjective> list = this.List_0;
	Profile profile_ = @class.profile_0;
	List<ObjectiveInfo> list2;
	if (profile_ != null)
	{
		if ((list2 = profile_.Quests.SelectMany(new Func<QuestInfo, IEnumerable<ObjectiveInfo>>(Targeting.Class139.<>9.method_9)).ToList<ObjectiveInfo>()) != null)
		{
			goto IL_009D;
		}
	}
	list2 = new List<ObjectiveInfo>();
	IL_009D:
	List<ObjectiveInfo> list3 = list2;
	@class.hashSet_0 = new HashSet<uint>();
	@class.hashSet_1 = new HashSet<uint>();
	if (ProfileManager.CurrentProfile != null && ProfileManager.CurrentProfile.QuestOrder.Any<OrderNode>())
	{
		@class.hashSet_0.UnionWith(new HashSet<uint>(list.Where(new Func<Quest.QuestObjective, bool>(Targeting.Class139.<>9.method_10)).Select(new Func<Quest.QuestObjective, uint>(Targeting.Class139.<>9.method_11))));
		@class.hashSet_0.UnionWith(list3.Where(new Func<ObjectiveInfo, bool>(Targeting.Class139.<>9.method_12)).Select(new Func<ObjectiveInfo, uint>(Targeting.Class139.<>9.method_13)));
		@class.hashSet_1.UnionWith(new HashSet<uint>(list.Where(new Func<Quest.QuestObjective, bool>(Targeting.Class139.<>9.method_14)).Select(new Func<Quest.QuestObjective, uint>(Targeting.Class139.<>9.method_15))));
		@class.hashSet_1.UnionWith(list3.Where(new Func<ObjectiveInfo, bool>(Targeting.Class139.<>9.method_16)).Select(new Func<ObjectiveInfo, uint>(Targeting.Class139.<>9.method_17)));
	}
	using (StyxWoW.Memory.AcquireFrame())
	{
		units.RemoveAll(new Predicate<WoWObject>(@class.method_0));
	}
}
```

Fichier créé à partir des sources décompilées dans `hb decompile`; aucun commentaire ni suggestion n'est inclus dans ce fichier.

## Flags, Blackspots et Targeting (extraits du code)

Les extraits ci‑dessous sont strictement descriptifs et tirés des fichiers source présents dans le dépôt.

- `Targeting.DefaultRemoveTargetsFilter` (fichier `Styx/Logic/Targeting.cs`) utilise :
	- le profil courant (`ProfileManager.CurrentProfile`) pour vérifier `AvoidMobs` et `Blackspots` ;
	- la méthode statique `Targeting.IsTooNearBlackspot(IEnumerable<Blackspot>, WoWPoint)` pour exclure les unités situées à l'intérieur ou trop proches d'un `Blackspot` ;
	- la méthode d'instance `IsNotWithinHotspotRange(WoWPoint, bool)` pour déterminer si une unité est en dehors d'un hotspot actif (voir extrait dans ce fichier).

- `Blackspot` / marquage de zones :
	- Les blackspots sont représentés dans les profils et exposés via `Profile.Blackspots` (`Styx/Logic/Profiles/Profile.cs`).
	- Le code qui marque et gère les blackspots dans la navmesh se trouve dans `Styx/Logic/Pathing/BlackspotManager.cs` et dans les appels natifs de `Tripper/Navigation/NativeMethods.cs` (SetPolyArea / SetAreaCost). `Navigator` initialise un coût élevé pour `AreaType.Blackspot`.

- Relation avec les flags de navigation :
	- `StraightPathFlags` (OffMeshConnection) est un métadonné du pathfinder ; il est géré principalement à la couche navigation/post‑processing (`Tripper/Navigation/PathPostProcessor.cs`, `Tripper/Navigation/Navigator.cs`) et par les handlers off‑mesh dans HB (extraits décompilés inclus dans `hb-flag-system.md`).
	- Le système de targeting n'inspecte pas directement `StraightPathFlags` pour décider d'exclure une unité. Il s'appuie sur :
		- les `Blackspots` (zones à éviter) ;
		- les `Hotspots` (zones de grind) et la variable `KillBetweenHotspots` ;
		- les drapeaux de l'unité (IsFlying, OnTaxi, IsFlightMaster, TaggedByOther, etc.) ;
		- les collections `ProtectedItems`, `AvoidMobs`, et d'autres règles de profil.

## Fichiers principaux à consulter (pour targeting / blackspots)

- `Styx/Logic/Targeting.cs` — `DefaultRemoveTargetsFilter`, `IsTooNearBlackspot`, hotspot checks.
- `Styx/Logic/Profiles/Profile.cs` — propriété `Blackspots` et parsing XML des blackspots dans les profils.
- `Styx/Logic/Pathing/BlackspotManager.cs` — gestion, persistance et marquage navmesh des blackspots.
- `Tripper/Navigation/NativeMethods.cs` — API native pour `SetPolyArea`, `SetAreaCost`, et flags poly.
- `Tripper/Navigation/Navigator.cs` — initialisation des `QueryFilter`, `SetDefaultAreaCosts`, lecture des `Flags[]`/`PolyTypes[]` depuis la DLL.

---
Contenu ajouté uniquement à partir des fichiers source du dépôt ; aucune interprétation hors‑code n'est fournie.
