#!/usr/bin/env python3
"""
Zygor Profession Guides → GatherBuddy2 Profile Converter

Parses Zygor's Herbalism/Mining profession guides and outputs GatherBuddy2-compatible
HBProfile XML files with <Hotspots> for each farming route.

Sources:
  - Zygor Profession Guides (Horde/Alliance/Common): path-based farming loops
  - gameobject_spawns.json (SPP DB): world coords for herb/ore game objects
  - Zone map boundaries: for converting Zygor % coords → WoW world coords

Output: HBProfile XML files with Hotspots only (no Blackspots — added manually)
"""

import re
import os
import sys
import json
import math
from dataclasses import dataclass, field
from typing import List, Optional, Dict, Tuple
from pathlib import Path
import xml.etree.ElementTree as ET
from xml.dom.minidom import parseString


# ============================================================================
# Data Classes
# ============================================================================

@dataclass
class Hotspot:
    x: float
    y: float
    z: float = 0.0


@dataclass
class GatherRoute:
    """A single gathering farming route parsed from Zygor"""
    name: str                          # e.g. "Copper Ore", "Herbalism 1-70"
    profession: str                    # "Herbalism" or "Mining"
    guide_type: str                    # "Farming" or "Leveling" 
    zone: str                          # e.g. "Durotar", "The Barrens"
    map_id: int = 0                    # WoW MapId
    min_skill: int = 0                 # Minimum skill level required
    max_skill: int = 0                 # Target skill level
    faction: str = "Neutral"           # "Horde", "Alliance", "Neutral"
    hotspots: List[Hotspot] = field(default_factory=list)
    game_object_id: Optional[int] = None  # e.g. 1731 for Copper Vein
    game_object_name: Optional[str] = None
    item_id: Optional[int] = None        # e.g. 2770 for Copper Ore
    item_name: Optional[str] = None      # e.g. "Copper Ore"
    description: Optional[str] = None


# ============================================================================
# Zone Map Boundaries: zone_name → {map_id, top, left, bottom, right}
# These are the WoW map coordinate boundaries for converting Zygor % → world coords
# Format: X(world) = left + (right - left) * (x_pct / 100)
#         Y(world) = top + (bottom - top) * (y_pct / 100)  
# Note: WoW's coordinate system: X is North/South, Y is East/West
# Zygor: x% is horizontal (→ WoW Y), y% is vertical (→ WoW X)
# So: WoW_X = top - (y_pct/100) * (top - bottom)   [top is north/higher X]
#     WoW_Y = left - (x_pct/100) * (left - right)   [left is west/higher Y]
# Actually in WoW: going East = decreasing Y, going South = decreasing X
# Zygor: x% increases left-to-right (West to East), y% increases top-to-bottom (North to South)
# ============================================================================

