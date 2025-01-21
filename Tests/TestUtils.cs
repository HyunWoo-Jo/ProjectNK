using UnityEngine;
using NUnit.Framework;
namespace N.Test
{
    public static class TestUtils {
        public static void AssertWithDebug(bool condition, string message) {
            Debug.Assert(condition, message);
            Assert.IsTrue(condition, message);
        }
    }
}
