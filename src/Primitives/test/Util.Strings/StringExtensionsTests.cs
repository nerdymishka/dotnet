using Mettle;
using NerdyMishka.Util.Strings;

public class StringExtensionsTests
{
    private IAssert assert;

    public StringExtensionsTests(IAssert assert)
    {
        this.assert = assert;
    }

    [UnitTest]
    public void Equals_CurrentCulture_IgnoreCase()
    {
        // TODO: consider getting strings from different cultures
        var result = "iHazCats".Equals("IHAZCATS", true);
        assert.Ok(result);

        result = "iHazCatz".Equals("IHAZCATS", true);
        assert.Ok(!result);
    }

    [UnitTest]
    public void Equals_CurrentCulture_MatchCase()
    {
        var result = "iHazCats".Equals("IHAZCATS", false);
        assert.Ok(!result);

        result = "IHAZCATS".Equals("IHAZCATS", false);
        assert.Ok(result);
    }

    [UnitTest]
    public void Equals_Ordinal_IgnoreCase()
    {
        var result = "iHazCats".EqualsOrdinal("IHAZCATS", true);
        assert.Ok(result);

        result = "iHazCatz".EqualsOrdinal("IHAZCATS", true);
        assert.Ok(!result);
    }

    [UnitTest]
    public void Equals_Ordinal_MatchCase()
    {
        var result = "iHazCats".EqualsOrdinal("IHAZCATS", false);
        assert.Ok(!result);

        result = "IHAZCATS".EqualsOrdinal("IHAZCATS", false);
        assert.Ok(result);
    }

    [UnitTest]
    public void Equals_Invariant_IgnoreCase()
    {
        var result = "iHazCats".EqualsInvariant("IHAZCATS", true);
        assert.Ok(result);

        result = "iHazCatz".EqualsInvariant("IHAZCATS", true);
        assert.Ok(!result);
    }

    [UnitTest]
    public void Equals_Invariant_MatchCase()
    {
        var result = "iHazCats".EqualsInvariant("IHAZCATS", false);
        assert.Ok(!result);

        result = "IHAZCATS".EqualsInvariant("IHAZCATS", false);
        assert.Ok(result);
    }

    [UnitTest]
    public void Strip_Unwanted_Values()
    {
        var result = "abcdefghijklmnopqrz".Strip("c", "gh", "z", "nop");
        assert.Equal("abdefijklmqr", result);
    }
}
