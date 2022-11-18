using System;
namespace Common.Models.Product
{
    public class DecreaseAvailableCountProductMessage
    {
        public List<CheckProductIntegrationMessage> Products { get; set; }
    }
}

