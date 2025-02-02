﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankClientApi.Data;
using BankClientApi.Models.Client;
using BankClientApi.Configurations;
using AutoMapper;
using BankClientApi.Contracts;

namespace BankClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IMapper _mapper;

        public ClientsController(IClientsRepository clientsRepository, IMapper mapper)
        {
            _clientsRepository = clientsRepository;
            _mapper = mapper;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetClientDto>>> GetClients()
        {
            var clients = await _clientsRepository.GetAllAsync();
            var records = _mapper.Map<List<GetClientDto>>(clients);
            return Ok(records);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetClientDto>> GetClient(int id)
        {
            var client = await _clientsRepository.GetAsync(id);
            var records = _mapper.Map<GetClientDto>(client);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(records);
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<CreateClientDto>> PostClient(CreateClientDto createClient)
        {
            var client = _mapper.Map<Client>(createClient);
            await _clientsRepository.AddAsync(client);

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _clientsRepository.GetAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            await _clientsRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> ClientExists(int id)
        {
            return await _clientsRepository.Exists(id);
        }
    }
}
