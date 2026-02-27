# HB 6.2.3 — Système de flags (StraightPathFlags) et utilisation (loot / navigation)

Ce document décrit strictement, à partir du code décompilé de HonorBuddy 6.2.3, comment le système de "flags" de chemin (Path.Flags / StraightPathFlags / PolyTypes) est utilisé pour la navigation et les interactions (loot, off‑mesh, ascenseurs, portals, interactions d'unités/objets). Aucune suggestion ou modification n'est fournie ici — uniquement le comportement observé dans HB 6.2.3.

## Vue d'ensemble A→Z

1) Génération de chemin

- HB appelle le pathfinder (MeshNavigator.FindPath / PathFindResult). Le résultat contient au moins : Points[], Flags[], PolyTypes[], AbilityFlags[] (métadonnées de segment/edge).

2) Construction du MeshMovePath et calcul de l'indice de départ

- Après création de MeshMovePath à partir du PathFindResult, HB exécute une procédure pour déterminer l'indice initial (`Index`) à partir des flags du chemin. Extrait exact (décompilé HB 6.2.3) :

```
private void method_14(MeshMovePath meshMovePath_1, Vector3 vector3_0)
{
    meshMovePath_1.Path.Manager.LoadTile(TileIdentifier.GetByPosition(vector3_0));
    Vector3 vector = NavHelper.ToNav(vector3_0);
    PolygonReference polygonReference;
    if (!meshMovePath_1.Path.Manager.MeshQuery.FindNearestPolygon(vector, this.Nav.Extents, this.Nav.QueryFilter.InternalFilter, ref vector, ref polygonReference).Failed && polygonReference.Id != 0U)
    {
        int num = 1;
        while (num < meshMovePath_1.Path.Points.Length && (meshMovePath_1.Path.Flags[num - 1] & 4) == null)
        {
            Vector3 vector2 = NavHelper.ToNav(meshMovePath_1.Path.Points[num]);
            float num2;
            if (this.method_13(meshMovePath_1.Path.Manager, polygonReference, vector, vector2, out num2))
            {
                break;
            }
            num++;
        }
        int num3 = num - 1;
        if (meshMovePath_1.Path.Flags[num3].HasFlag(4) && num3 + 1 < meshMovePath_1.Path.Points.Length)
        {
            AreaType areaType = meshMovePath_1.Path.PolyTypes[num3];
            if (areaType != AreaType.Elevator && areaType != AreaType.Portal && areaType != AreaType.DefendersPortal && areaType != AreaType.HordePortal && areaType != AreaType.AlliancePortal && areaType != AreaType.InteractUnit && areaType != AreaType.InteractObject)
            {
                Vector3 vector3 = meshMovePath_1.Path.Points[num3];
                Vector3 vector4 = meshMovePath_1.Path.Points[num3 + 1];
                Vector3 vector5 = MeshNavigator.smethod_3(vector3_0, vector3, vector4);
                if (this.method_27(vector3_0, vector5))
                {
                    num3++;
                }
            }
        }
        if (num3 >= 2)
        {
            Logging.WriteDiagnostic("Skipped {0} path nodes", new object[] { num3 });
        }
        meshMovePath_1.Index = num3;
        return;
    }
}
```

- Remarques factuelles tirées du code ci‑dessus : HB parcourt les points et consulte `Path.Flags[num - 1]` pour décider jusqu'où « sauter » avant de commencer à suivre la séquence de waypoints; les flags servent donc à détecter des segments particuliers (off‑mesh / interactions) et à positionner l'Index de départ.

3) Utilisation des flags pendant le suivi du chemin

- Lors de la lecture du MeshMovePath, HB utilise explicitement les flags pour dispatcher le traitement des segments spéciaux. Extrait (décompilé HB 6.2.3) :

```
public virtual MoveResult MovePath(MeshMovePath path)
{
    if (((path != null) ? path.Path : null) == null || path.Index < 0 || path.Index > path.Path.Points.Length)
    {
        return MoveResult.Failed;
    }
    this.CurrentHopAbilityFlags = ((path.Index > 0) ? path.Path.AbilityFlags[path.Index - 1] : AbilityFlags.None);
    if (path.Index > 0 && (path.Path.Flags[path.Index - 1] & 4) != null)
    {
        return this.method_18(path);
    }
    return this.method_24(path);
}
```

