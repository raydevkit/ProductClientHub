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
                LastName = entity.LastName,
                Email = entity.Email,
                CodiceFiscale = entity.CodiceFiscale,
                Products = entity.Products.Select(product => new ResponseShortProductJson
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand,
                    Price = product.Price
                }).ToList()

            };
                  
        }
    }
}
