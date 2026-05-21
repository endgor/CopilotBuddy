#!/usr/bin/env python3
"""
Convert wRobot GathererProfile XML to HB GatherBuddy profile XML.

Usage:
    python convert_wrobot_to_hb.py <input.xml> [output.xml]
    python convert_wrobot_to_hb.py <folder>         # batch convert all .xml in folder

wRobot format:
    <GathererProfile>
      <Vectors3>
        <Vector3 X="..." Y="..." Z="..." Type="Flying|Ground" />

HB format:
    <HBProfile>
      <Hotspots>
        <Hotspot X="..." Y="..." Z="..." />
"""

import sys
import os
import xml.etree.ElementTree as ET


def convert_file(input_path: str, output_path: str | None = None) -> str:
    """Convert a single wRobot XML file to HB format. Returns output path."""

    # wRobot files often have BOM + wrong encoding declaration — handle all cases
    with open(input_path, 'rb') as f:
        raw = f.read()

    # Detect actual encoding from BOM
    if raw[:3] == b'\xef\xbb\xbf':
        # UTF-8 BOM — file is UTF-8 despite possible wrong declaration
        content = raw[3:].decode('utf-8', errors='replace')
    elif raw[:2] in (b'\xff\xfe', b'\xfe\xff'):
        # UTF-16 BOM
        content = raw.decode('utf-16', errors='replace')
    else:
        # Try UTF-8, fall back to latin-1
        try:
            content = raw.decode('utf-8', errors='strict')
        except UnicodeDecodeError:
            content = raw.decode('latin-1', errors='replace')

    # Fix any wrong encoding declaration so ET accepts it
    import re as _re
    content = _re.sub(r'encoding=["\']utf-16["\']', 'encoding="utf-8"', content, flags=_re.IGNORECASE)

    try:
        root = ET.fromstring(content.encode('utf-8'))
    except ET.ParseError as e:
        raise ValueError(f"XML parse error: {e}")

    # Validate it's a GathererProfile
    if root.tag != "GathererProfile":
        raise ValueError(f"Not a GathererProfile: root tag is <{root.tag}>")

    vectors = root.find("Vectors3")
    if vectors is None:
        raise ValueError("No <Vectors3> element found")

    hotspots = list(vectors.findall("Vector3"))
    if not hotspots:
        raise ValueError("No <Vector3> hotspots found")

    # Build profile name from filename
    base_name = os.path.splitext(os.path.basename(input_path))[0]

    # Build HB XML as a string (ElementTree doesn't preserve formatting well)
    lines = []
    lines.append('<HBProfile>')
    lines.append(f'\t<Name>{base_name}</Name>')
    lines.append(f'\t<MinDurability>0.3</MinDurability>')
    lines.append(f'\t<MinFreeBagSlots>3</MinFreeBagSlots>')
    lines.append('')
    lines.append(f'\t<MinLevel>1</MinLevel>')
    lines.append(f'\t<MaxLevel>101</MaxLevel>')
    lines.append(f'\t<Factions>99999</Factions>')
    lines.append('')
    lines.append(f'\t<MailGrey>false</MailGrey>')
    lines.append(f'\t<MailWhite>True</MailWhite>')
    lines.append(f'\t<MailGreen>True</MailGreen>')
    lines.append(f'\t<MailBlue>True</MailBlue>')
    lines.append(f'\t<MailPurple>True</MailPurple>')
    lines.append('')
    lines.append(f'\t<SellGrey>false</SellGrey>')
    lines.append(f'\t<SellWhite>false</SellWhite>')
    lines.append(f'\t<SellGreen>false</SellGreen>')
    lines.append(f'\t<SellBlue>false</SellBlue>')
    lines.append(f'\t<SellPurple>false</SellPurple>')
    lines.append('\t<Vendors>')
    lines.append('\t</Vendors>')
    lines.append('\t<Mailboxes>')
    lines.append('\t</Mailboxes>')
    lines.append('\t<Blackspots>')
    lines.append('\t</Blackspots>')
    lines.append('\t<Hotspots>')
    for h in hotspots:
        x = h.get('X', '0')
        y = h.get('Y', '0')
        z = h.get('Z', '0')
        lines.append(f'\t    <Hotspot X="{x}" Y="{y}" Z="{z}" />')
    lines.append('\t</Hotspots>')
    lines.append('</HBProfile>')

    output_xml = '\n'.join(lines)

    # Determine output path
    if output_path is None:
        output_path = os.path.splitext(input_path)[0] + '_HB.xml'

    with open(output_path, 'w', encoding='utf-8') as f:
        f.write(output_xml)

    print(f"  [{len(hotspots)} hotspots] {os.path.basename(input_path)} -> {os.path.basename(output_path)}")
    return output_path


def convert_folder(folder: str, output_folder: str | None = None):
    """Batch convert all wRobot XMLs in a folder."""
    if output_folder is None:
        output_folder = os.path.join(folder, 'HB_Converted')
    os.makedirs(output_folder, exist_ok=True)

    xml_files = [f for f in os.listdir(folder) if f.lower().endswith('.xml')]
    if not xml_files:
        print(f"No XML files found in {folder}")
        return

    print(f"Converting {len(xml_files)} files -> {output_folder}")
    ok = 0
    for fname in sorted(xml_files):
        input_path = os.path.join(folder, fname)
        out_name = os.path.splitext(fname)[0] + '_HB.xml'
        output_path = os.path.join(output_folder, out_name)
        try:
            convert_file(input_path, output_path)
            ok += 1
        except Exception as e:
            print(f"  SKIP {fname}: {e}")

    print(f"\nDone: {ok}/{len(xml_files)} converted.")


def main():
    if len(sys.argv) < 2:
        print(__doc__)
        sys.exit(1)

    target = sys.argv[1]
    output = sys.argv[2] if len(sys.argv) >= 3 else None

    if os.path.isdir(target):
        convert_folder(target, output)
    elif os.path.isfile(target):
        convert_file(target, output)
    else:
        print(f"Error: {target} is not a file or folder")
        sys.exit(1)


if __name__ == '__main__':
    main()
