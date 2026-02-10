# WoWInternals API Surface Audit

**Scope:** `Styx.WoWInternals` — ObjectManager.cs, Lua.cs, WoWMovement.cs, LuaEvents.cs  
**Codebases compared:** HB 4.3.4 (API source), HB 5.4.8, HB 6.2.3, CopilotBuddy (3.3.5a target)

---

## Architecture Summary

| Aspect | HB 4.3.4 | HB 5.4.8 | HB 6.2.3 | CopilotBuddy |
|--------|-----------|-----------|-----------|--------------|
| Memory lib | BlueMagic | GreyMagic | GreyMagic | GreenMagic |
| Address type | `uint` | `IntPtr` | `IntPtr` | `uint` |
| GUID type | `ulong` | `ulong` | `WoWGuid` (struct) | `ulong` |
| Executor | `ExecutorRand` | `Executor` | `Executor` | `ExecutorRand` |
| Offset source | Cata (4.3.4) | MoP (5.4.8) | WoD (6.2.3) | WotLK (3.3.5a 12340) |

---

## 1. ObjectManager.cs

### Public / Protected API Surface

| Member | HB 4.3.4 | HB 5.4.8 | HB 6.2.3 | CopilotBuddy | Status |
|--------|-----------|-----------|-----------|--------------|--------|
| `event OnObjectListUpdateFinished` | `EventHandler` | `EventHandler` | `EventHandler` [Obsolete] | `EventHandler` | **OK** |
| `WoWProcess` {get;private set} | `Process` | — | — | `Process` | OK (matches 4.3.4) |
| `Wow` (Memory) | `BlueMagic.Memory` | — (use `StyxWoW.Memory`) | — | `GreenMagic.Memory` | OK (matches 4.3.4) |
| `Executor` | `ExecutorRand` | — | — | `ExecutorRand` | OK (matches 4.3.4) |
| `Me` | `public LocalPlayer` | `internal LocalPlayer` | `internal LocalPlayer` | `public LocalPlayer` | OK (matches 4.3.4) |
| `IsInitialized` | `internal bool` | `internal bool` | `internal bool` | `public bool` | **WIDENED** — public vs internal |
| `IsInGame` | `public bool` | `public bool` | `public bool` | `public bool` | OK |
| `LocalGuid` | `ulong` | `IntPtr` | `WoWGuid` | `ulong` | OK (matches 4.3.4) |
| `ObjectList` | `List<WoWObject>` | `List<WoWObject>` | `List<WoWObject>` | `List<WoWObject>` | OK |
| `PerformanceCounter` | — | — | — | `uint` (public) | **CB-ONLY** — added for aura timing |
| `HookEndscene()` → `bool` | public | — | — | public | OK (matches 4.3.4) |
| `Initialize(Memory)` | — | — | — | public | **CB-ONLY** — explicit init |
| `Update()` | public | internal | internal | public | OK (matches 4.3.4) |
| `GetObjectsOfType<T>()` | 3 overloads | 1 (optional params) | 1 (optional params) | 3 overloads | OK (matches 4.3.4) |
| `GetObjectsOfTypeFast<T>()` | — | public | public | — | **MISSING** — added in 5.4.8 |
| `GetObjectByGuid<T>(ulong)` | public | public (IntPtr) | public (WoWGuid) | public (ulong) | OK (matches 4.3.4) |
| `GetAnyObjectByGuid<T>(ulong)` | public | public [Obsolete] | — | — | **MISSING** — exists in 4.3.4 |
| `InternalGetObjectByGuid(ulong)` | internal | internal | internal | `GetObjectInternal(ulong)` | OK (renamed) |

### Key Differences

