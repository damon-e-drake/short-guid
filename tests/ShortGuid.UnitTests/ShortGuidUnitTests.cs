using System;
using Xunit;
using DEDrake;

namespace UnitTests {
  public class ShortGuidUnitTests {
    [Theory(DisplayName = "Convert Guid to ShortGuid.")]
    [InlineData("751563d4-0386-4724-99a2-ebd1b7f816c8", "1GMVdYYDJEeZouvRt_gWyA")]
    [InlineData("D134E666C3DF447AB74B3706F4DAB147", "ZuY00d_DekS3SzcG9NqxRw")]
    [InlineData("0d25159a-83d1-444d-9b8d-10c8a5175ebb", "mhUlDdGDTUSbjRDIpRdeuw")]
    [InlineData("b98baba2-1d9f-4c5c-bdba-951d14b3eed6", "oquLuZ8dXEy9upUdFLPu1g")]
    [InlineData("7c1f6c3a-1015-4f2c-87e1-f9ea077364fc", "OmwffBUQLE-H4fnqB3Nk_A")]
    public void ShouldConvertGuidToShortGuid(string guid, string sguid) {
      var id = Guid.Parse(guid);
      var sg = new ShortGuid(id);

      Assert.NotEqual(id, Guid.Empty);
      Assert.Equal(sg, sguid);
      Assert.Equal(sg.Guid, id);
      Assert.True(sg.Equals(id));
      Assert.Equal((Guid)sg, id);
      Assert.Equal(22, sg.ToString().Length);
    }

    [Theory(DisplayName = "Convert Guid to ShortGuid.")]
    [InlineData("751563d4-0386-4724-99a2-ebd1b7f816c8", "1GMVdYYDJEeZouvRt_gWyA")]
    [InlineData("D134E666C3DF447AB74B3706F4DAB147", "ZuY00d_DekS3SzcG9NqxRw")]
    [InlineData("{0d25159a-83d1-444d-9b8d-10c8a5175ebb}", "mhUlDdGDTUSbjRDIpRdeuw")]
    [InlineData("b98baba2-1d9f-4c5c-bdba-951d14b3eed6", "oquLuZ8dXEy9upUdFLPu1g==")]
    [InlineData("7c1f6c3a-1015-4f2c-87e1-f9ea077364fc", "OmwffBUQLE-H4fnqB3Nk_A")]
    public void ShouldParseShortGuid(string guid, string sguid) {
      var sgGuid = ShortGuid.Parse(guid);
      var sgShort = ShortGuid.Parse(sguid);

      Assert.Equal(sgGuid, sgShort);
    }

    [Theory(DisplayName = "Should create ShortGuid from string assignment")]
    [InlineData("1GMVdYYDJEeZouvRt/gWyA==")]
    [InlineData("ZuY00d_DekS3SzcG9NqxRw")]
    [InlineData("mhUlDdGDTUSbjRDIpRdeuw")]
    [InlineData("oquLuZ8dXEy9upUdFLPu1g")]
    [InlineData("OmwffBUQLE+H4fnqB3Nk/A")]
    public void ShouldCreateShortGuidFromString(string id) {
      ShortGuid sg = id;

      Assert.NotEqual(sg, ShortGuid.Empty);
    }

    [Theory(DisplayName = "Show throw exception for invalid strings")]
    [InlineData("751563d4/0386-4724-99a2-ebd1b7f816c8")]
    [InlineData("ZZ34E666C3DF447AB74B3706F4DAB147")]
    [InlineData("X0d25159a-83d1-444d-9b8d-10c8a5175ebb}")]
    [InlineData("ZuY00d?DekS3SzcG9NqxRw")]
    [InlineData("ZuY00d_DekS3SzcG9Nqx")]
    public void ShouldThrowParseExceptions(string id) {
      Assert.Throws<FormatException>(() => ShortGuid.Parse(id));
    }
  }
}