# Zone boundaries: {zone: (map_continent, top_x, left_y, bottom_x, right_y)}
# Derived from WoW map data for 3.3.5a (build 12340)
# top_x = northernmost X (highest), bottom_x = southernmost X (lowest)
# left_y = westernmost Y (highest), right_y = easternmost Y (lowest)
ZONE_MAP_BOUNDS = {
    # === Kalimdor (map 1) ===
    "Durotar":               (1, 1925.0, -5600.0, -800.0, -3200.0),
    "Valley of Trials":      (1, 400.0, -5100.0, -400.0, -4300.0),
    "Mulgore":               (1, 100.0, -2800.0, -2700.0, -100.0),
    "The Barrens":           (1, 2600.0, -5300.0, -2050.0, -1100.0),
    "Orgrimmar":             (1, 2050.0, -5050.0, 1000.0, -3800.0),
    "Thunder Bluff":         (1, -200.0, -1500.0, -600.0, -1050.0),
    "Ashenvale":             (1, 4000.0, -6200.0, 1300.0, -2000.0),
    "Stonetalon Mountains":  (1, 2400.0, -2300.0, 200.0, -100.0),
    "Thousand Needles":      (1, -1000.0, -7000.0, -5400.0, -3400.0),
    "Desolace":              (1, 3400.0, -500.0, 300.0, 2800.0),
    "Dustwallow Marsh":      (1, -100.0, -5100.0, -4200.0, -2300.0),
    "Feralas":               (1, 800.0, -5800.0, -3500.0, -2200.0),
    "Tanaris":               (1, -3200.0, -9800.0, -8000.0, -5800.0),
    "Un'Goro Crater":        (1, -3200.0, -8100.0, -5700.0, -5100.0),
    "Silithus":              (1, -5100.0, -9100.0, -8600.0, -5600.0),
    "Felwood":               (1, 7300.0, -5200.0, 3250.0, -2650.0),
    "Winterspring":          (1, 8300.0, -7100.0, 4000.0, -4100.0),
    "Azshara":               (1, 5400.0, -8300.0, 2000.0, -4850.0),
    "Moonglade":             (1, 7900.0, -4900.0, 7200.0, -4200.0),
    "Darkshore":             (1, 8400.0, -2200.0, 4400.0, 660.0),
    "Teldrassil":            (1, 11200.0, 2200.0, 8700.0, 400.0),

    # === Eastern Kingdoms (map 0) ===
    "Elwynn Forest":         (0, 100.0, -9600.0, -1800.0, -8200.0),
    "Westfall":              (0, -500.0, -11200.0, -1600.0, -9900.0),
    "Redridge Mountains":    (0, -200.0, -9300.0, -1200.0, -8200.0),
    "Duskwood":              (0, -400.0, -11100.0, -1400.0, -9800.0),
    "Stranglethorn Vale":    (0, 100.0, -14800.0, -3600.0, -12400.0),
    "Swamp of Sorrows":      (0, -1000.0, -10800.0, -2800.0, -9900.0),
    "Blasted Lands":         (0, -2800.0, -11800.0, -4400.0, -10500.0),
    "Burning Steppes":       (0, -6200.0, -8200.0, -7600.0, -6800.0),
    "Searing Gorge":         (0, -6100.0, -7200.0, -7200.0, -6300.0),
    "Badlands":              (0, -3500.0, -7500.0, -5800.0, -5400.0),
    "Loch Modan":            (0, -3100.0, -6100.0, -4700.0, -4800.0),
    "Wetlands":              (0, -1900.0, -4400.0, -3600.0, -2800.0),
    "Arathi Highlands":      (0, -1700.0, -3100.0, -3700.0, -1400.0),
    "Hillsbrad Foothills":   (0, -400.0, -1400.0, -1600.0, 100.0),
    "Silverpine Forest":     (0, 1000.0, -1400.0, -600.0, 700.0),
    "Tirisfal Glades":       (0, 3100.0, -1300.0, 1200.0, 1500.0),
    "Western Plaguelands":   (0, 3300.0, -500.0, 1100.0, 1700.0),
    "Eastern Plaguelands":   (0, 3700.0, 1500.0, 1300.0, 3600.0),
    "The Hinterlands":       (0, 200.0, -200.0, -1300.0, 1100.0),
    "Dun Morogh":            (0, -4200.0, -6200.0, -6000.0, -4600.0),
    "Alterac Mountains":     (0, 300.0, -400.0, -700.0, 600.0),
    
    # === Blood Elf / Draenei (map 530) ===
    "Eversong Woods":        (530, 10100.0, -7200.0, 8100.0, -5700.0),
    "Ghostlands":            (530, 8700.0, -5000.0, 6500.0, -2800.0),
    
    # === Outland (map 530) ===
    "Hellfire Peninsula":    (530, 340.0, 8300.0, -2400.0, 4900.0),
    "Zangarmarsh":           (530, 830.0, 4700.0, -1300.0, 2400.0),
    "Terokkar Forest":       (530, -1300.0, 5600.0, -4750.0, 1700.0),
    "Nagrand":               (530, -1200.0, 1800.0, -4100.0, -1400.0),
    "Blade's Edge Mountains":(530, 4100.0, 5500.0, 1400.0, 2300.0),
    "Netherstorm":           (530, 5200.0, 6000.0, 1500.0, 2000.0),
    "Shadowmoon Valley":     (530, -2200.0, 6400.0, -5200.0, 2700.0),
    
    # === Northrend (map 571) ===
    "Borean Tundra":         (571, 6200.0, 2600.0, 2200.0, 6400.0),
    "Howling Fjord":         (571, 3100.0, -2700.0, 700.0, 600.0),
    "Dragonblight":          (571, 5700.0, -200.0, 2700.0, 3400.0),
    "Grizzly Hills":         (571, 4800.0, -3100.0, 3200.0, -1200.0),
    "Zul'Drak":              (571, 7000.0, -2200.0, 4600.0, 400.0),
    "Sholazar Basin":        (571, 6650.0, 3900.0, 4800.0, 5700.0),
    "The Storm Peaks":       (571, 9600.0, 400.0, 5800.0, 4400.0),
    "Icecrown":              (571, 8600.0, 3600.0, 5800.0, 6600.0),
}

