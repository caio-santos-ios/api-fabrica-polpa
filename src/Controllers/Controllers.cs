using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulpaAPI.src.Interfaces;
using PulpaAPI.src.Models.Base;
using PulpaAPI.src.Shared.DTOs;

namespace PulpaAPI.src.Controllers
{
    // ─── PRODUCT ──────────────────────────────────────────────────────────────
    [Route("api/products")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await productService.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            ResponseApi<dynamic?> response = await productService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, new { response.Result });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await productService.CreateAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await productService.UpdateAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await productService.DeleteAsync(id);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }

    // ─── STOCK ────────────────────────────────────────────────────────────────
    [Route("api/stock")]
    [ApiController]
    public class StockController(IStockService stockService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await stockService.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            ResponseApi<dynamic> response = await stockService.GetAlertsAsync();
            return StatusCode(response.StatusCode, new { response.Result });
        }

        [Authorize]
        [HttpPost("batches")]
        public async Task<IActionResult> AddBatch([FromBody] CreateStockBatchDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await stockService.AddBatchAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpPost("losses")]
        public async Task<IActionResult> RegisterLoss([FromBody] RegisterLossDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await stockService.RegisterLossAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
    }

    // ─── SALE ─────────────────────────────────────────────────────────────────
    [Route("api/sales")]
    [ApiController]
    public class SaleController(ISaleService saleService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await saleService.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            ResponseApi<dynamic?> response = await saleService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, new { response.Result });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await saleService.CreateAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            var response = await saleService.CancelAsync(id);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
    }

    // ─── CUSTOMER ─────────────────────────────────────────────────────────────
    [Route("api/customers")]
    [ApiController]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await customerService.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            ResponseApi<dynamic?> response = await customerService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, new { response.Result });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await customerService.CreateAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await customerService.UpdateAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await customerService.DeleteAsync(id);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }

    // ─── SUPPLIER ─────────────────────────────────────────────────────────────
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController(ISupplierService supplierService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await supplierService.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            ResponseApi<dynamic?> response = await supplierService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, new { response.Result });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await supplierService.CreateAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSupplierDTO dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            var response = await supplierService.UpdateAsync(dto);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await supplierService.DeleteAsync(id);
            return StatusCode(response.StatusCode, new { response.Message });
        }
    }

    // ─── REPORT ───────────────────────────────────────────────────────────────
    [Route("api/reports")]
    [ApiController]
    public class ReportController(IReportService reportService) : ControllerBase
    {
        [Authorize]
        [HttpGet("sales-summary")]
        public async Task<IActionResult> GetSalesSummary([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            ResponseApi<dynamic> response = await reportService.GetSalesSummaryAsync(from, to);
            return StatusCode(response.StatusCode, new { response.Result });
        }

        [Authorize]
        [HttpGet("product-profit")]
        public async Task<IActionResult> GetProductProfit([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            ResponseApi<dynamic> response = await reportService.GetProductProfitAsync(from, to);
            return StatusCode(response.StatusCode, new { response.Result });
        }

        [Authorize]
        [HttpGet("stock-losses")]
        public async Task<IActionResult> GetStockLosses([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            ResponseApi<dynamic> response = await reportService.GetStockLossesAsync(from, to);
            return StatusCode(response.StatusCode, new { response.Result });
        }
    }
}
