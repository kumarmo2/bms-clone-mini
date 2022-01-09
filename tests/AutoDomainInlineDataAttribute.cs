using AutoFixture.Xunit2;

namespace BMS.Tests;

public class AutoDomainInlineDataAttribute : InlineAutoDataAttribute
{

    public AutoDomainInlineDataAttribute(params object[] objects)
        : base(new AutoDomainDataAttribute(), objects) { }
}

