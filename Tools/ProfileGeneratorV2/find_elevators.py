#!/usr/bin/env python3
"""
Find all elevator/transport GameObjects in WoW 3.3.5a database.
Queries the local SPP WotLK MariaDB for GAMEOBJECT_TYPE_TRANSPORT (11)
and GAMEOBJECT_TYPE_MO_TRANSPORT (15) to find elevators that may need
off-mesh connections in mmaps.

Usage:
    python find_elevators.py [--host 127.0.0.1] [--port 3310]
"""

import sys
import argparse

try:
    import mariadb
except ImportError:
    import subprocess
    subprocess.check_call([sys.executable, "-m", "pip", "install", "mariadb"])
    import mariadb


def main():
    parser = argparse.ArgumentParser(description='Find WoW elevators/transports')
    parser.add_argument('--host', default='127.0.0.1')
    parser.add_argument('--port', type=int, default=3310)
    parser.add_argument('--user', default='root')
    parser.add_argument('--password', default='123456')
    parser.add_argument('--database', default='world')
    args = parser.parse_args()

    print(f"Connecting to {args.host}:{args.port}/{args.database}...")
    conn = mariadb.connect(
        host=args.host, port=args.port, 
        user=args.user, password=args.password, 
        database=args.database
    )
    cursor = conn.cursor()

    # =========================================================================
    # 1. Find GAMEOBJECT_TYPE_TRANSPORT (type=11) - static transports/elevators
    # =========================================================================
    print("\n=== GAMEOBJECT_TYPE_TRANSPORT (type=11) - Elevators ===\n")
    
    # Get templates
    cursor.execute("""
        SELECT gt.entry, gt.name, gt.displayId, gt.data0, gt.data1, gt.data2, gt.data3, gt.data4
        FROM gameobject_template gt
        WHERE gt.type = 11
        ORDER BY gt.entry
    """)
    templates_11 = cursor.fetchall()
    
    print(f"Found {len(templates_11)} transport templates (type=11)")
    print("-" * 120)
    print(f"{'Entry':>8}  {'Name':40s}  {'DisplayId':>9}  {'data0':>8}  {'data1':>8}  {'data2':>8}  {'data3':>8}  {'data4':>8}")
    print("-" * 120)
    
    for row in templates_11:
        entry, name, displayId, d0, d1, d2, d3, d4 = row
        print(f"{entry:>8}  {name:40s}  {displayId:>9}  {d0:>8}  {d1:>8}  {d2:>8}  {d3:>8}  {d4:>8}")

    # Get spawns for type 11
    print(f"\n\n=== Spawns of type=11 GameObjects ===\n")
    
    type11_entries = [r[0] for r in templates_11]
    if type11_entries:
        ids_str = ','.join(str(e) for e in type11_entries)
        cursor.execute(f"""
            SELECT g.guid, g.id, gt.name, g.map, g.position_x, g.position_y, g.position_z
            FROM gameobject g
            JOIN gameobject_template gt ON g.id = gt.entry
            WHERE g.id IN ({ids_str})
            ORDER BY g.map, gt.name, g.guid
        """)
        spawns_11 = cursor.fetchall()
        
        print(f"Found {len(spawns_11)} spawned transport objects")
        print("-" * 130)
        print(f"{'GUID':>8}  {'Entry':>8}  {'Name':40s}  {'Map':>5}  {'X':>12}  {'Y':>12}  {'Z':>10}")
        print("-" * 130)
        
        for guid, entry, name, map_id, x, y, z in spawns_11:
            print(f"{guid:>8}  {entry:>8}  {name:40s}  {map_id:>5}  {x:>12.4f}  {y:>12.4f}  {z:>10.4f}")

    # =========================================================================
    # 2. Find GAMEOBJECT_TYPE_MO_TRANSPORT (type=15) - moving transports
    # =========================================================================
    print(f"\n\n=== GAMEOBJECT_TYPE_MO_TRANSPORT (type=15) - Ships/Zeppelins ===\n")
    
    cursor.execute("""
        SELECT gt.entry, gt.name, gt.displayId, gt.data0, gt.data6
        FROM gameobject_template gt
        WHERE gt.type = 15
        ORDER BY gt.entry
    """)
    templates_15 = cursor.fetchall()
    
    print(f"Found {len(templates_15)} MO_TRANSPORT templates (type=15)")
    print("-" * 100)
    print(f"{'Entry':>8}  {'Name':50s}  {'DisplayId':>9}  {'TaxiPathId':>10}  {'MapId':>6}")
    print("-" * 100)
    
    for entry, name, displayId, taxiPathId, mapId in templates_15:
        print(f"{entry:>8}  {name:50s}  {displayId:>9}  {taxiPathId:>10}  {mapId:>6}")

    # =========================================================================
    # 3. Search for elevator-related names in all GO types
    # =========================================================================
    print(f"\n\n=== GameObjects with 'elevator' or 'lift' in name ===\n")
    
    cursor.execute("""
        SELECT gt.entry, gt.name, gt.type, gt.displayId, 
               g.map, g.position_x, g.position_y, g.position_z
        FROM gameobject_template gt
        LEFT JOIN gameobject g ON gt.entry = g.id
        WHERE gt.name LIKE '%elevator%' 
           OR gt.name LIKE '%lift%'
           OR gt.name LIKE '%Elevator%'
           OR gt.name LIKE '%Lift%'
        ORDER BY g.map, gt.name
    """)
    elevator_rows = cursor.fetchall()
    
    print(f"Found {len(elevator_rows)} entries")
    print("-" * 130)
    print(f"{'Entry':>8}  {'Name':40s}  {'Type':>4}  {'DisplayId':>9}  {'Map':>5}  {'X':>12}  {'Y':>12}  {'Z':>10}")
    print("-" * 130)
    
    for entry, name, go_type, displayId, map_id, x, y, z in elevator_rows:
        mx = f"{x:>12.4f}" if x is not None else "      N/A   "
        my = f"{y:>12.4f}" if y is not None else "      N/A   "
        mz = f"{z:>10.4f}" if z is not None else "    N/A   "
        mm = f"{map_id:>5}" if map_id is not None else "  N/A"
        print(f"{entry:>8}  {name:40s}  {go_type:>4}  {displayId:>9}  {mm}  {mx}  {my}  {mz}")

    # =========================================================================
    # 4. Interesting transport displayIds (from vmangos source)
    # =========================================================================
    print(f"\n\n=== Known elevator DisplayIds from WoW source ===\n")
    
    known_displays = {
        360: "Elevatorcar.m2",
        455: "Undeadelevator.m2",
        561: "Ironforgeelevator.m2", 
        807: "Gnomeelevatorcar01.m2",
        808: "Gnomeelevatorcar02.m2",
        827: "Gnomeelevatorcar03.m2",
        852: "Gnomeelevatorcar03.m2 (alt)",
        1587: "Gnomehutelevator.m2",
        2454: "Burningsteppselevator.m2",
        3831: "Subwaycar.m2 (Deeprun Tram)"
    }
    
    display_ids = ','.join(str(d) for d in known_displays.keys())
    cursor.execute(f"""
        SELECT g.guid, g.id, gt.name, gt.displayId, g.map, 
               g.position_x, g.position_y, g.position_z
        FROM gameobject g
        JOIN gameobject_template gt ON g.id = gt.entry
        WHERE gt.displayId IN ({display_ids})
        ORDER BY g.map, gt.displayId, g.guid
    """)
    known_rows = cursor.fetchall()
    
    print(f"Found {len(known_rows)} spawns with known elevator models")
    print("-" * 140)
    print(f"{'GUID':>8}  {'Entry':>8}  {'Name':40s}  {'Model':30s}  {'Map':>5}  {'X':>12}  {'Y':>12}  {'Z':>10}")
    print("-" * 140)
    
    for guid, entry, name, displayId, map_id, x, y, z in known_rows:
        model = known_displays.get(displayId, "?")
        print(f"{guid:>8}  {entry:>8}  {name:40s}  {model:30s}  {map_id:>5}  {x:>12.4f}  {y:>12.4f}  {z:>10.4f}")

    cursor.close()
    conn.close()
    print("\nDone!")


if __name__ == '__main__':
    main()
