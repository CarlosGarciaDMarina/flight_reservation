using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightData.Data;
using FlightData.Models;      // si lo necesitas para el modelo
using FlightApi.DTOs;         // <-- añade esto

namespace FlightApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FlightsController(AppDbContext context)
        {
            _context = context;
        }

        // CRUD
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDto>>> GetFlights()
        {
            var flights = await _context.Flights
                .Select(f => new FlightDto
                {
                    Id = f.Id,
                    Origin = f.Origin,
                    DepartureTime = f.DepartureTime,
                    Destination = f.Destination
                })
                .ToListAsync();

            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDto>> GetFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null) return NotFound();

            return new FlightDto
            {
                Id = flight.Id,
                Origin = flight.Origin,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime
            };
        }

        /*
        [HttpPost]
        public async Task<ActionResult<FlightDto>> CreateFlightDto(CreateFlightDto dto)
        {
            var flight = new Flight
            {
                Origin = dto.Origin,
                Destination = dto.Destination,
                DepartureTime = dto.DepartureTime
            };

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            var result = new FlightDto
            {
                Id = flight.Id,
                Origin = flight.Origin,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime
            };

            return CreatedAtAction(nameof(GetFlight), new { Id = flight.Id }, result);
        }
        */

        [HttpPost]
        public async Task<ActionResult<FlightDto>> CreateFlight(CreateFlightDto dto)
        {
            // Mapeamos dto -> Entidad
            var flight = new Flight
            {
                Origin = dto.Origin,
                Destination = dto.Destination,
                DepartureTime = dto.DepartureTime
            };

            // Lo añadimos y gardamos
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            // Mapear Entidad -> Dto de respuesta
            var result = new FlightDto
            {
                Id = flight.Id,
                Origin = flight.Origin,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime
            };

            // Devolvemos OK!
            return CreatedAtAction(nameof(GetFlight), new { Id = flight.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight(int id, CreateFlightDto dto)
        {
            // Buscamos el id
            var flight = await _context.Flights.FindAsync(id);
            // Si no lo encontramos devolvemos no encontrado
            if (flight == null) return NotFound();

            // Si lo encontramos actualizamos los registros
            flight.Origin = dto.Origin;
            flight.Destination = dto.Destination;
            flight.DepartureTime = dto.DepartureTime;

            // Guardamos los cambios en la bbdd
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            // Buscamos el id
            var flight = await _context.Flights.FindAsync(id);
            // Si no lo encontramos devolvemos no encontrado
            if (flight == null) return NotFound();

            // Si lo encontramos borramos
            _context.Flights.Remove(flight);
            // Actualizamos BBDD
            await _context.SaveChangesAsync();
            return NoContent();
        }



    }
}