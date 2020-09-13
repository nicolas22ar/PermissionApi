using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using API.Test.Infrastructure.DTOs;
using API.Test.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Test.Webservice.Controllers
{
	/// <summary>
	/// API Controller for Permission
	/// </summary>
	[Route("[Controller]")]
	[ApiController]
	public class PermissionController : ControllerBase
	{
		private readonly IPermissionService _service;

		/// <summary>
		/// Permission API controller
		/// </summary>
		public PermissionController(IPermissionService service)
		{
			_service = service;
		}

		/// <summary>
		/// Returns a permission by its id
		/// </summary>
		/// <param name="id">Id</param>
		/// <returns></returns>
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<PermissionResponse>> GetPermission(
			[FromRoute(Name = "id")][Required] int id)
		{
			var result = await _service.GetAsync(id);

			if (result == null)
			{
				return NotFound();
			}

			return Ok(result);
		}

		/// <summary>
		/// Returns all the permissions
		/// </summary>
		/// <returns></returns>
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetPermissions()
		{
			return Ok(await _service.GetAllAsync());
		}

		/// <summary>
		/// Creates a new permission
		/// </summary>
		[HttpPost]
		[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
		public async Task<ActionResult<int>> Create([FromBody] PermissionRequest permission)
		{
			if (permission == null)
			{
				throw new ArgumentNullException(nameof(permission));
			}

			var id = await _service.CreateAsync(permission);

			if (permission == null)
			{
				return NotFound();
			}

			return Ok(id);
		}

		/// <summary>
		/// Updates a permission
		/// </summary>
		[HttpPut("{id}")]
		[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
		public async Task<ActionResult> Update(
			[FromRoute(Name = "id")][Required] int id,
			[FromBody] PermissionRequest permission)
		{
			if (permission == null)
			{
				throw new ArgumentNullException(nameof(permission));
			}

			await _service.UpdateAsync(id, permission);

			return Ok();
		}

		/// <summary>
		/// Deletes a permission
		/// </summary>
		[HttpDelete("{id}")]
		[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
		public async Task<ActionResult> Delete([FromRoute(Name = "id")][Required] int id)
		{
			await _service.DeleteAsync(id);

			return Ok();
		}

	}
}
