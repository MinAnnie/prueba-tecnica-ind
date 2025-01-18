using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prueba_tecnica.Models;
using prueba_tecnica.Services;
using prueba_tecnica.Utils.Errors;

namespace prueba_tecnica.Controllers
{
    /// <summary>
    /// Controlador para gestionar productos.
    /// Este controlador permite realizar operaciones CRUD sobre los productos, como obtener todos los productos, obtener un producto por su ID, crear, actualizar y eliminar productos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <returns>Lista de productos</returns>
        /// <response code="200">Retorna la lista de productos</response>
        /// <response code="500">Si ocurre un error interno en el servidor</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (AppException ex)
            {
                return HandleAppException(ex);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                return StatusCode(500,
                    new { Code = ErrorCodes.GeneralErrors.InternalServerError, Message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto a obtener</param>
        /// <returns>Producto con el ID especificado</returns>
        /// <response code="200">Retorna el producto encontrado</response>
        /// <response code="404">Si no se encuentra el producto</response>
        /// <response code="500">Si ocurre un error interno en el servidor</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new
                        { Code = ErrorCodes.ProductErrors.ProductNotFound, Message = "Producto no encontrado" });
                }

                return Ok(product);
            }
            catch (AppException ex)
            {
                return HandleAppException(ex);
            }
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="product">Producto a crear</param>
        /// <returns>El producto creado</returns>
        /// <response code="201">Retorna el producto creado</response>
        /// <response code="500">Si ocurre un error interno en el servidor</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            try
            {
                await _productService.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (AppException ex)
            {
                return HandleAppException(ex);
            }
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar</param>
        /// <param name="product">Producto con los nuevos valores</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="204">Indica que la actualización fue exitosa</response>
        /// <response code="400">Si el ID no coincide con el producto proporcionado</response>
        /// <response code="500">Si ocurre un error interno en el servidor</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest(new
                    {
                        Code = ErrorCodes.GeneralErrors.InvalidRequest, Message = "El ID del producto no coincide."
                    });
                }

                await _productService.UpdateProductAsync(product);
                return NoContent();
            }
            catch (AppException ex)
            {
                return HandleAppException(ex);
            }
        }

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="204">Indica que el producto fue eliminado exitosamente</response>
        /// <response code="500">Si ocurre un error interno en el servidor</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (AppException ex)
            {
                return HandleAppException(ex);
            }
        }

        private IActionResult HandleAppException(AppException ex)
        {
            return StatusCode(500, new
            {
                Code = ex.Code,
                Message = ex.Message
            });
        }
    }
}