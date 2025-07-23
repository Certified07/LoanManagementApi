using LoanManagementApi.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementApi.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientService.GetAllClients();
            return Ok(clients);
        }

        [HttpGet("{clientId}")]
        [Authorize(Roles = "Admin, Client")]
        public async Task<IActionResult> GetClientDetails(string clientId)
        {
            var result = await _clientService.GetClientWithLoansAsync(clientId);
            if (result == null)
                return NotFound(new { message = "Client not found" });

            return Ok(result);
        }
    }
}