# Zone name to WoW MapId mapping
ZONE_TO_MAPID = {
    "Durotar": 14, "Valley of Trials": 363, "Mulgore": 215,
    "Tirisfal Glades": 85, "The Barrens": 17, "Orgrimmar": 1637,
    "Thunder Bluff": 1638, "Undercity": 1497, "Ashenvale": 331,
    "Stonetalon Mountains": 406, "Thousand Needles": 400, "Desolace": 405,
    "Dustwallow Marsh": 15, "Feralas": 357, "Tanaris": 440,
    "Un'Goro Crater": 490, "Silithus": 1377, "Felwood": 361,
    "Winterspring": 618, "Azshara": 16, "Moonglade": 493,
    "Darkshore": 148, "Teldrassil": 141,
    
    "Elwynn Forest": 12, "Westfall": 40, "Redridge Mountains": 44,
    "Duskwood": 10, "Stranglethorn Vale": 33, "Swamp of Sorrows": 8,
    "Blasted Lands": 4, "Burning Steppes": 46, "Searing Gorge": 51,
    "Badlands": 3, "Loch Modan": 38, "Wetlands": 11,
    "Arathi Highlands": 45, "Hillsbrad Foothills": 267,
    "Alterac Mountains": 36, "Silverpine Forest": 130,
    "Western Plaguelands": 28, "Eastern Plaguelands": 139,
    "The Hinterlands": 47, "Dun Morogh": 1,
    
    "Eversong Woods": 3430, "Ghostlands": 3433,
    
    "Hellfire Peninsula": 3483, "Zangarmarsh": 3521, "Terokkar Forest": 3519,
    "Nagrand": 3518, "Blade's Edge Mountains": 3522, "Netherstorm": 3523,
    "Shadowmoon Valley": 3520,
    
    "Borean Tundra": 3537, "Howling Fjord": 495, "Dragonblight": 65,
    "Grizzly Hills": 394, "Zul'Drak": 66, "Sholazar Basin": 3711,
    "The Storm Peaks": 67, "Icecrown": 210,
}


# ============================================================================
# Coordinate Conversion
# ============================================================================

def zygor_pct_to_world(zone: str, x_pct: float, y_pct: float) -> Optional[Tuple[float, float, float]]:
    """
    Convert Zygor percentage map coordinates to WoW world coordinates.
    
    Zygor: x% = horizontal (0=left/west, 100=right/east)
           y% = vertical (0=top/north, 100=bottom/south)
    
    WoW:   X axis = North(+) / South(-)
           Y axis = West(+) / East(-)
    
    Returns (world_x, world_y, z=0) or None if zone not found.
    """
    if zone not in ZONE_MAP_BOUNDS:
        return None
    
    _continent, top_x, left_y, bottom_x, right_y = ZONE_MAP_BOUNDS[zone]
    
    # Zygor y% (top-to-bottom) → WoW X (north-to-south, decreasing)
    world_x = top_x - (y_pct / 100.0) * (top_x - bottom_x)
    
    # Zygor x% (left-to-right, west to east) → WoW Y (west-to-east, decreasing) 
    world_y = left_y - (x_pct / 100.0) * (left_y - right_y)
    
    return (round(world_x, 3), round(world_y, 3), 0.0)


# ============================================================================
# Regex Patterns for Zygor Profession Guides
# ============================================================================

# Guide registration
PATTERN_REGISTER = re.compile(
    r'RegisterGuide\("Profession Guides\\\\(Herbalism|Mining)\\\\(?:Farming Guides\\\\|)(.*?)"'
)

# Guide metadata
PATTERN_SKILL_COND = re.compile(r"skill\('(Herbalism|Mining)'\)\s*>=\s*(\d+)")
PATTERN_DESCRIPTION = re.compile(r'description\s*=\s*"\\n(.*?)"')

