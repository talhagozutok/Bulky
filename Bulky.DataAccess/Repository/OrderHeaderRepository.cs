﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;

namespace Bulky.DataAccess.Repository;
public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
	private readonly ApplicationDbContext _dbContext;

	public OrderHeaderRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}

	public void Update(OrderHeader orderHeader)
	{
		_dbContext.OrderHeaders.Update(orderHeader);
	}
}
