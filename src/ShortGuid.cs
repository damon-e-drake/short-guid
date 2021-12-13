namespace DEDrake.ShortGuid {
  public struct ShortGuid {
    public readonly static ShortGuid Empty = new(Guid.Empty);

    private readonly Guid _guid;
    private readonly string _value;

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

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    public override bool Equals(object obj) {
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
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

    public static ShortGuid NewGuid() {
      return new(Guid.NewGuid());
    }

    public static string Encode(string value) {
      var guid = new Guid(value);
      return Encode(guid);
    }

    public static string Encode(Guid guid) {
      var encoded = Convert.ToBase64String(guid.ToByteArray());
      encoded = encoded.Replace("/", "_").Replace("+", "-");
      return encoded[..22];
    }

    public static Guid Decode(string value) {
      value = value.Replace("_", "/").Replace("-", "+");
      var buffer = Convert.FromBase64String(value + "==");
      return new Guid(buffer);
    }

    public static bool operator ==(ShortGuid x, ShortGuid y) => x._guid == y._guid;

    public static bool operator !=(ShortGuid x, ShortGuid y) => !(x == y);

    public static implicit operator string(ShortGuid shortGuid) => shortGuid._value;

    public static implicit operator Guid(ShortGuid shortGuid) => shortGuid._guid;

    public static implicit operator ShortGuid(string shortGuid) => new(shortGuid);

    public static implicit operator ShortGuid(Guid guid) => new(guid);
  }
}