# Step patterns
PATTERN_MAP = re.compile(r'^\s*map\s+(.+?)(?:/\d+)?\s*$')
PATTERN_PATH = re.compile(r'^\s*path\s+([\d.,\s\t]+)\s*$')
PATTERN_PATH_OPTIONS = re.compile(r'^\s*path\s+(?:follow\s+\w+|loop\s+\w+|ants\s+\w+|dist\s+\d+)')
PATTERN_SKILL_TARGET = re.compile(r'\|skill\s+(?:Herbalism|Mining)\s*,\s*(\d+)')
PATTERN_SKILLMAX = re.compile(r'\|skillmax\s+(?:Herbalism|Mining)\s*,\s*(\d+)')
PATTERN_CLICK = re.compile(r'click\s+(.+?)##(\d+)')
PATTERN_COLLECT = re.compile(r'collect\s+(?:\d+\s+)?(.+?)##(\d+)')
PATTERN_DING = re.compile(r'\|ding\s+(\d+)')
PATTERN_TALK = re.compile(r'talk\s+(.+?)##(\d+)')
PATTERN_GOTO = re.compile(r'\|goto\s+(?:([^/\d][^/]*?)(?:/\d+)?\s+)?([\d.]+)\s*,\s*([\d.]+)')

# Path coordinate values (tab or space separated pairs)
PATTERN_COORD_PAIR = re.compile(r'([\d.]+)\s*,\s*([\d.]+)')


# ============================================================================
# Parser Class
# ============================================================================

