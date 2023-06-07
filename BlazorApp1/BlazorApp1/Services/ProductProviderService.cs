using BlazorApp1.Models;
using System.Collections.Immutable;

namespace BlazorApp1.Services
{
    public static class ProductProviderService
    {
        public static readonly ImmutableList<Product>
            Products = ImmutableList.CreateRange(new List<Product>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "All Roads lead to Blazor Server",
                    Description = "A tutorial for Blazor",
                    Price = 70
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "All roads lead to Blazor WASM",
                    Description = "A tutorial for WASM",
                    Price = 90
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "gRPC for Blazor WASM",
                    Description = "A tutorial for developing Backend for Blazor WASM",
                    Price = 100
                }
            });
    }
}

