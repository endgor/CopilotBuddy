using System;
using System.Collections.Generic;
using System.Linq;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals.WoWObjects;

namespace Styx.WoWInternals
{
    // HB 6.2.3 port — WoWVehicle wraps a WoWUnit that represents a vehicle.
    // In WotLK 3.3.5a vehicles ARE WoWUnits in the ObjectManager (not NativeObjects).
    // Methods that require native engine calls (GetVirtualSeatCount, FindPassengerUnit)
    // or DBC data (VehicleRecord, SpellMissile) are stubbed as they are WoD-only.
    public class WoWVehicle
    {
        private readonly WoWUnit _unit;

        public WoWVehicle(WoWUnit vehicleUnit)
        {
            _unit = vehicleUnit ?? throw new ArgumentNullException(nameof(vehicleUnit));
        }

        // HB 6.2.3: Returns all WoWUnits currently riding this vehicle.
        // In WotLK, passengers have their MovementInfo.TransportGuid set to the vehicle GUID.
        public IEnumerable<WoWUnit> GetPassengers()
        {
            ulong vehicleGuid = _unit.Guid;
            return ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                .Where(u => u.IsOnTransport && u.WoWMovementInfo.TransportGuid == vehicleGuid);
        }

        // HB 6.2.3 stub: requires WoD native engine call — not available in WotLK CB.
        public int GetVirtualSeatCount() => 0;

        // HB 6.2.3 stub: requires WoD native engine call — not available in WotLK CB.
        public WoWUnit? FindPassengerUnit(int seatIndex) => null;

        // HB 6.2.3: Vehicle DBC record — no DBC.Vehicle implementation in CB.
        public object? VehicleRecord => null;

        // HB 6.2.3: Vehicle facing in radians, read directly from the underlying WoWUnit.
        public float Rotation => _unit.Rotation;

        // HB 6.2.3: CanRotateToAngle — checks DBC turn constraints (FacingLimitRight/Left).
        // No VehicleRecord DBC available in CB → always allow rotation.
        public bool CanRotateToAngle(float angleInRadians) => true;

        // HB 6.2.3: CanRotateTowardsLocation — direction-to-target angle check.
        // No VehicleRecord DBC turn constraints in CB → always allow.
        public bool CanRotateTowardsLocation(WoWPoint targetLocation) => true;

        // HB 6.2.3: CanProjectileReachLocation — requires SpellMissile DBC data
        // (DefaultPitchMin/Max, DefaultSpeedMin/Max, Gravity). Not available in CB.
        public bool CanProjectileReachLocation(WoWPoint location, WoWSpell projectileSpell, WoWPoint? projectileStart = null) => false;

        // Convenience accessor for the underlying unit.
        public WoWUnit Unit => _unit;

        // Convenience: position of the vehicle unit.
        public WoWPoint Location => _unit.Location;

        public override string ToString()
        {
            return string.Format("[WoWVehicle: {0} ({1})]", _unit.Name, _unit.Guid);
        }
    }
}
