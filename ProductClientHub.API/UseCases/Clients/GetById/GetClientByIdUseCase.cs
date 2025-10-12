using Microsoft.EntityFrameworkCore;
using ProductClientHub.API.Infrastructure;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.UseCases.Clients.GetById
{
    public class GetClientByIdUseCase
    {

        public ResponseClientJson Execute(Guid id)
        {
            var dbContext = new ProductClientHubDBContext();
            var entity = dbContext
                .Clients
                .Include(client => client.Products)
                .FirstOrDefault(client => client.Id == id);
                
                       
            
            if (entity == null)
                throw new Exception("Client not found");
            return new ResponseClientJson
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                CodiceFiscale = entity.CodiceFiscale,
                Products = entity.Products.Select(product => new ResponseShortProductJson
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                }).ToList()

            };
                  
        }
    }
}
