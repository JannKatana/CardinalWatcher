using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Attendances
{
    public class Report
    {
        public class Query : IRequest<FileStreamResult>
        {
            public ReportDto Report { get; set; }
        }

        //public class Handler : IRequestHandler<Query, FileStreamResult>
        //{
        //    private readonly DataContext _context;
        //    private readonly IMapper _mapper;
        //    private readonly IUserAccessor _userAccessor;
        //    private readonly IHostingEnvironment _hostingEnvironment;

        //    public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor, IHostingEnvironment hostingEnvironment)
        //    {
        //        _context = context;
        //        _mapper = mapper;
        //        _userAccessor = userAccessor;
        //        _hostingEnvironment = hostingEnvironment;
        //    }

        //    private object PhysicalFile(string v1, string v2)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