class ZygorGatherParser:
    def __init__(self, gameobject_spawns_path: Optional[str] = None):
        self.gameobject_spawns: Dict[int, List[Dict]] = {}
        self.gameobject_names: Dict[int, str] = {}
        
        if gameobject_spawns_path:
            self._load_gameobject_spawns(gameobject_spawns_path)
    
    def _load_gameobject_spawns(self, path: str):
        """Load game object world coordinates from SPP database export."""
        try:
            with open(path, 'r', encoding='utf-8') as f:
                data = json.load(f)
            
            objects = data.get("objects", {})
            for obj_id_str, obj_data in objects.items():
                obj_id = int(obj_id_str)
                name = obj_data.get("name", "")
                spawns = obj_data.get("spawns", [])
                self.gameobject_names[obj_id] = name
                self.gameobject_spawns[obj_id] = spawns
            
            print(f"  Loaded {len(self.gameobject_spawns)} game objects from spawns DB")
        except Exception as e:
            print(f"  Warning: Could not load gameobject_spawns.json: {e}")
    
    def parse_file(self, filepath: str, faction: str = "Horde") -> List[GatherRoute]:
        """Parse a Zygor profession guide file and extract gathering routes."""
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
        
        routes = []
        
        # Split into guide blocks
        guide_blocks = self._split_guides(content)
        
        for guide_name, guide_header, guide_body in guide_blocks:
            parsed = self._parse_guide(guide_name, guide_header, guide_body, faction)
            if parsed:
                routes.extend(parsed)
        
        return routes
    
    def _split_guides(self, content: str) -> List[Tuple[str, str, str]]:
        """Split file content into (guide_name, header, body) tuples."""
        blocks = []
        
        # Find all RegisterGuide calls for Herbalism/Mining
        pattern = re.compile(
            r'RegisterGuide\("(Profession Guides\\\\(?:Herbalism|Mining)\\\\.*?)",'
            r'(.*?)\},\[\[(.*?)\]\]\)',
            re.DOTALL
        )
        
        for match in pattern.finditer(content):
            guide_name = match.group(1).replace('\\\\', '\\')
            header = match.group(2)
            body = match.group(3)
            blocks.append((guide_name, header, body))
        
        return blocks
    
    def _parse_guide(self, guide_name: str, header: str, body: str, faction: str) -> List[GatherRoute]:
        """Parse a single guide block into one or more GatherRoutes."""
        routes = []
        
        # Determine profession and guide type
        parts = guide_name.split('\\')
        profession = parts[1] if len(parts) > 1 else "Unknown"  # "Herbalism" or "Mining"
        
        is_farming = "Farming Guides" in guide_name
        is_leveling = not is_farming and any("(" in p for p in parts)
        if not is_leveling and not is_farming:
            is_leveling = len(parts) == 2
        
        guide_type = "Leveling" if is_leveling else "Farming"
        
        # Get guide item/resource name
        if is_farming:
            resource_name = parts[-1] if len(parts) > 2 else profession
        else:
            resource_name = parts[-1] if len(parts) > 1 else profession
        
        # Parse skill requirement from header
        min_skill = 0
        skill_match = PATTERN_SKILL_COND.search(header)
        if skill_match:
            min_skill = int(skill_match.group(2))
        
        # Parse description
        description = None
        desc_match = PATTERN_DESCRIPTION.search(header)
        if desc_match:
            description = desc_match.group(1).strip()
        
        # Split body into steps
        steps = body.split('\nstep\n')
        if not steps[0].strip().startswith('step'):
            # First chunk might be empty or partial
            if steps[0].strip():
                steps[0] = steps[0]
        
        # Parse each step, collect gathering routes with paths 
        current_zone = None
        current_coords = []
        current_skill_target = 0
        current_skill_start = min_skill
        current_object_id = None
        current_object_name = None
        current_item_id = None
        current_item_name = None
        
        for step_text in steps:
            lines = step_text.strip().split('\n')
            step_zone = None
            step_coords = []
            step_skill_target = None
            step_object_id = None
            step_object_name = None
            step_item_id = None
            step_item_name = None
            is_path_step = False
            is_trainer_step = False
            
            for line in lines:
                line = line.strip()
                
                # Skip empty/comment lines
                if not line or line.startswith('--'):
                    continue
                
                # Map zone
                map_match = PATTERN_MAP.match(line)
                if map_match:
                    step_zone = map_match.group(1).strip()
                    continue
                
                # Path options line (follow smart; loop on; etc.)
                if PATTERN_PATH_OPTIONS.match(line):
                    is_path_step = True
                    # Some option lines also have coords after the options
                    # e.g. "path	follow smart;	loop on;	ants curved;	dist 30"
                    # but also "path	53.29,45.77	55.68,46.14 ..."
                    # Just grab any coord pairs
                    pairs = PATTERN_COORD_PAIR.findall(line)
                    for x_str, y_str in pairs:
                        x_val, y_val = float(x_str), float(y_str)
                        if x_val > 5.0 and y_val > 5.0:  # Filter options like dist 30
                            step_coords.append((x_val, y_val))
                    continue
                
                # Path coordinates
                path_match = PATTERN_PATH.match(line)
                if path_match:
                    is_path_step = True
                    coords_str = path_match.group(1)
                    pairs = PATTERN_COORD_PAIR.findall(coords_str)
                    for x_str, y_str in pairs:
                        step_coords.append((float(x_str), float(y_str)))
                    continue
                
                # Also try matching path lines that start with "path" and have coords
                if line.startswith('path') and '\t' in line:
                    is_path_step = True
                    pairs = PATTERN_COORD_PAIR.findall(line)
                    # Filter out small numbers that might be "dist 30" etc.
                    for x_str, y_str in pairs:
                        x_val, y_val = float(x_str), float(y_str)
                        if x_val > 1.0 and y_val > 1.0:  # Skip dist/option values
                            step_coords.append((x_val, y_val))
                    continue
                
                # Skill target
                skill_match = PATTERN_SKILL_TARGET.search(line)
                if skill_match:
                    step_skill_target = int(skill_match.group(1))
                
                # Skillmax (trainer step)
                if PATTERN_SKILLMAX.search(line):
                    is_trainer_step = True
                
                # Trainer talk
                if PATTERN_TALK.search(line) and ('Train' in line or 'skillmax' in line):
                    is_trainer_step = True
                
                # Click (game object interaction)
                click_match = PATTERN_CLICK.search(line)
                if click_match:
                    step_object_name = click_match.group(1).strip()
                    step_object_id = int(click_match.group(2))
                
                # Collect (item)
                collect_match = PATTERN_COLLECT.search(line)
                if collect_match:
                    step_item_name = collect_match.group(1).strip()
                    step_item_id = int(collect_match.group(2))
            
            # Skip trainer/level requirement steps
            if is_trainer_step or not is_path_step:
                continue
            
            # Build a route from this step
            zone = step_zone or current_zone
            if step_zone:
                current_zone = step_zone
            
            if not zone or not step_coords:
                continue
            
            # Update tracking
            if step_object_id:
                current_object_id = step_object_id
                current_object_name = step_object_name
            if step_item_id:
                current_item_id = step_item_id
                current_item_name = step_item_name
            
            # Convert coordinates
            hotspots = []
            for x_pct, y_pct in step_coords:
                world = zygor_pct_to_world(zone, x_pct, y_pct)
                if world:
                    hotspots.append(Hotspot(x=world[0], y=world[1], z=world[2]))
            
            if not hotspots:
                print(f"  Warning: No world coords for zone '{zone}' in route '{resource_name}'")
                continue
            
            # Determine skill range
            skill_start = current_skill_start
            skill_end = step_skill_target or current_skill_target or (min_skill + 50)
            
            if step_skill_target:
                current_skill_start = step_skill_target
                current_skill_target = step_skill_target
            
            # Build route name
            if guide_type == "Farming":
                route_name = resource_name
                route_desc = description
                # Use the header skill requirement for farming guides
                skill_start = min_skill
                skill_end = min_skill + 75  # Approximate: farm until ~75 above min
            else:
                route_name = f"{profession} {skill_start}-{skill_end}"
                route_desc = f"Level {profession} from {skill_start} to {skill_end} in {zone}"
            
            route = GatherRoute(
                name=route_name,
                profession=profession,
                guide_type=guide_type,
                zone=zone,
                map_id=ZONE_TO_MAPID.get(zone, 0),
                min_skill=skill_start,
                max_skill=skill_end,
                faction=faction,
                hotspots=hotspots,
                game_object_id=step_object_id or current_object_id,
                game_object_name=step_object_name or current_object_name,
                item_id=step_item_id or current_item_id,
                item_name=step_item_name or current_item_name,
                description=route_desc,
            )
            routes.append(route)
        
        # Post-process: merge multi-step farming guides into a single route
        # (e.g. Mana Thistle has 3 steps across 2 zones — merge hotspots)
        if guide_type == "Farming" and len(routes) > 1:
            merged = routes[0]
            for extra in routes[1:]:
                merged.hotspots.extend(extra.hotspots)
                # If multiple zones, note it
                if extra.zone != merged.zone:
                    merged.zone = f"{merged.zone}+{extra.zone}"
            routes = [merged]
        
        return routes


