using System;
using REstate.Auth.Repositories;
using REstate.Repositories;
using REstate.Susanoo;

namespace REstate.Auth.Susanoo
{
    public static class REstateRepositoryExtensions
    {
        public static IAuthRepository GetAuthRepository(this IConfigurationRepository repository)
        {
            var root = repository as ConfigurationRepository;

            if(root == null) throw new ArgumentException("Type mismatch between root repository and auth library.", nameof(repository));

            return new AuthRepository(root);
        }
    }
}