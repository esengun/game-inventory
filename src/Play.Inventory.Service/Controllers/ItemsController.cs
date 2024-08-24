﻿using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Controllers
{
	[ApiController]
	[Route("items")]
	public class ItemsController: ControllerBase
	{
		private readonly IRepository<InventoryItem> inventoryItemsRepository;
		private readonly IRepository<CatalogItem> catalogItemRepository;

		public ItemsController(IRepository<InventoryItem> itemsRepository, IRepository<CatalogItem> catalogItemRepository)
		{
			this.inventoryItemsRepository = itemsRepository;
			this.catalogItemRepository = catalogItemRepository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				return BadRequest();
			}

			var inventoryItemEntities = await inventoryItemsRepository.GetAllAsync(item => item.UserId == userId);
			var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId);
			var catalogItemEntities = await catalogItemRepository.GetAllAsync(item => itemIds.Contains(item.Id));

			var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
			{
				var catalogItem = catalogItemEntities.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
				return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
			});

			return Ok(inventoryItemDtos);			
		}

		[HttpPost]
		public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
		{
			var inventoryItem = await inventoryItemsRepository.GetAsync(
				item => item.UserId == grantItemsDto.UserId	&& item.CatalogItemId == grantItemsDto.CatalogItemId);

			if (inventoryItem == null)
			{
				inventoryItem = new InventoryItem
				{
					CatalogItemId = grantItemsDto.CatalogItemId,
					UserId = grantItemsDto.UserId,
					Quantity = grantItemsDto.Quantity,
					AcquiredDate = DateTimeOffset.UtcNow
				};

				await inventoryItemsRepository.CreateAsync(inventoryItem);
			}
			else
			{
				inventoryItem.Quantity += grantItemsDto.Quantity;
				await inventoryItemsRepository.UpdateAsync(inventoryItem);
			}

			return Ok();
		}
	}
}
