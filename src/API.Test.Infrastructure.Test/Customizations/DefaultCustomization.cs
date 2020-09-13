using AutoFixture;
using AutoFixture.AutoMoq;

namespace API.Test.Infrastructure.Test.Customizations
{
    public class DefaultCustomization : CompositeCustomization
    {
        public DefaultCustomization()
            : base(new AutoMoqCustomization())
        { 
        
        }
    }
}
