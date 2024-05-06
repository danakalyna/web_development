using NUnit.Framework;
using CommunityCenterApi.Helpers;
using System;

namespace CommunityCenterApiTests.Helpers
{
    [TestFixture]
    public class PasswordHasherTests
    {
        [Test]
        public void HashPassword_WhenCalled_ReturnsHashWithExpectedFormat()
        {
            var password = "TestPassword123!";
            var hash = PasswordHasher.HashPassword(password);

            // Expected format: {iterations}.{salt}.{hash}
            var parts = hash.Split('.');
            Assert.AreEqual(3, parts.Length);
            Assert.That(parts[0], Is.EqualTo("10000")); // Iterations
            Assert.That(parts[1].Length, Is.GreaterThan(0)); // Salt
            Assert.That(parts[2].Length, Is.GreaterThan(0)); // Key
        }

        [Test]
        public void CheckPassword_ValidHash_ReturnsVerifiedTrue()
        {
            var password = "ValidPassword!";
            var hash = PasswordHasher.HashPassword(password);

            var result = PasswordHasher.CheckPassword(hash, password);

            Assert.IsTrue(result.Verified);
            Assert.IsFalse(result.NeedsUpgrade);
        }

        [Test]
        public void CheckPassword_InvalidHash_ReturnsVerifiedFalse()
        {
            var password = "ValidPassword!";
            var wrongPassword = "InvalidPassword!";
            var hash = PasswordHasher.HashPassword(password);

            var result = PasswordHasher.CheckPassword(hash, wrongPassword);

            Assert.IsFalse(result.Verified);
        }

        [Test]
        public void CheckPassword_OldHashFormat_ReturnsNeedsUpgradeTrue()
        {
            var password = "NeedsUpgrade!";
            var hash = $"{1000}.{Convert.ToBase64String(new byte[16])}.{Convert.ToBase64String(new byte[32])}"; // Using a lower iteration count to simulate an old format

            var result = PasswordHasher.CheckPassword(hash, password);

            Assert.IsTrue(result.NeedsUpgrade);
        }

        [Test]
        public void CheckPassword_InvalidFormat_ThrowsFormatException()
        {
            var hash = "bad-format-hash";

            Assert.Throws<FormatException>(() => PasswordHasher.CheckPassword(hash, "password"));
        }
    }
}
