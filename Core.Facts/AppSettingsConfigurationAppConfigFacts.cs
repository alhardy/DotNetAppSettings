
namespace Core.Facts
{
    using Autofac;
    using System.Configuration;
    using Xunit;

    #region sample

    public class EcommerceSettings
    {

        public EcommerceSettings(string paymentProvider, int taxAmountPercentage)
        {
            PaymentProvider = paymentProvider;
            TaxAmountPercentage = taxAmountPercentage;
        }

        public string PaymentProvider { get; private set; }
        public int TaxAmountPercentage { get; private set; }
    }

    public interface IPaymentService
    {
        string PaymentProvider { get; }
        int TaxAmountPercentage { get; }
    }

    public class PaymentService : IPaymentService
    {
        private readonly EcommerceSettings _settings;

        public PaymentService(EcommerceSettings settings)
        {
            _settings = settings;
        }

        public string PaymentProvider { get { return _settings.PaymentProvider; } }
        public int TaxAmountPercentage { get { return _settings.TaxAmountPercentage; } }
    }

    #endregion


    public class AppSettingsConfigurationAppConfigFacts
    {
        private IContainer _container;

        public AppSettingsConfigurationAppConfigFacts()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<PaymentService>().As<IPaymentService>();
            builder.Register(c => c.Resolve<IAppSettingsConfiguration<EcommerceSettings>>().Load()).SingleInstance();

            builder.RegisterGeneric(typeof(AppSettingsConfiguration<>)).As(typeof(IAppSettingsConfiguration<>)).WithParameter("settings", ConfigurationManager.AppSettings);

            _container = builder.Build();
        }

        [Fact]
        public void TestOne()
        {
            // Arrange                    
            var service = _container.Resolve<IPaymentService>();

            // Act
            var provider = service.PaymentProvider;
            var taxPercentage = service.TaxAmountPercentage;

            // Assert
            Assert.True(provider == "eway");
            Assert.True(taxPercentage == 10);
        }
    }
}
