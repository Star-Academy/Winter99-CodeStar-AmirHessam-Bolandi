using System;
using System.Collections.Generic;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SearchApi.Controllers
{
    [ApiController]
    public class SearchApiController
    {
        [Route("")]
        [HttpGet]
        public ActionResult<string> StartPage()
        {
            return new OkObjectResult("Hello \n" +
                                      "This is an Search API using Asp.net and Elastic Search\n" +
                                      "use GET /query/[...] To make a search query\n" +
                                      "use PUT /init/[Y/n] To initial SearchEngine\n");
        }
        
        
        [Route("init")]
        [Route("init/{isCreated}")]
        [ProducesResponseType(400)]
        [HttpGet]
        public ActionResult<string> Init(string isCreated)
        {
            string response = null;
            isCreated = isCreated == null ? "n" : isCreated.ToLower();
            if (isCreated == "y" || isCreated == "yes")
                response = SearchApi.Initialize(true);
            if (isCreated == "n" || isCreated == "no")
                response = SearchApi.Initialize(true);
            if (response == null)
                return new BadRequestResult();
            return new OkObjectResult(response);
        }

        //this method has some errors
        [Route("addDoc")]
        [ProducesResponseType(400)]
        [HttpPost]
        public ActionResult<string> AddDocument([FromQuery(Name = "path")]string path)
        {
            // Console.WriteLine(path+"<<<<<<<<<<<<");
            var response = SearchApi.PostNewData(path);
            if (response == null)
                return new BadRequestResult();

            return new OkObjectResult(response);
        }

        [Route("query/{queryContent}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpGet]
        public ActionResult<List<string>> GetQuery(string queryContent)
        {
            var response = SearchApi.GetQuery(queryContent);
            if (response == null)
                return new BadRequestResult();
            return new OkObjectResult(response);
        }
        
        [Route("query")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpGet]
        public ActionResult<List<string>> GetQuerySplitted([FromQuery(Name = "normals")]string normals="", [FromQuery(Name = "pluses")]string pluses="",[FromQuery(Name = "minuses")]string minuses="" )
        {
            var response = SearchApi.GetQuery(normals,pluses,minuses);
            if (response == null)
                return new BadRequestResult();
            return new OkObjectResult(response);
        }
        
        [Route("file/{fileName}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpGet]
        public ActionResult<List<string>> GetFileContent(string fileName)
        {
            var response = SearchApi.GetFileContent(fileName);
            if (response == null)
                return new BadRequestResult();
            return new OkObjectResult(response);
        }
    }
}