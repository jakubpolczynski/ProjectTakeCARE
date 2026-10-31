#!/usr/bin/env python3
"""Check tracked configuration without ever printing credential values."""
import json
import pathlib
import re
import subprocess
import sys

root = pathlib.Path(__file__).resolve().parents[1]
paths = subprocess.check_output(["git", "-C", str(root), "ls-files", "-z"]).decode().split("\0")
# Include untracked safe templates and configuration in a delivery checkout.
paths += [str(p.relative_to(root)) for p in (root / "TakeCare").glob("appsettings*.json")]
problems = []
for path in sorted(set(filter(None, paths))):
    source = root / path
    if not source.is_file():
        continue
    if re.search(r"(^|/)appsettings[^/]*\.json$", path, re.I):
        try:
            data = json.loads(source.read_text(encoding="utf-8-sig"))
        except (ValueError, OSError):
            problems.append((path, "invalid-json"))
            continue
        for section, names in {"Jwt": ["Key", "SigningKeyBase64"], "Email": ["Public", "Secret"]}.items():
            for name in names:
                if data.get(section, {}).get(name):
                    problems.append((path, section + ":" + name))
        for name, value in data.get("ConnectionStrings", {}).items():
            if value:
                problems.append((path, "ConnectionStrings:" + name))
    if re.search(r"\.(pfx|p12|pem|key)$", path, re.I):
        problems.append((path, "tracked-private-key-or-certificate"))
for path, key in problems:
    print(path + ": disallowed configuration field " + key)
if problems:
    sys.exit(1)
print("Configuration check passed; no credential values printed.")