# ============================================================================
# XML Generator
# ============================================================================

def route_to_hbprofile_xml(route: GatherRoute) -> str:
    """Convert a GatherRoute to GatherBuddy2-style HBProfile XML."""
    
    lines = []
    lines.append('<HBProfile>')
    lines.append(f'  <Name>(GB2 {route.min_skill}-{route.max_skill})({route.zone}.{route.faction}){route.profession}</Name>')
    lines.append(f'  <MinDurability>0.4</MinDurability>')
    lines.append(f'  <MinFreeBagSlots>1</MinFreeBagSlots>')
    lines.append(f'')
    lines.append(f'  <MinLevel>1</MinLevel>')
    lines.append(f'  <MaxLevel>91</MaxLevel>')
    lines.append(f'  <Factions>99999</Factions>')
    lines.append(f'')
    lines.append(f'  <MailGrey>False</MailGrey>')
    lines.append(f'  <MailWhite>True</MailWhite>')
    lines.append(f'  <MailGreen>True</MailGreen>')
    lines.append(f'  <MailBlue>True</MailBlue>')
    lines.append(f'  <MailPurple>True</MailPurple>')
    lines.append(f'')
    lines.append(f'  <SellGrey>True</SellGrey>')
    lines.append(f'  <SellWhite>True</SellWhite>')
    lines.append(f'  <SellGreen>False</SellGreen>')
    lines.append(f'  <SellBlue>False</SellBlue>')
    lines.append(f'  <SellPurple>False</SellPurple>')
    lines.append(f'')
    lines.append(f'  <Vendors>')
    lines.append(f'')
    lines.append(f'  </Vendors>')
    lines.append(f'')
    lines.append(f'  <Mailboxes>')
    lines.append(f'')
    lines.append(f'  </Mailboxes>')
    lines.append(f'  <Blackspots>')
    lines.append(f'    <!-- Add blackspots manually -->')
    lines.append(f'  </Blackspots>')
    lines.append(f'')
    lines.append(f'  <Hotspots>')
    
    for hs in route.hotspots:
        lines.append(f'    <Hotspot X="{hs.x}" Y="{hs.y}" Z="{hs.z}" />')
    
    lines.append(f'  </Hotspots>')
    lines.append('</HBProfile>')
    
    return '\n'.join(lines)


