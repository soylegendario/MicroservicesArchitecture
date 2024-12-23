using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Inventory.UnitTests;

public class AutoMoqDataAttribute() : AutoDataAttribute(() => new Fixture()
    .Customize(new AutoMoqCustomization()));