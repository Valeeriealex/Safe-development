using Consul;
using Microsoft.Extensions.Configuration;

namespace SafeDevelopment
{
public class ConsulServiceRegistration
    {
        private readonly IConfiguration _configuration;

        public ConsulServiceRegistration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void RegisterService()
        {
            var consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri(_configuration.GetConnectionString("Consul"));
            });

            var serviceId = $"{_configuration["ServiceName"]}-{Environment.MachineName}";
            var registration = new AgentServiceRegistration
            {
                ID = serviceId,
                Name = _configuration["ServiceName"],
                Address = _configuration["ServiceAddress"],
                Port = _configuration.GetValue<int>("ServicePort"),
                Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),
                    Interval = TimeSpan.FromSeconds(5),
                    HTTP = $"http://{_configuration["ServiceAddress"]}:{_configuration.GetValue<int>(\"ServicePort\")}/health/ready",
                    Timeout = TimeSpan.FromSeconds(3)
                }
            };

        consulClient.Agent.ServiceRegister(registration).Wait();
        }
    }
}
