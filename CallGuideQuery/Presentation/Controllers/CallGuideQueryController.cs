using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Requests;
using Domain.Entity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Presentation.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class CallGuideQueryController : Controller
    {
        private readonly ICallGuideMongoDbRepository<CallGuide> repository;
        public CallGuideQueryController(ICallGuideMongoDbRepository<CallGuide> _repository)
        {
            repository=_repository;
        }
        [HttpPost]
        public async Task<bool> CreateAsync(CreateCallGuideRequest model)
        {
            var result = false;
            try
            {
                var callguide = new CallGuide{
                    Id = model.Id,
                    Company= model.Company,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    UserId = model.UserId
                };
                if(model.CommunicationInfos!=null)
                {
                    model.CommunicationInfos.ForEach(item=>{
                        callguide.CommunicationInfos.Add(new CommunicationInfo(item));
                    });
                    
                }
                result = await repository.CreateAsync(callguide);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        [HttpGet("GetWithID")]
        public IEnumerable<CallGuide> GetWhere(int expression)
        {
            try
            {
                var result = repository.Where(p=>p.Id==expression); 
                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        [HttpGet("GetAllCallGuideWithUserID")]
        public IEnumerable<string> GetAllCallGuide(int userId)
        {
            try
            {
                List<string> allguide = new List<string>();
                var result = repository.Where(p=>p.UserId==userId).ToList();
                result.ForEach(p=>{
                    allguide.Add(p.Firstname +" "+p.Lastname);
                });
                return allguide;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        [HttpPost("AddComInfo")]
        public async Task<bool> AddComInfo(AddComInfoRequest request)
        {
            try
            {
                // var Id = new ObjectId(request.GuideId.ToString());
                
                var callGuide = await repository.FindById(request.GuideId);
                var cominfo = new CommunicationInfo
                {
                    Email =request.Email,
                    Phone = request.Phone,
                    Location = request.Location
                };
                callGuide.CommunicationInfos.Add(cominfo);
                var result = await repository.UpdateAsync(callGuide);
                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        [HttpPost("RemoveComInfo")]
        public async Task<bool> RemoveComInfo(RemoveComInfoRequest request)
        {
            try
            {
                var callGuide = repository.Where(p=>p.Id==request.GuideId && p.UserId==request.UserId).FirstOrDefault();
                var cominfo = callGuide.CommunicationInfos.Find(p=>p.Id==request.Id);
                callGuide.CommunicationInfos.Remove(cominfo);
                var result = await repository.UpdateAsync(callGuide);
                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}