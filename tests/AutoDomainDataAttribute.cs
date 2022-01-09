namespace BMS.Tests;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

public class AutoDomainDataAttribute : AutoDataAttribute
{
    public AutoDomainDataAttribute()
        : base(() => new Fixture().Customize(new AutoMoqCustomization()))
    {
    }
}

