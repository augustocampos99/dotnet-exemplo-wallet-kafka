using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Contracts;
using Wallet.Api.Services;
using Wallet.Api.Services.Interfaces;

namespace Wallet.Api.Controllers
{
    [Route("wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            this._walletService = walletService;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            int limit = 10;
            int skip = 0;

            if (!String.IsNullOrEmpty(Request.Query["limit"]) && !String.IsNullOrEmpty(Request.Query["skip"]))
            {
                try
                {
                    limit = Int32.Parse(Request.Query["limit"]);
                    skip = Int32.Parse(Request.Query["skip"]);
                }
                catch (Exception ex)
                {
                    return BadRequest("Invalid parameter!");
                }
            }

            var result = await this._walletService.FindAll(skip, limit);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var result = await this._walletService.FindById(id);
            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] WalletRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await this._walletService.Create(request);
                    return Ok(result);
                }
                catch (BadRequestException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest("Server error. Contact I.T" + ex.Message);
                }
            }

            return BadRequest("");
        }

        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> Deposit(Guid id, [FromBody] TransactionRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await this._walletService.Deposit(id, request);
                    return Ok(result);
                }
                catch (BadRequestException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest("Server error. Contact I.T" + ex.Message);
                }
            }

            return BadRequest("");
        }

        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(Guid id, [FromBody] TransactionRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await this._walletService.Withdraw(id, request);
                    return Ok(result);
                }
                catch (BadRequestException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest("Server error. Contact I.T" + ex.Message);
                }
            }

            return BadRequest("");
        }

        [HttpPost("{id}/transfer")]
        public async Task<IActionResult> Transfer(Guid id, [FromBody] TransferRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await this._walletService.Transfer(id, request);
                    return Ok(result);
                }
                catch (BadRequestException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest("Server error. Contact I.T" + ex.Message);
                }
            }

            return BadRequest("");
        }

    }
}
