using Refit;
using Slayer.DTOS;

namespace Slayer.Interfaces
{
    interface IGetProduct
    {
        [Get("/catalog-cache/commercial/technical-products?commercialCode={code}")]
        Task<Root> GetProductAsync(string code);
    }
}