- **`GetObjectsOfTypeFast<T>()`**: New in 5.4.8. Returns objects faster by skipping some checks. Not in 4.3.4 or CB. Low priority since 4.3.4 doesn't have it.
- **`GetAnyObjectByGuid<T>(ulong)`**: Present in 4.3.4, missing in CB. Returns object even if not fully valid. Consider adding as stub.
- **`PerformanceCounter`**: CB-only addition (not in any HB). Used for aura timing optimization.
- **`ActiveMoverGuid`**: Not exposed in ObjectManager in any version (it's on WoWMovement).

---

## 2. Lua.cs

### Public / Protected API Surface

| Member | HB 4.3.4 | HB 5.4.8 | HB 6.2.3 | CopilotBuddy | Status |
|--------|-----------|-----------|-----------|--------------|--------|
| `Escape(string)` → `string` | public static | public static | public static | public static | OK |
| `ShowLuaStack(uint)` | public static | public static | public static | public static | OK |
| `GetReturnValues(string)` → `List<string>` | public static | public static | public static | public static | OK |
| `GetReturnValues(string, string)` → `List<string>` | public static | public static | public static | public static | OK |
| `LuaGetReturnValue(string)` [Obsolete] | public static | public static | public static | public static | OK |
| `GetReturnVal<T>(string, uint)` | `uint` param | `uint` param | `uint` param | `uint` + `int` overloads | OK (extra overload) |
| `DoString(string)` | public static | public static | public static | public static | OK |
| `DoString(string, string)` | public static | public static | public static | public static | OK |
| `DoString(string, params object[])` | public static | public static | public static | public static | OK |
| `DoString(string, string, uint)` | public static | — | — | — | Missing from CB **and** from 5.4.8+ (obsolete pattern) |
| `ParseLuaValue<T>(string)` | — | public static | public static | — | **MISSING** — added in 5.4.8 |
| `GetLocalizedText<T>(string)` | [Obsolete] | [Obsolete] | [Obsolete] | **NOT obsolete** | **DIFFERENCE** — should mark [Obsolete] |
| `GetLocalizedText(string)` | [Obsolete] | [Obsolete] | [Obsolete] | **NOT obsolete** | **DIFFERENCE** |
| `GetLocalizedText(string, uint)` | [Obsolete] | [Obsolete] | [Obsolete] | **NOT obsolete** | **DIFFERENCE** |
| `GetLocalizedInt32` | [Obsolete] | [Obsolete] | [Obsolete] | **NOT obsolete** | **DIFFERENCE** — functional though |
| `GetLocalizedUInt32` | [Obsolete] | [Obsolete] | [Obsolete] | **NOT obsolete** | **DIFFERENCE** |
| `GetLocalizedInt64` | [Obsolete] | [Obsolete] | [Obsolete] | **NOT obsolete** | **DIFFERENCE** |
| `GetLocalizedUInt64` | [Obsolete] | [Obsolete] | [Obsolete] | **NOT obsolete** | **DIFFERENCE** |
| `GetLocalizedBool` | [Obsolete] | [Obsolete] | [Obsolete] | **NOT obsolete** | **DIFFERENCE** |
| `State` → `LuaState` | public static | public static | public static | public static | OK |
| `Events` → `LuaEvents` | public static | public static | public static | public static | OK |
| `GetTop(uint)` | internal | internal | internal | **public** | **WIDENED** — CB exposes as public |
| `ToLString(uint, int, int)` | internal | internal | internal | **public** | **WIDENED** — CB exposes as public |

### CB-Only Additions

| Member | Description |
|--------|-------------|
| `LuaState.Globals` property | Returns `LuaTable` for Lua global variables. Not in any HB LuaState. |
| `GetReturnVal<T>(string, int)` overload | Extra overload accepting `int` index. HB only has `uint`. |

### Key Differences

- **`ParseLuaValue<T>(string)`**: Added in 5.4.8, parses Lua string to typed value. Not in 4.3.4 or CB. Useful utility — consider adding.
- **`GetLocalizedText` family**: Functional in CB but should be marked `[Obsolete]` per HB 4.3.4 to match API.
- **`GetTop` / `ToLString`**: Internal in all HB versions, public in CB. Not a problem functionally but deviates from HB API surface.
- **Default script name**: HB uses `"hax.lua"` (4.3.4) or `"WoW.lua"` (5.4.8+). CB uses `"CopilotBuddy.lua"` / `"CopilotBuddy"`.

---

## 3. WoWMovement.cs

### Public / Protected API Surface

| Member | HB 4.3.4 | HB 5.4.8 | HB 6.2.3 | CopilotBuddy | Status |
|--------|-----------|-----------|-----------|--------------|--------|
| `Pulse()` | public static | public static | public static | — | **MISSING** — timed movement queue processor |
| `ClickToMoveInfo` → `ClickToMoveInfoStruct` | public static | public static | public static (separate class) | public static | OK |
| `ActiveMoverGuid` | `ulong` | `ulong` | `WoWGuid` | — (not present) | **MISSING** |
| `ActiveMover` → `WoWUnit` | public static | public static | public static | **returns `WoWPoint`** | **WRONG RETURN TYPE** |
| `IsFacing` → `bool` | public static | public static | public static | public static | OK |
| `ActiveInputControl` → `InputControl` | public static | public static | public static | public static | OK |
| `IsMoving` → `bool` | — | — | — | public static | **CB-ONLY** |
| `CalculatePointFrom(WoWPoint, float)` | public static | public static | public static | public static | OK |
| `GetHeadingDiff(float, float)` | public static | public static | — | public static | OK |
| `Face(ulong)` | public static | public static | `Face(WoWGuid)` | public static (ulong) | OK |
| `Face()` (current target) | public static | public static | public static | — | **MISSING** — no-arg Face |
| `Face(WoWPoint)` | — | — | — | public static | **CB-ONLY** |
| `Face(WoWUnit)` | — | — | — | public static | **CB-ONLY** |
| `StopFace()` | public static | public static | public static | public static | OK |
| `ConstantFace(ulong)` | public static | public static | `ConstantFace(WoWGuid)` | public static (ulong) | OK |
| `ConstantFace(float)` | — | — | — | public static | **CB-ONLY** |
| `ConstantFaceStop(ulong)` | public static | public static | `ConstantFaceStop(WoWGuid)` | — | **MISSING** — only `ConstantFaceStop()` (no arg) |
| `ConstantFaceStop()` | — | — | — | public static | CB-ONLY (delegates to `StopFace`) |
| `ClickToMove(...)` overloads | 6 overloads | 6+ overloads | uses `WoWGuid` | multiple overloads | OK (adapted for 3.3.5a) |
| `ClickToMoveStop()` | — | via smethod_5 (internal) | via smethod_5 | public static | **CB-ONLY** as public |
| `Move(MovementDirection)` | public static | public static | public static | public static | OK |
| `Move(MovementDirection, bool)` | — | — | — | public static | **CB-ONLY** — Lua-based |
| `Move(MovementDirection, TimeSpan)` | public static | public static | public static | public static | OK |
| `MoveStop()` | public static | public static | public static | public static | OK |
| `MoveStop(MovementDirection)` | public static | public static | public static | public static | OK |
| `StopMovement(MovementDirection)` | — | — | — | public static | **CB-ONLY** alias of MoveStop |
| `Jump()` | — | — | — | public static | **CB-ONLY** |
| `Ascend()` | — | — | — | public static | **CB-ONLY** |
| `Descend()` | — | — | — | public static | **CB-ONLY** |
| `DescendStop()` | — | — | — | public static | **CB-ONLY** |
| `Navigate(WoWPoint)` | public static | — | — | public static | OK (matches 4.3.4) |
| `Navigate(WoWPoint, float)` | public static | — | — | public static | OK |
| `OnMovementFlagsChanged` event | — | internal | internal | — | N/A (internal only) |

### Enums and Structs

| Type | HB 4.3.4 | HB 5.4.8 | HB 6.2.3 | CopilotBuddy | Status |
|------|-----------|-----------|-----------|--------------|--------|
| `ClickToMoveType` enum | 14 values | identical | identical | **different values** | **DIFFERENCE** — CB simplified |
| `MovementDirection` [Flags] | full (26 values) | identical | identical | **simplified (8 values)** | **DIFFERENCE** — CB missing many flags |
| `ClickToMoveInfoStruct` | full layout | identical | identical | adapted for 3.3.5a | OK |
| `InputControl` struct | full (Flags, Movement) | identical | identical | **simplified (Time + MovementControl)** | **DIFFERENCE** — missing Flags field |
| `MovementControl` struct | full (private) | identical | identical | simplified | OK |
| `MovementEventArgs` class | — | public | public | — | N/A (5.4.8+) |

### Critical Issues

1. **`ActiveMover` returns `WoWPoint` instead of `WoWUnit`** — This is a significant API mismatch. All HB versions return `WoWUnit`. Callers expecting a unit object (e.g., `.Guid`, `.IsValid`, `.Transport`) will fail.

2. **`ActiveMoverGuid` missing** — Needed by ClickToMove internals and Face methods in HB. CB works around this by getting player base address directly.

3. **`Pulse()` missing** — Processes timed movement queue (`list_0`). Without it, `Move(direction, timespan)` adds entries but they won't be cleaned up automatically. CB's Lua-based `Move` with `Thread.Sleep` works differently.

4. **`MovementDirection` enum simplified** — CB has only 8 flags (Forward=1, Backwards=2, etc.) vs HB's 26 flags (Forward=16, Backwards=32, etc.). The **numeric values are different**. This will cause bugs if any code uses the raw values.

5. **`Face()` no-arg** — Faces current target. Missing in CB; simple to add.

6. **Movement implementation difference** — CB uses Lua.DoString for movement commands (MoveForwardStart, etc.) while HB uses ASM injection via `smethod_3` (calling game functions directly). CB approach is simpler but potentially less reliable.

---

## 4. LuaEvents.cs

### Public / Protected API Surface

| Member | HB 4.3.4 | HB 5.4.8 | HB 6.2.3 | CopilotBuddy | Status |
|--------|-----------|-----------|-----------|--------------|--------|
| `AttachEvent(string, handler)` | public | public | public | public | OK |
| `AttachEvent(string, handler, filter)` | — | — | public (6.2.3) | — | N/A (6.2.3 only) |
| `DetachEvent(string, handler)` | public | public | public | public | OK |
| `AddFilter(string, string)` → `bool` | public | public | — (inline in AttachEvent) | public | OK (matches 4.3.4) |
| `RemoveFilter(string)` | public | public | — | public | OK |
| `IsInitialized` → `bool` | public | public | public | — | **MISSING** — not exposed |
| `PrintAllEvents` → `bool` | public static | public static | public static | public static (renamed logic) | OK (functional) |
| `ProcessEvents()` | internal (`method_1`) | internal (`method_3`) | internal (`method_7`) | **public** (`ProcessEvents`) | **WIDENED** |
| `ProcessPendingEvents()` | — | — | — | public static | **CB-ONLY** |

### Key Differences

1. **Event processing stride**: HB 4.3.4/5.4.8 use 3-element stride (event, time, args). HB 6.2.3 uses 4-element stride (event, time, filterMask, args). CB uses 3-element stride matching 4.3.4.

2. **Filter implementation**: HB 4.3.4 sends filter code to Lua side (`{filterTable}["event"] = function(args) ... end`). CB implements filters in C# only (WoW 3.3.5a compatibility). This is a valid adaptation.

3. **Initialization**: HB 4.3.4 uses 3 random strings (eventTable, filterTable, frameName). HB 5.4.8 adds a 4th (botRunning flag). HB 6.2.3 uses 10+ random strings. CB uses 2 (eventTable, frameName — no filter table in Lua).

4. **`IsInitialized`**: Public property in all HB versions, not exposed in CB. It checks if the Lua-side event table exists.

5. **Bot lifecycle**: HB 5.4.8+ hooks `BotEvents.OnBotStarted`/`OnBotStopped` to wipe/pause event collection. CB lacks this integration.

6. **Thread safety**: HB 5.4.8+ locks on `StyxWoW.Memory.Executor.AssemblyLock` in AttachEvent/DetachEvent/ProcessEvents. CB does not lock.

7. **Cleanup**: HB 4.3.4 uses `~LuaEvents()` finalizer to clean up Lua frame. CB also uses finalizer but has null-safety improvements.

---

## Summary: Missing from CopilotBuddy

### High Priority (used by consumers / API mismatch)

| Missing Item | Source | Impact |
|-------------|--------|--------|
| `WoWMovement.ActiveMover` returns `WoWUnit` | All HB | **Critical** — wrong return type |
| `WoWMovement.ActiveMoverGuid` → `ulong` | 4.3.4 | Needed for Face/CTM internals |
| `WoWMovement.Pulse()` | 4.3.4 | Timed movement queue not processed |
| `WoWMovement.MovementDirection` values | 4.3.4 | Enum values mismatch (1,2,4... vs 16,32,64...) |
| `WoWMovement.Face()` (no-arg, faces current target) | 4.3.4 | Missing convenience method |
| `WoWMovement.ConstantFaceStop(ulong)` | 4.3.4 | Missing — CB only has no-arg version |
| `LuaEvents.IsInitialized` property | 4.3.4 | Not exposed (easy add) |

### Medium Priority (useful utilities)

| Missing Item | Source | Impact |
|-------------|--------|--------|
| `Lua.ParseLuaValue<T>(string)` | 5.4.8 | Useful utility for parsing Lua results |
| `ObjectManager.GetAnyObjectByGuid<T>(ulong)` | 4.3.4 | Returns objects even if not fully valid |
| `WoWMovement.InputControl.Flags` field | 4.3.4 | CB struct missing MovementDirection Flags |
| `GetLocalizedText` family [Obsolete] marking | 4.3.4 | Should be marked [Obsolete] per HB |
| LuaEvents thread safety (locking) | 5.4.8 | Prevents race conditions in event handlers |
| LuaEvents bot lifecycle hooks | 5.4.8 | Wipe events on bot start/stop |

### Low Priority (5.4.8+ additions not in 4.3.4)

| Missing Item | Source | Why Low |
|-------------|--------|---------|
| `ObjectManager.GetObjectsOfTypeFast<T>()` | 5.4.8 | Not in 4.3.4 (API source) |
| `LuaEvents.AttachEvent` with inline filter | 6.2.3 | 6.2.3 only |
| `WoWMovement.OnMovementFlagsChanged` event | 5.4.8 | Internal; no consumer impact |
| `WoWMovement.MovementEventArgs` class | 5.4.8 | Only used by internal event |

### CB-Only Additions (not in any HB)

| Addition | Notes |
|----------|-------|
| `ObjectManager.PerformanceCounter` (uint) | Aura timing optimization |
| `ObjectManager.Initialize(Memory)` | Explicit initialization |
| `WoWMovement.IsMoving` (bool) | Convenience property |
| `WoWMovement.Face(WoWPoint)` | Face a world point |
| `WoWMovement.Face(WoWUnit)` | Face a unit |
| `WoWMovement.ConstantFace(float)` | Face a raw angle |
| `WoWMovement.Jump()`, `Ascend()`, `Descend()`, `DescendStop()` | Lua-based convenience methods |
| `WoWMovement.Move(dir, bool)` overload | Start/stop with bool |
| `WoWMovement.ClickToMoveStop()` | Public CTM stop |
| `WoWMovement.Navigate()` overloads | Delegates to Navigator |
| `WoWMovement.CalculatePointFrom()` | Uses WoWMathHelper |
| `WoWMovement.GetHeadingDiff()` | Heading math |
| `Lua.GetTop(uint)` (public) | Internal in HB |
| `Lua.ToLString(uint, int, int)` (public) | Internal in HB |
| `Lua.LuaState.Globals` | Direct access to Lua global table |
| `LuaEvents.ProcessPendingEvents()` | Static convenience |
| `LuaEvents.ApplyFilter()` (C#-side) | 3.3.5a adaptation |

---

## Recommendations

1. **Fix `WoWMovement.ActiveMover`** — Must return `WoWUnit`, not `WoWPoint`. This is the highest priority fix.
2. **Add `ActiveMoverGuid`** — Return `ulong` of the player controlling movement.
3. **Fix `MovementDirection` enum values** — Align with HB 4.3.4 values (Forward=16, not 1).
4. **Add `Pulse()`** — Process timed movement entries from `Move(dir, TimeSpan)`.
5. **Add `Face()` no-arg** — Face current target.
6. **Add `ConstantFaceStop(ulong)`** — Stop facing a specific GUID.
7. **Add `LuaEvents.IsInitialized`** — Expose the existing check.
8. **Mark `GetLocalizedText` family `[Obsolete]`** — Match HB 4.3.4 API.
9. **Consider `ParseLuaValue<T>`** — Useful for typed Lua result parsing.
10. **Add event processing locks** — Thread safety for LuaEvents.
