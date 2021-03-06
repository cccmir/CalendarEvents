﻿using AutoMapper;
using CalendarEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarEvents
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<EventModel, EventModelDTO>();
            CreateMap<EventModelDTO, EventModel>();
            CreateMap<EventModel, EventPostRequest>();
            CreateMap<EventPostRequest, EventModel>();
            CreateMap<EventModel, EventPutRequest>();
            CreateMap<EventPutRequest, EventModel>();
            CreateMap<GetRequest<EventModelDTO>, GetRequest<EventModel>>();
            CreateMap<GetRequest<EventModel>, GetRequest<EventModelDTO>>();
        }
    }
}
