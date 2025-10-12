﻿using ProductClientHub.API.Infrastructure;
using ProductClientHub.API.UseCases.Clients.SharedValidator;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.API.UseCases.Clients.Update
{
    public class UpdateClientUseCase
    {

        public void Execute(Guid clientId, RequestClientJson request)
        {
            Validate(request);

            var dbContext = new ProductClientHubDBContext();

            var entity = dbContext.Clients.FirstOrDefault(client => client.Id == clientId);
            if (entity is null)
            {
                throw new NotFoundException($"Client with ID {clientId} not found.");
            }
            entity.Name = request.Name;
            entity.Email = request.Email;
            entity.LastName = request.LastName;
            entity.CodiceFiscale = request.CodiceFiscale;

            dbContext.Clients.Update(entity);
            dbContext.SaveChanges();
        }

        private void Validate(RequestClientJson request)
        {
            var validator = new RequestClientValidator();

            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var errors = result.Errors.Select(failure => failure.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
        

    }
}
