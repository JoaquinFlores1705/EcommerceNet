﻿using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery]ProductSpecificationParams productParams)
        {
            var spec = new ProductWithCategoryAndBrandSpecification(productParams);
            var products = await _productRepository.GetAllWithSpec(spec);
            var specCount = new ProductForCountingSpecification(productParams);
            int totalProducts = await _productRepository.CountASync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalProducts) / productParams.PageSize);
            var totalPages = Convert.ToInt32(rounded);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);
            return Ok(
                    new Pagination<ProductDto>
                    {
                        Count = totalProducts,
                        Data = data,
                        PageCount = totalPages,
                        PageIndex = productParams.PageIndex,
                        PageSize = productParams.PageSize,
                    }
                );
            
            //return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductWithCategoryAndBrandSpecification(id);
            var product = await _productRepository.GetByIdWithSpec(spec);

            if(product == null)
            {
                return NotFound(new CodeErrorResponse(404, "El producto no existe"));
            }

            return _mapper.Map<Product, ProductDto>(product);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            var result = await _productRepository.Add(product);
            if(result == 0)
            {
                throw new Exception("No se inserto el producto");
            }

            return Ok(product);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(int id, Product product)
        {
            product.Id = id;
            var result = await _productRepository.Update(product);
            if (result == 0)
            {
                throw new Exception("No se actualizo el producto");
            }

            return Ok(product);
        }

    }
}
