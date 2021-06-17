using AutoMapper;
using E_Invoice_API.Core.Interfaces.Repositories;
using E_Invoice_API.Core.MappingConfiguration;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Tests.Core.Services
{

    public abstract class BaseServiceTests
    {
        protected static Mock<TRepository> CreateMockedGenericRepositoryFromList<TRepository, TEntity>(IList<TEntity> list) where TRepository : class, IGenericRepository<TEntity> where TEntity : class
        {
            var mockedRepository = new Mock<TRepository>();

            // GetAsync
            mockedRepository
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<TEntity, bool>>>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>(),
                    It.IsAny<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>(),
                    It.IsAny<int?>()))
                .Returns(new Func<Expression<Func<TEntity, bool>>, CancellationToken, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>, int?, Task<IEnumerable<TEntity>>>
                        (async (expr, c, i, ob, t) => await Task.FromResult((ob?.Invoke(list.AsQueryable()) ?? list.AsQueryable()).Where(expr.Compile()))));

            //AnyAsync
            mockedRepository
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .Returns(new Func<Expression<Func<TEntity, bool>>, CancellationToken, Task<bool>>
                        (async (p, c) => await Task.FromResult(list.AsQueryable().Any(p))));

            //GetSingleAsync
            mockedRepository
                .Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), 
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>()))
                .Returns(new Func<Expression<Func<TEntity, bool>>, CancellationToken, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>, Task<TEntity>>
                        (async (expr, c, i) => await Task.FromResult((i?.Invoke(list.AsQueryable()) ?? list.AsQueryable()).SingleOrDefault(expr.Compile()))));

            return mockedRepository;
        }

        protected IMapper CreateMapper()
        {
            var mappingConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Configuration());
            });

            return mappingConfiguration.CreateMapper();
        }
    }
}
