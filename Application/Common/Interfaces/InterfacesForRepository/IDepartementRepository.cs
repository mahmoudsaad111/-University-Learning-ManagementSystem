﻿using Application.Common.Interfaces.Presistance;
using Contract.Dto;
using Contract.Dto.Departements;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.InterfacesForRepository
{
    public interface IDepartementRepository :IBaseRepository<Departement>
    {
        public Task<Departement> GetDepartementHasAcadimicYearId(int acadimicyearId);
        public Task<IEnumerable<Departement>> GetDepartementsBelongsToFaculty(int FacultyId);          
    }
}
