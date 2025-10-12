﻿using ProductClientHub.API.Entities;
using ProductClientHub.API.Infrastructure;
using ProductClientHub.API.UseCases.Clients.SharedValidator;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.API.UseCases.Clients.Register
{
    public class RegisterClientUseCase
    {
        public ResponseShortClientJson Execute(RequestClientJson request)
        {
            Validate(request);

            var dbContext = new ProductClientHubDBContext();

            var entity = new Client

            {
                Name = request.Name,
                LastName = request.LastName,
                CodiceFiscale = request.CodiceFiscale,
                Email = request.Email                
            };

            dbContext.Clients.Add(entity);

            dbContext.SaveChanges();

            return new ResponseShortClientJson
            {
                Id = entity.Id,
                Name = entity.Name
            };
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
