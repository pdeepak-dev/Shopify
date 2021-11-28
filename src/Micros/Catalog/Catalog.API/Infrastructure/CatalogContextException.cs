using System;

namespace Catalog.API.Infrastructure
{
    public class CatalogContextException : Exception
    {
        public CatalogContextException(string message)
            : base(message)
        {
        }

        public override string Message 
            => $"{nameof(CatalogContextException)}: {base.Message}";

        public override string HelpLink { get; set; } = "https://github.com/mehmetozkaya/AspnetMicroservices";
    }
}