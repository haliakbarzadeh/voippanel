using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO.Compression;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.Application.Features.Files.Commands.CreateFile;
using Goldiran.VOIPPanel.Application.Features.Files.Commands.DeleteFile;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Api.Models;
using Goldiran.VOIPPanel.Application.Common.Enums;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Hosting;
using Renci.SshNet;


namespace Goldiran.VOIPPanel.Api.Controllers;

//[Authorize(Policy = "file")]
public class FilesController : ApiControllerBase
{
    private readonly IConfiguration _configuration;

    public FilesController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// upload file
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    [HttpPost, DisableRequestSizeLimit]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<FileDisplayDto>> Create([FromForm] FileUploadModel file)
    {
        //var receivedFiles = files; //Request.Form.Files;
        if (file == null || file?.File?.Length == 0)
        {
            return BadRequest("there is no file in request body");

        }

        var receivedFile = file.File; //Request.Form.Files[0];
        var createFileCommand = new CreateFileCommand
        {
            FileOwnerTypeId = file.FileOwnerTypeId,
            Name = receivedFile.FileName,
            FileName = receivedFile.FileName,
            Length = receivedFile.Length,
            ContentType = receivedFile.ContentType,
            Content = ToByteArray(receivedFile),
            FileOwnerId = file.FileOwnerId

        };

        return Ok(await Mediator.Send(createFileCommand));

    }

    /// <summary>
    /// delete file
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteFileCommand { Id = id });
        return NoContent();
    }

    /// <summary>
    /// download file based on Id
    /// </summary>
    /// <param name="id"> the identifier of requested file</param>
    /// <returns> FileStreamResult</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get( Guid id)
    {
        var file = await Mediator.Send(new GetFileByIdQuery { Id = id });
        var stream = new MemoryStream(file.Content ?? throw new InvalidOperationException());
        return File(stream, file.ContentType, file.FileName);
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<FileRowDto>>> List([FromQuery] GetFilesQuery query)
    {
        return await Mediator.Send(query);
    }

    [AllowAnonymous]
    [Microsoft.AspNetCore.Mvc.Route("download")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Download(string fileName)
    {
        var ipAddress = _configuration.GetValue<string>("FileServerConfigs:IPAddress");
        var username = _configuration.GetValue<string>("FileServerConfigs:UserName");
        var password = _configuration.GetValue<string>("FileServerConfigs:Pass");
        var remoteFilePath = _configuration.GetValue<string>("FileServerConfigs:Path");
        var fullPath = _configuration.GetValue<string>("FileServerConfigs:FullPath");

        byte[] fileBytes = ReadFileFromSsh(ipAddress, username, password, $"{remoteFilePath}{fileName}");
        return await GetFile(fileBytes, fileName);
        // await GetFile(fileName,fullPath);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [NonAction]
    private byte[] ToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    private  byte[] ReadFileFromSsh(string host, string username, string password, string remoteFilePath)
    {
        using (var client = new SftpClient(host, username, password))
        {
            client.Connect();
            using (var memoryStream = new MemoryStream())
            {
                client.DownloadFile(remoteFilePath, memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>

    private async Task<IActionResult> GetFile(byte[] data, string fileName)
    {

        var contentType = "application /octet-stream"; // Default content type
        return File(data, contentType, fileName);
    }

    private async Task<IActionResult> GetFile(string fileName, string fullPath)
    {
        var data=System.IO.File.ReadAllBytes(fullPath+fileName);

        var contentType = "application /octet-stream"; // Default content type
        return File(data, contentType, fileName);
    }
}