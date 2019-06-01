﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalendarEvents.Models;
using CalendarEvents.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalendarEvents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        //TODO: remove this dependency into IService, IRepository using autofac.
        private readonly IEventsService _eventsService;        

        public EventsController(IEventsService eventsService)
        {
            this._eventsService = eventsService;
        }

        // GET api/events
        [HttpGet]
        public ActionResult<IEnumerable<EventModel>> Get()
        {
            ResultService<IEnumerable<EventModel>> result = this._eventsService.GetAllItems();
            if (result.Success)
            {
                return Ok(result.Value);
            }
            else
            {
                return StatusCode(500, result.ErrorCode);
            }            
        }

        // GET api/events/c4df7159-2402-4f49-922c-1a2caef02de2
        [HttpGet("{id}", Name = "GET")]
        public ActionResult<EventModel> Get(Guid id)
        {
            ResultService<EventModel> result = this._eventsService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Value);
            }
            else
            {
                return StatusCode(500, result.ErrorCode);
            }
        }

        // POST api/events
        [HttpPost]
        public ActionResult Post([FromBody] EventModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ResultService result = this._eventsService.Add(item);
            if (result.Success)
            {
                return CreatedAtAction("Post", new { item.Id }, result);
            }
            else
            {
                return StatusCode(500, result.ErrorCode);
            }
        }

        // PUT api/events/c4df7159-2402-4f49-922c-1a2caef02de2
        [HttpPut("{id}")]
        public ActionResult Put(Guid id, [FromBody] EventModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ResultService result = this._eventsService.Update(id, item);
            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, result.ErrorCode);
            }
        }

        // DELETE api/events/c4df7159-2402-4f49-922c-1a2caef02de2
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ResultService result = this._eventsService.Remove(id);
            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, result.ErrorCode);
            }
        }
    }
}