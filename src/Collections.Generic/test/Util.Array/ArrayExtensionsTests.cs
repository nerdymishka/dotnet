using Mettle;
using NerdyMishka.Util.Arrays;
#pragma warning disable xUnit2013

namespace Tests
{
    public static class ArrayExtensionsTests
    {
        [UnitTest]
        public static void Clear(IAssert assert)
        {
            var listA = new[] { "alpha", "beta", "gamma", "delta" };
            var listB = new string[4];
            listA.CopyTo(listB, 0);

            assert.Check().Equal(4, listB.Length);
            assert.Contains("alpha", listB);

            listB.Clear();
            assert.DoesNotContain("alpha", listB);
            assert.DoesNotContain("beta", listB);
            assert.DoesNotContain("gamma", listB);
            assert.DoesNotContain("delta", listB);

            listA.CopyTo(listB, 0);
            listB.Clear(2, 2);
            assert.Contains("alpha", listB);
            assert.Contains("beta", listB);
            assert.DoesNotContain("gamma", listB);
            assert.DoesNotContain("delta", listB);
        }

        [UnitTest]
        public static void Grow(IAssert assert)
        {
            var list = System.Array.Empty<string>();

            assert.Check().Equal(0, list.Length);
            list = list.Grow(10);

            assert.Equal(10, list.Length);
        }

        [UnitTest]
        public static void Shrink(IAssert assert)
        {
            var list = new string[10];

            assert.Check().Equal(10, list.Length);
            list = list.Shrink(2);

            assert.Equal(8, list.Length);
        }
    }
}
#pragma warning restore xUnit2013