- Donc HB teste `path.Path.Flags[path.Index - 1]` pour savoir si le segment antérieur nécessite un traitement spécial (ici `& 4` correspond à un flag particulier — le type de flag exact est géré par l'API Tripper/HB). Si le flag est présent, HB appelle la routine dédiée (méthode de dispatch : ascenseur/portal/interaction, etc.).

4) Sémantique des flags

- Les flags proviennent du moteur Tripper/NavMesh (OffMeshConnection, etc.). HB combine ces flags avec `PolyTypes` (AreaType) pour distinguer : ascenseurs, portals, zones d'interaction (InteractUnit / InteractObject), et autres cas nécessitant une logique particulière.
- Extrait montrant le test d'AreaType (cf. code plus haut) : HB exclut certains AreaType (Elevator, Portal, InteractUnit, InteractObject, ...) avant de décider de « sauter » un point.

5) Loot et interactions

- Pour le looting, HB n'utilise pas un mécanisme séparé de flags : le chemin peut contenir des segments marqués comme `InteractObject` / `InteractUnit` (poly type / flag) et HB dispatchera vers la logique d'interaction correspondante.
- Concrètement, les comportements de loot/interaction dans HB attendent explicitement l'événement Lua `LOOT_OPENED` via un nœud `WaitLuaEvent("LOOT_OPENED", ...)`. Exemple extrait décompilé :

```
array7[2] = new WaitLuaEvent("LOOT_OPENED", new WaitGetTimeoutDelegate(LevelBot.Class1375.<>9.method_24), new ActionRunCoroutine(new Func<object, Task>(LevelBot.Class1375.<>9.method_25)));
```

- Ainsi, l'ordre est : pathification (flags), déplacement jusqu'au point/segment approprié, exécution de la routine d'interaction (qui peut invoquer ClickToMove/Interact), puis attente du signal `LOOT_OPENED` (ou timeout défini par HB) avant de poursuivre.

## Ce qui crée les pauses / "latences" observées (description factuelle HB)