def routes_to_combined_xml(routes: List[GatherRoute], profession: str) -> str:
    """
    Create a combined profile from multiple routes for a given profession.
    This merges steps for "leveling" guides into sequential routes.
    """
    if not routes:
        return ""
    
    # For farming guides: one profile per resource
    # For leveling guides: one profile per skill segment
    return route_to_hbprofile_xml(routes[0])


# ============================================================================
# Main CLI
# ============================================================================

def main():
    import argparse
    
    parser = argparse.ArgumentParser(
        description="Parse Zygor Profession Guides into GatherBuddy2 HBProfile XML"
    )
    parser.add_argument("zygor_file", help="Path to Zygor profession guide .lua file")
    parser.add_argument("-o", "--output", default="output_gather", help="Output directory")
    parser.add_argument("--faction", default="Horde", choices=["Horde", "Alliance", "Neutral"],
                       help="Faction filter")
    parser.add_argument("--spawns", default=None, help="Path to gameobject_spawns.json")
    parser.add_argument("--profession", default=None, choices=["Herbalism", "Mining"],
                       help="Filter to specific profession (default: both)")
    parser.add_argument("--type", default=None, choices=["Farming", "Leveling"],
                       help="Filter to guide type")
    parser.add_argument("--list", action="store_true", help="List routes without generating files")
    
    args = parser.parse_args()
    
    # Auto-detect spawns JSON
    spawns_path = args.spawns
    if not spawns_path:
        default_spawns = Path(__file__).parent / "gameobject_spawns.json"
        if default_spawns.exists():
            spawns_path = str(default_spawns)
    
    print(f"Parsing: {args.zygor_file}")
    print(f"Faction: {args.faction}")
    
    # Parse
    gather_parser = ZygorGatherParser(gameobject_spawns_path=spawns_path)
    routes = gather_parser.parse_file(args.zygor_file, faction=args.faction)
    
    # Filter
    if args.profession:
        routes = [r for r in routes if r.profession == args.profession]
    if args.type:
        routes = [r for r in routes if r.guide_type == args.type]
    
    print(f"\nFound {len(routes)} gathering routes:")
    for i, route in enumerate(routes, 1):
        print(f"  {i:3d}. [{route.guide_type:8s}] {route.profession:10s} | "
              f"{route.min_skill:3d}-{route.max_skill:3d} | "
              f"{route.zone:25s} | {route.name} "
              f"({len(route.hotspots)} hotspots)")
    
    if args.list:
        return
    
    # Generate output
    output_dir = Path(args.output)
    output_dir.mkdir(parents=True, exist_ok=True)
    
    # Group routes by type
    farming_routes = [r for r in routes if r.guide_type == "Farming"]
    leveling_routes = [r for r in routes if r.guide_type == "Leveling"]
    
    generated = 0
    
    # Generate individual farming profiles
    for route in farming_routes:
        safe_name = re.sub(r'[^\w\s\-\.\(\)]', '', route.name).strip()
        filename = f"(GB2 {route.profession})({route.zone}.{route.faction}){safe_name}.xml"
        filepath = output_dir / filename
        
        xml_content = route_to_hbprofile_xml(route)
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(xml_content)
        
        generated += 1
        print(f"  Generated: {filename} ({len(route.hotspots)} hotspots)")
    
    # Generate leveling profiles (one file per skill segment/zone)
    for route in leveling_routes:
        safe_zone = re.sub(r'[^\w\s\-\.\(\)]', '', route.zone).strip()
        filename = f"(GB2 {route.min_skill}-{route.max_skill})({safe_zone}.{route.faction}){route.profession}.xml"
        filepath = output_dir / filename
        
        xml_content = route_to_hbprofile_xml(route)
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(xml_content)
        
        generated += 1
        print(f"  Generated: {filename} ({len(route.hotspots)} hotspots)")
    
    print(f"\n{'='*60}")
    print(f"Generated {generated} profiles in {output_dir}")
    print(f"{'='*60}")


if __name__ == "__main__":
    main()
