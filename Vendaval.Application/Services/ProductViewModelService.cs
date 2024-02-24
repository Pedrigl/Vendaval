using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Infrastructure.Data.Contexts;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ProductViewModelService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductAvaliationRepository _productAvaliationRepository;
        private readonly IMapper _mapper;
        public ProductViewModelService(IProductRepository productRepository, IProductAvaliationRepository productAvaliationRepository, IMapper _mapper) 
        {
            _productRepository = productRepository;
            _productAvaliationRepository = productAvaliationRepository;
            _mapper = mapper;
        }


    }
}
