using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Game.Drawing;
using CodeMagic.UI.Drawing;
using CodeMagic.UI.Services;
using Moq;
using NUnit.Framework;

namespace CodeMagic.UI.Tests
{
    [TestFixture]
    public class LightLevelManagerTests
    {
        [TestCase(LightLevel.Darkness, ExpectedResult = 0)]
        [TestCase(LightLevel.Dusk1, ExpectedResult = 25)]
        [TestCase(LightLevel.Dusk2, ExpectedResult = 50)]
        [TestCase(LightLevel.Dim1, ExpectedResult = 75)]
        [TestCase(LightLevel.Dim2, ExpectedResult = 100)]
        [TestCase(LightLevel.Medium, ExpectedResult = 125)]
        [TestCase(LightLevel.Bright1, ExpectedResult = 150)]
        [TestCase(LightLevel.Bright2, ExpectedResult = 175)]
        [TestCase(LightLevel.UltraBright1, ExpectedResult = 200)]
        [TestCase(LightLevel.UltraBright2, ExpectedResult = 225)]
        [TestCase(LightLevel.Blinding, ExpectedResult = 250)]
        public int ApplyLightLevel_AdjustsColor(LightLevel lightLevel)
        {
            // ARRANGE
            const int sourceColorBrightness = 125;
            const float brightness = 0.2F;

            var sourceColor = Color.FromArgb(sourceColorBrightness, sourceColorBrightness, sourceColorBrightness);
            var image = new SymbolsImage(1, 1);
            var pixel = image[0, 0];
            pixel.Symbol = '.';
            pixel.Color = sourceColor;
            pixel.BackgroundColor = sourceColor;

            var settingsServiceMock = new Mock<ISettingsService>();
            settingsServiceMock.SetupGet(service => service.Brightness).Returns(brightness);

            var objectUnderTest = new LightLevelManager(settingsServiceMock.Object);

            // ACT
            var result = objectUnderTest.ApplyLightLevel(image, lightLevel);

            // ASSERT
            var resultPixel = result[0, 0];
            Assert.AreEqual(pixel.Symbol, resultPixel.Symbol);
            Assert.IsNotNull(resultPixel.Color);
            Assert.IsNotNull(resultPixel.BackgroundColor);

            Assert.AreEqual(255, resultPixel.Color!.Value.A);
            Assert.AreEqual(255, resultPixel.BackgroundColor!.Value.A);

            Assert.AreEqual(resultPixel.Color!.Value.R, resultPixel.Color!.Value.B);
            Assert.AreEqual(resultPixel.Color!.Value.R, resultPixel.Color!.Value.G);

            Assert.AreEqual(resultPixel.Color!.Value.R, resultPixel.BackgroundColor!.Value.R);
            Assert.AreEqual(resultPixel.Color!.Value.R, resultPixel.BackgroundColor!.Value.G);
            Assert.AreEqual(resultPixel.Color!.Value.R, resultPixel.BackgroundColor!.Value.B);

            return resultPixel.Color!.Value.R;
        }
    }
}