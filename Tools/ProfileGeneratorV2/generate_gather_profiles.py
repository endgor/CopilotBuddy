#!/usr/bin/env python3
"""
Batch generator for GatherBuddy2 gathering profiles.
Generates profiles from all Zygor profession guides (Herbalism & Mining).

Usage:
  python generate_gather_profiles.py
  python generate_gather_profiles.py --profession Mining
  python generate_gather_profiles.py --type Farming
  python generate_gather_profiles.py --list
"""

import subprocess
import sys
from pathlib import Path

# Paths
ZYGOR_BASE = Path(r"c:\Users\Texy\Desktop\ZygorGuidesViewerClassicTBC\Guides-WOTLK\Professions")
OUTPUT_BASE = Path(__file__).parent / "output_gather"
PARSER_SCRIPT = Path(__file__).parent / "zygor_gather_parser.py"
SPAWNS_JSON = Path(__file__).parent / "gameobject_spawns.json"

# Guide files with faction mapping
# Common file has faction-neutral farming routes (Outland, Northrend)
# Horde/Alliance files have faction-specific leveling + classic farming routes
GUIDES = [
    ("ZygorProfessionsHordeCLASSIC.lua",   "Horde"),
    ("ZygorProfessionsAllianceCLASSIC.lua", "Alliance"),
    ("ZygorProfessionsCommonCLASSIC.lua",   "Neutral"),
]


def main():
    import argparse
    
    ap = argparse.ArgumentParser(description="Batch generate GatherBuddy2 profiles")
    ap.add_argument("--profession", choices=["Herbalism", "Mining"], default=None,
                    help="Filter to one profession")
    ap.add_argument("--type", choices=["Farming", "Leveling"], default=None,
                    help="Filter to guide type")
    ap.add_argument("--list", action="store_true", help="List only, don't generate")
    ap.add_argument("--faction", choices=["Horde", "Alliance", "Neutral"], default=None,
                    help="Process only one faction")
    args = ap.parse_args()
    
    OUTPUT_BASE.mkdir(parents=True, exist_ok=True)
    
    total = 0
    success = 0
    
    for guide_file, faction in GUIDES:
        # Filter by faction
        if args.faction and faction != args.faction:
            continue
        
        zygor_path = ZYGOR_BASE / guide_file
        if not zygor_path.exists():
            print(f"[SKIP] {guide_file} not found")
            continue
        
        # Create faction-specific output directory
        faction_dir = OUTPUT_BASE / faction
        faction_dir.mkdir(parents=True, exist_ok=True)
        
        cmd = [
            sys.executable,
            str(PARSER_SCRIPT),
            str(zygor_path),
            "-o", str(faction_dir),
            "--faction", faction,
        ]
        
        if SPAWNS_JSON.exists():
            cmd.extend(["--spawns", str(SPAWNS_JSON)])
        
        if args.profession:
            cmd.extend(["--profession", args.profession])
        
        if args.type:
            cmd.extend(["--type", args.type])
        
        if args.list:
            cmd.append("--list")
        
        print(f"\n{'='*60}")
        print(f"Processing: {guide_file} ({faction})")
        print(f"{'='*60}")
        
        result = subprocess.run(cmd, capture_output=False)
        total += 1
        if result.returncode == 0:
            success += 1
    
    print(f"\n{'='*60}")
    print(f"SUMMARY: {success}/{total} guide files processed")
    print(f"Output directory: {OUTPUT_BASE}")
    print(f"{'='*60}")


if __name__ == "__main__":
    main()
