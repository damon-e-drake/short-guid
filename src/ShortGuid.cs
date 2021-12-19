using System;
using System.Text.RegularExpressions;

namespace DEDrake {
  public struct ShortGuid {
    private readonly static Regex GuidRegEx = new(@"^[0-9a-fA-F]{32}$");

    private readonly Guid _guid;
    private readonly string _value;

    public readonly static ShortGuid Empty = new(Guid.Empty);

    public Guid Guid { get => _guid; }

    public ShortGuid(string value) {
      _value = value;
      _guid = Decode(value);
    }

    public ShortGuid(Guid guid) {
      _value = Encode(guid);
      _guid = guid;
    }

    public override string ToString() {
      return _value;
    }

    public override bool Equals(object obj) {
      if (obj is null)
        throw new ArgumentNullException(nameof(obj));

      if (obj is ShortGuid guid)
        return _guid.Equals(guid._guid);
      if (obj is Guid guid1)
        return _guid.Equals(guid1);
      if (obj is string)
        return _guid.Equals(((ShortGuid)obj)._guid);
      return false;
    }

    public override int GetHashCode() {
      return _guid.GetHashCode();
    }

    public static ShortGuid Parse(string value) {
#if NET40_OR_GREATER
      if (string.IsNullOrWhiteSpace(value))
        throw new ArgumentNullException(nameof(value));
#else
      if (string.IsNullOrEmpty(value))
        throw new ArgumentNullException(nameof(value));

      if (value.Trim() == "")
        throw new ArgumentNullException(nameof(value));
#endif
      value = value.Trim();

      ShortGuid sg;

      if (value.Length >= 22 && value.Length <= 24) {
        value = value.Replace("=", "");
        sg = new ShortGuid(value);
      }
      else if (value.Length >= 32 && value.Length <= 38) {
        value = value.Replace("{", "").Replace("}", "").Replace("-", "");
        if (!GuidRegEx.IsMatch(value)) {
          throw new FormatException("Guid string contained invalid characters.");
        }
        else {
#if NET40_OR_GREATER
          sg = new ShortGuid(Guid.Parse(value));
#else
          sg = new ShortGuid(new Guid(value));
#endif
        }
      }
      else {
        throw new FormatException("String was not in a valid format.");
      }

      return sg;
    }

    public static ShortGuid NewGuid() {
      return new ShortGuid(Guid.NewGuid());
    }

    public static string Encode(string value) {
      var guid = new Guid(value);
      return Encode(guid);
    }

    public static string Encode(Guid guid) {
      var encoded = Convert.ToBase64String(guid.ToByteArray());
      encoded = encoded.Replace("/", "_").Replace("+", "-");
      return encoded.Substring(0, 22);
    }

    public static Guid Decode(string value) {
      value = value.Replace("_", "/").Replace("-", "+");
      var buffer = value.EndsWith("==") ? Convert.FromBase64String(value) : Convert.FromBase64String(value + "==");
      return new Guid(buffer);
    }

    public static bool operator ==(ShortGuid x, ShortGuid y) => x._guid.Equals(y._guid);

    public static bool operator !=(ShortGuid x, ShortGuid y) => !(x == y);

    public static implicit operator string(ShortGuid shortGuid) => shortGuid._value;

    public static implicit operator Guid(ShortGuid shortGuid) => shortGuid._guid;

    public static implicit operator ShortGuid(string shortGuid) => new(shortGuid);

    public static implicit operator ShortGuid(Guid guid) => new(guid);
  }
}
