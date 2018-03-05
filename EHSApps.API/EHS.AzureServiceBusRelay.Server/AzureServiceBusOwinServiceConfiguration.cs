using Microsoft.ServiceBus;

namespace EHS.AzureServiceBusRelay.Server
{
    public class AzureServiceBusOwinServiceConfiguration
    {
        public string IssuerName { get; private set; }
        public string IssuerSecret { get; private set; }
        public string Address { get; private set; }

        public AzureServiceBusOwinServiceConfiguration(string issuerName, string issuerSecret, string address)
        {
            IssuerName = issuerName;
            IssuerSecret = issuerSecret;
            Address = address;
        }

        public TransportClientEndpointBehavior GetTransportBehavior()
        {
            return new TransportClientEndpointBehavior
            {
                TokenProvider =
                    TokenProvider.CreateSharedAccessSignatureTokenProvider(IssuerName, IssuerSecret)
            };
        }
    }
}