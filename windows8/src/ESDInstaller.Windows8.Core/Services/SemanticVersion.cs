using System;
using System.Collections.Generic;
using System.Linq;

namespace ESDInstaller.Windows8.Core.Services;

public sealed class SemanticVersion : IComparable<SemanticVersion>
{
    private SemanticVersion(int major, int minor, int patch, IReadOnlyList<string> preRelease)
    {
        Major = major; Minor = minor; Patch = patch; PreRelease = preRelease;
    }

    public int Major { get; }
    public int Minor { get; }
    public int Patch { get; }
    public IReadOnlyList<string> PreRelease { get; }

    public static SemanticVersion Parse(string value)
    {
        SemanticVersion version;
        if (!TryParse(value, out version)) throw new FormatException("'" + value + "' is not a valid semantic version.");
        return version;
    }

    public static bool TryParse(string value, out SemanticVersion version)
    {
        version = null!;
        if (string.IsNullOrWhiteSpace(value)) return false;
        var text = value.Trim();
        if (text.StartsWith("v", StringComparison.OrdinalIgnoreCase)) text = text.Substring(1);
        var buildIndex = text.IndexOf('+');
        if (buildIndex >= 0) text = text.Substring(0, buildIndex);
        var preRelease = new string[0];
        var preReleaseIndex = text.IndexOf('-');
        if (preReleaseIndex >= 0)
        {
            preRelease = text.Substring(preReleaseIndex + 1).Split('.');
            text = text.Substring(0, preReleaseIndex);
            if (preRelease.Length == 0 || preRelease.Any(string.IsNullOrEmpty)) return false;
        }
        var parts = text.Split('.');
        int major, minor, patch;
        if (parts.Length != 3 || !TryPart(parts[0], out major) || !TryPart(parts[1], out minor) || !TryPart(parts[2], out patch)) return false;
        version = new SemanticVersion(major, minor, patch, preRelease);
        return true;
    }

    public int CompareTo(SemanticVersion other)
    {
        if (other == null) return 1;
        var result = Major.CompareTo(other.Major); if (result != 0) return result;
        result = Minor.CompareTo(other.Minor); if (result != 0) return result;
        result = Patch.CompareTo(other.Patch); if (result != 0) return result;
        if (PreRelease.Count == 0) return other.PreRelease.Count == 0 ? 0 : 1;
        if (other.PreRelease.Count == 0) return -1;
        for (var index = 0; index < Math.Min(PreRelease.Count, other.PreRelease.Count); index++)
        {
            int left, right;
            var leftNumeric = int.TryParse(PreRelease[index], out left);
            var rightNumeric = int.TryParse(other.PreRelease[index], out right);
            if (leftNumeric && rightNumeric) result = left.CompareTo(right);
            else if (leftNumeric != rightNumeric) result = leftNumeric ? -1 : 1;
            else result = string.CompareOrdinal(PreRelease[index], other.PreRelease[index]);
            if (result != 0) return result;
        }
        return PreRelease.Count.CompareTo(other.PreRelease.Count);
    }

    public override string ToString() => Major + "." + Minor + "." + Patch +
        (PreRelease.Count == 0 ? string.Empty : "-" + string.Join(".", PreRelease));

    private static bool TryPart(string value, out int part) =>
        int.TryParse(value, out part) && part >= 0 && (value == "0" || !value.StartsWith("0", StringComparison.Ordinal));
}