- Les pauses observables peuvent provenir, dans HB, de plusieurs éléments **séparés** :
  - dispatch vers des handlers off‑mesh (ascenseurs/portals/interact) qui impliquent des interactions et des conditions d'approche serrées (HB peut exiger une précision très faible pour avancer ou utiliser ClickToMove si hors précision) ;
  - logique ClickToMove / ClickToMoveInfo utilisée pour certaines transitions (HB appelle `WoWMovement.ClickToMove(...)` à plusieurs endroits quand l'implémentation de navigation le demande) ;
  - nœuds `Wait` / `WaitTimer` dispersés dans les arbres de comportements HB, dont les durées sont fixées module par module (pas une valeur globale unique).

- Exemples concrets tirés du code HB 6.2.3 (occurrences réelles dans le décompilé) :
  - `new Wait(5, ...)` apparaît dans certains bots (ex. ArchBuddy) — c'est un nœud TreeSharp Wait avec timeout 5 secondes pour ce flux précis.
  - `new WaitTimer(TimeSpan.FromSeconds(5.0))` ou `new WaitTimer(TimeSpan.FromSeconds(7.0))` apparaissent dans plusieurs composants (par ex. DungeonBuddy contient `waitTimer_15 = new WaitTimer(TimeSpan.FromSeconds(7.0))`, d'autres modules utilisent 5s, 2s, 10s, etc.).

- Conclusion factuelle : les "5s" et "7s" mentionnés sont des timeouts/timers définis explicitement dans différents modules HB (nœuds Wait ou WaitTimer) et varient selon le composant et le contexte — ce ne sont pas une unique constante globale du moteur de navigation.

## Notes finales (strictement descriptif)

- Le système de flags dans HB 6.2.3 sert à marquer des segments « spéciaux » renvoyés par le pathfinder (OffMesh / interactions / etc.), HB lit `Flags[]` + `PolyTypes[]` pour décider du dispatch (handler ascenseur/portail/interaction) et pour calculer l'indice de départ (`Index`) avant de suivre le chemin.
- Les pauses visibles sont une conjonction de : dispatch off‑mesh (interaction/ClickToMove), et nœuds `Wait`/`WaitTimer` propres aux comportements (ex.: LOOT_OPENED wait, Wait(5), WaitTimer 7s) — tout cela est explicitement codé dans HB 6.2.3.

---
Fichier rédigé à partir des sources décompilées HB 6.2.3 trouvées dans `hb decompile` (extraits inclus ci‑dessus). Aucun diagnostic ni proposition de correction n'est fourni ici.

## Définitions d'énumérations (extraits du code)

Les définitions ci‑dessous sont extraites des fichiers source présents dans le dépôt (Tripper/Navigation) et reprennent les valeurs observées dans HB / Tripper.

- StraightPathFlags (Tripper/Navigation/StraightPathFlags.cs)

```
[Flags]
public enum StraightPathFlags : byte
{
    None = 0,
    Start = 1,
    End = 2,
    OffMeshConnection = 4
}
```

- AbilityFlags (Tripper/Navigation/AbilityFlags.cs)

```
[Flags]
public enum AbilityFlags : ushort
{
    None = 0,
    Run = 1,
    RunSafe = 2,
    Swim = 4,
    Jump = 8,
    Unwalkable = 16,
    Teleport = 32,
    Transport = 64,
    Horde = 4096,
    Alliance = 8192,
    All = 65535
}
```

- AreaType (Tripper/Navigation/AreaType.cs) — extraits pertinents

```
public enum AreaType : byte
{
    Ground = 1,
    Water = 2,
    Lava = 3,
    Road = 4,
    Fall = 5,
    Elevator = 6,
    Gate = 7,
    Portal = 8,
    DefendersPortal = 9,
    HordePortal = 10,
    AlliancePortal = 11,
    Blocked = 12,
    InteractUnit = 13,
    InteractObject = 14,
    Horde = 15,
    Alliance = 16,
    Blackspot = 17
}
```

## Où ces flags apparaissent dans le code

- Résultat du pathfinder : `PathFindResult` contient `Points[]`, `Flags[]` (`StraightPathFlags[]`), `AbilityFlags[]` et `PolyTypes[]` (`AreaType[]`). Voir `Tripper/Navigation/PathFindResult.cs`.
- Post‑processing : `Tripper/Navigation/PathPostProcessor.cs` respecte `StraightPathFlags.OffMeshConnection` et évite de déplacer les points off‑mesh.
- Navigator / QueryFilter : `Tripper/Navigation/Navigator.cs` initialise des `QueryFilter` utilisant `AbilityFlags` et appelle `NativeMethods.SetAreaCost` pour `AreaType`.
- Marquage de zones : `BlackspotManager` et `Navigator.SetPolyArea`/`SetPolyFlags` manipulent `AreaType.Blackspot` et les flags de polygone via l'API native (`Tripper/Navigation/NativeMethods.cs`).

## Résumé factuel

- `StraightPathFlags` identifie des points simples (Start/End) et des points spéciaux `OffMeshConnection` (valeur 4) qui correspondent aux connexions hors‑maille (ascenseurs, portals, liens off‑mesh). HB et Tripper testent ce flag par opérations bitwise pour dispatcher un handler d'interaction.
- `AbilityFlags` dit quelles capacités sont nécessaires pour suivre un segment (nager, sauter, transport, faction). Ces flags sont utilisés par les filtres de requête (`QueryFilter`) qui influencent le pathfinder.
- `AreaType` décrit la nature du polygone et permet d'appliquer des coûts (`SetAreaCost`) ou des comportements (ex.: `InteractObject`, `InteractUnit`, `Blackspot`).

---
Texte construit uniquement à partir des fichiers sources présents dans le dépôt (Tripper et extraits HB fournis). Aucun commentaire d'opinion n'a été ajouté.